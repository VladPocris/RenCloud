using RenCloud;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

//Make it visible to UnitTest without affecting it's accessibility.
[assembly: InternalsVisibleTo("TestRenCloud")]

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
            Application.Run(new LogInForm());
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
                if (frameTimer != null)
                {
                    frameTimer.Stop();
                    frameTimer.Dispose();
                }

                // Dispose of previous image if it exists
                if (animatedImage != null)
                {
                    animatedImage.Dispose();
                }

                // Set the gif and reset frame count
                animatedImage = customGif;
                currentFrame = 0;

                // Create a new frame timer and start the animation
                frameTimer = new Timer();
                frameTimer.Interval = 15;
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
            public void StopAnimation()
            {
                    frameTimer.Stop();
                    frameTimer.Tick -= OnFrameChanged; // Unsubscribe from the event handler
                    frameTimer.Dispose();
                    frameTimer = null;
                    animatedImage.Dispose();
                    animatedImage = null;
                    targetPictureBox.Paint -= FrameUpdate;
                    targetPictureBox.Image = null;
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

        //LOGIN//
        public class Login
        {
            //LOGIN LOGIC//
            private readonly string correctUsername = "admin";
            private readonly string correctPassword = "admin";
            public bool LogIn(string username, string password)
            {
                if (username == correctUsername && password == correctPassword)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("     Username or Password incorrect.\n\t    Please try again.");
                    return false;
                }
            }
        }

        //PLACEHOLDER//
        public class Placeholder
        {
            public void PlaceholderOut(object sender, EventArgs e, TextBox box, string placeholder)
            {
                // Check if the current text is the placeholder
                if (box.ForeColor == SystemColors.InactiveCaption)
                {
                    box.Text = "";
                    box.ForeColor = Color.Black;
                }
            }
            public void PlaceholderIn(object sender, EventArgs e, TextBox box, string placeholder)
            {
                // Check if the current text is the placeholder
                if (box.Text == string.Empty)
                {
                    box.Text = placeholder;
                    box.ForeColor = SystemColors.InactiveCaption;
                    box.PasswordChar = '\0';
                }
            }
        }
        //EMAILVALIDATOR//
        public class EmailValidator
        {
            public bool IsValidEmail(string email)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(email) || email.Contains(" "))
                    {
                        return false;
                    }
                    var mailAddress = new MailAddress(email);
                    var parts = email.Split('@');
                    if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[1]) || !parts[1].Contains("."))
                    {
                        return false;
                    }
                    var domainParts = parts[1].Split('.');
                    if (domainParts.Length < 2 ||
                        domainParts[domainParts.Length - 1].Length < 2 ||
                        domainParts[domainParts.Length - 1].Length > 5 ||
                        Regex.IsMatch(parts[1], @"\d") ||
                        !Regex.IsMatch(parts[1], @"^[a-zA-Z0-9.-]+$"))
                    {
                        return false;
                    }
                    string tld = domainParts[1];
                    if (string.IsNullOrWhiteSpace(tld) || !Regex.IsMatch(tld, @"^[a-zA-Z]+$"))
                    {
                        return false;
                    }
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }
        //USERNAMEVALIDATOR//
        public class UsernameValidator
        {
            public bool IsValidUsername(string input)
            {
                string pattern = "^[a-zA-Z0-9]*$";
                if(!Regex.IsMatch(input, pattern) || string.IsNullOrWhiteSpace(input) || input.Length < 4 || input.Length > 16)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        //PASSWORDVALIDATOR//
        public class PasswordValidator
        {
            public bool IsValidPassword(string password)
            {
                if (password.Length <= 7)
                    return false;
                if (!Regex.IsMatch(password, "[A-Z]"))
                    return false;
                if (!Regex.IsMatch(password, "[0-9]"))
                    return false;
                if (password.Length > 32)
                    return false;
                if (password.Contains(" "))
                    return false;
                if (!Regex.IsMatch(password, "[!@#$%^&*(),.?\":{}|<>]"))
                    return false;
                foreach (char c in password)
                {
                    if (c > 127)
                        return false;
                }
                return true;
            }
        }
        //REGISTRATIONVALID
        public class RegistrationValid
        {
            public bool IsValidRegistration(PictureBox[] picture)
            {
                foreach (PictureBox pictureBox in picture) 
                {
                    if (pictureBox.Tag == null || (string)pictureBox.Tag != "Valid")
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}