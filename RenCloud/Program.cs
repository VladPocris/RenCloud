using RenCloud;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RenCloud
{ 
    internal class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        //ROUNDCORNERS//
        public class Corners
        {
            // Import dwmapi.dll and define DwmSetWindowAttribute.
            [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
            internal static extern void DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref int pvAttribute, uint cbAttribute);

            // The DWM_WINDOW_CORNER_PREFERENCE enum for DwmSetWindowAttribute's third parameter.
            public enum DWM_WINDOW_CORNER_PREFERENCE
            {
                DWMWCP_DEFAULT = 0,
                DWMWCP_DONOTROUND = 1,
                DWMWCP_ROUND = 2,
                DWMWCP_ROUNDSMALL = 3
            }

            // The DWMWINDOWATTRIBUTE enum parameters to be set
            public enum DWMWINDOWATTRIBUTE
            {
                DWMWA_WINDOW_CORNER_PREFERENCE = 33,
                DWMWA_BORDER_COLOR = 34
            }
            // Pass attributes for DWMWINDOWATTRIBUTE.
            public void AttributesRoundCorners(Form form, bool isActive)
            {
                Color borderColor = isActive ? Color.FromArgb(255, 153, 164) : Color.FromArgb(89, 76, 255);
                IntPtr hWnd = form.Handle;
                int cornerPreference = (int)DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
                DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref cornerPreference, sizeof(int));
                int colorValue = (borderColor.B << 16) | (borderColor.G << 8) | borderColor.R;
                DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_BORDER_COLOR, ref colorValue, sizeof(int));
            }
        }

        //SMOOTHGIFANIMATION//

        // Timer for GIF animation.
        public class GifAnimation
        {
            private Timer frameTimer;
            private Bitmap animatedImage;
            private int currentFrame;
            private PictureBox targetPictureBox;
            //AttachmentEvent
            public GifAnimation(PictureBox pictureBox)
            {
                targetPictureBox = pictureBox;
                AttachGifEvent();
            }
            public void AttachGifEvent()
            {
                targetPictureBox.Paint += new PaintEventHandler(FrameUpdate);
            }
            // Initializaton for smooth gif animation.
            public void InitializeGifAnimation(Bitmap customGif)
            {
                animatedImage = customGif;
                currentFrame = 0;
                frameTimer = new Timer
                {
                    Interval = 15
                };
                frameTimer.Tick += new EventHandler(OnFrameChanged);
                frameTimer.Start();
            }
            private void OnFrameChanged(object sender, EventArgs e)
            {
                TickUpdate(targetPictureBox);
            }
            //Update the current frame of the animation.
            public void FrameUpdate(object sender, PaintEventArgs e)
            {
                if (animatedImage != null)
                {
                    ImageAnimator.UpdateFrames(animatedImage);
                    e.Graphics.DrawImage(animatedImage, targetPictureBox.ClientRectangle);
                }
            }
            //Tick frame update
            public void TickUpdate(PictureBox targetPictureBox)
            {
                currentFrame++;
                if (currentFrame >= animatedImage.GetFrameCount(System.Drawing.Imaging.FrameDimension.Time))
                {
                    currentFrame = 0;
                }
                animatedImage.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Time, currentFrame);
                targetPictureBox.Invalidate();
            }
        }

        //DRAGGING//
        public class DragFunctionality
        {
            //Variables
            private bool isDragging = false;
            private Point dragCursorPoint;
            private Point dragFormPoint;
            public static class FormDrag
            {
                // Import the SendMessage and ReleaseCapture functions.
                [DllImport("user32.dll")]
                public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
                [DllImport("user32.dll")]
                public static extern bool ReleaseCapture();

                // Constants for the message parameters
                public const int WM_NCLBUTTONDOWN = 0xA1;
                public const int HTCAPTION = 0x2;
            }
            //Initializer
            public void InitializeDragging(MouseEventArgs e, Form form)
            {
                if (e.Button == MouseButtons.Left)
                {
                    FormDrag.ReleaseCapture();
                    FormDrag.SendMessage(form.Handle, FormDrag.WM_NCLBUTTONDOWN, FormDrag.HTCAPTION, 0);
                }
            }
            //AttachmentEvent
            public void AttachDraggingEvent(Control control, Form form)
            {
                control.MouseDown += (sender, e) => OnMouseDown(e, form);
                control.MouseMove += (sender, e) => OnMouseMove(form);
                control.MouseUp += (sender, e) => OnMouseUp(e);
            }
            // MouseDown to initiate dragging
            private void OnMouseDown(MouseEventArgs e, Form form)
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDragging = true;
                    dragCursorPoint = Cursor.Position;
                    dragFormPoint = form.Location;
                }
            }
            // MouseMove to handle dragging
            private void OnMouseMove(Form form)
            {
                if (isDragging)
                {
                    Point difference = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                    form.Location = Point.Add(dragFormPoint, new Size(difference));
                }
            }
            // MouseUp to stop dragging
            private void OnMouseUp(MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDragging = false;
                }
            }
        }
        public class Login
        {
            //LOGIN LOGIC//
            private readonly string correctUsername = "admin";
            private readonly string correctPassword = "admin";
            public bool LogIn(string username, string password)
            {
                if (username == correctUsername && password == correctPassword)
                {
                    MessageBox.Show("Logged In Successfully");
                    return true;
                }
                else
                {
                    MessageBox.Show("     Username or Password incorrect.\n\t    Please try again.");
                    return false;
                }
            }
        }
    }
}