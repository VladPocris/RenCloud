using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

//Make it visible to UnitTest without affecting it's accessibility.
[assembly: InternalsVisibleTo("TestRenCloud")]

namespace RenCloud
{ 
    internal class Program
    {
        /// <summary>
        /// Test settings.
        /// </summary>
        internal static bool IsTesting {  get; set; }
        internal static Color BordersColor { get; private set; }
        internal static bool TickUpdateCalled { get; private set; }

        /// <summary>
        /// The main entry point for the application. Not meant to be tested.
        /// </summary>
        [ExcludeFromCodeCoverage]
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
            internal static extern void DwmSetWindowAttribute(IntPtr hwnd, DwmWindowAttribute attribute, ref int pvAttribute, uint cbAttribute);

            // The DWM_WINDOW_CORNER_PREFERENCE enum for DwmSetWindowAttribute's third parameter.
            public enum DwmWindowCornerPreference
            {
                DwmwCpDefault = 0,
                DwmwCpDonotround = 1,
                DwmwCpRound = 2,
                DwmwCpRoundSmall = 3
            }

            // The DWMWINDOWATTRIBUTE enum parameters to be set
            public enum DwmWindowAttribute
            {
                DwmwaWindowCornerPreference = 33,
                DwmwaBorderColor = 34
            }

            // Pass attributes for DWMWINDOWATTRIBUTE.
            public static void AttributesRoundCorners(Form form, bool isActive)
            {
                if (!Environment.UserInteractive || !form.IsHandleCreated || form.Handle == IntPtr.Zero)
                    return;

                BordersColor = isActive
                    ? Color.FromArgb(255, 153, 164)
                    : Color.FromArgb(89, 76, 255);

                try
                {
                    AtrributesSetting(BordersColor, form);
                }
                catch
                {
                    //Just ignore the exception/not all platforms can handle it.
                }
            }

            [ExcludeFromCodeCoverage]
            private static void AtrributesSetting(Color borderColor, Form form)
            {
                if (IsTesting) return;

                IntPtr hWnd = form.Handle;

                int cornerPreference = (int)DwmWindowCornerPreference.DwmwCpRound;
                try
                {
                    DwmSetWindowAttribute(
                        hWnd,
                        DwmWindowAttribute.DwmwaWindowCornerPreference,
                        ref cornerPreference,
                        sizeof(int));
                }
                catch
                {
                    //Just ignore the exception/not all platforms can handle it.
                }

                int colorValue = (borderColor.B << 16) | (borderColor.G << 8) | borderColor.R;
                try
                {
                    DwmSetWindowAttribute(
                        hWnd,
                        DwmWindowAttribute.DwmwaBorderColor,
                        ref colorValue,
                        sizeof(int));
                }
                catch
                {
                    //Just ignore the exception/not all platforms can handle it.
                }
            }

        }

        //SMOOTHGIFANIMATION//
        public class GifAnimation
        {
            private Timer frameTimer;
            private Bitmap animatedImage;
            internal int currentFrame;
            private readonly PictureBox targetPictureBox;
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
            internal void OnFrameChanged(object sender, EventArgs e)
            {
                TickUpdateCalled = true;
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

            [ExcludeFromCodeCoverage]
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
            internal bool IsDragging { get; set; }
            internal Point DragCursorPoint { get; set; }
            internal Point DragFormPoint { get; set; }
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
            public static void InitializeDragging(MouseEventArgs e, Form form)
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
            internal void OnMouseDown(MouseEventArgs e, Form form)
            {
                if (e.Button == MouseButtons.Left)
                {
                    IsDragging = true;
                    DragCursorPoint = Cursor.Position;
                    DragFormPoint = form.Location;
                }
            }
            // MouseMove to handle dragging
            internal void OnMouseMove(Form form)
            {
                if (IsDragging)
                {
                    Point difference = Point.Subtract(Cursor.Position, new Size(DragCursorPoint));
                    form.Location = Point.Add(DragFormPoint, new Size(difference));
                }
            }
            // MouseUp to stop dragging
            internal void OnMouseUp(MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    IsDragging = false;
                }
            }
        }

        //LOGIN//
        public class Login
        {
            //LOGIN LOGIC//
            private readonly string correctUsername = ConfigurationManager.AppSettings["TestUsername"];
            private readonly string correctPassword = ConfigurationManager.AppSettings["TestPassword"];
            public bool LogIn(string username, string password, bool isTest)
            {
                if (username == correctUsername && BCrypt.Net.BCrypt.Verify(password, correctPassword))
                {
                    return true;
                }
                else
                {
                    ShowMessageBox(isTest);
                    return false;
                }
            }

            [ExcludeFromCodeCoverage]
            private void ShowMessageBox(bool isTest)
            {
                if (!isTest)
                {
                    MessageBox.Show("     Username or Password incorrect.\n\t    Please try again.");
                }
            }
        }

        //PLACEHOLDER//
        public static class Placeholder
        {
            public static void PlaceholderOut(object sender, EventArgs e, TextBox box, string placeholder)
            {
                // Check if the current text is the placeholder
                if (box.ForeColor == SystemColors.InactiveCaption)
                {
                    box.Text = "";
                    box.ForeColor = Color.Black;
                }
            }
            public static void PlaceholderIn(object sender, EventArgs e, TextBox box, string placeholder)
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
        [ExcludeFromCodeCoverage] //Its teste but cant reproduce RegexTimeout -> Engine too optimised
        public static class EmailValidator
        {
            //Sonar quber recomendation best DDOS security practice
            private static readonly Regex EmailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(100));
            public static bool IsValidEmail(string email)
            {
                if (string.IsNullOrWhiteSpace(email) || email.Contains(" "))
                {
                    return false;
                }

                try
                {
                    if (!EmailRegex.IsMatch(email))
                    {
                        return false;
                    }

                    var parts = email.Split('@');
                    string domain = parts[1];

                    if (domain.Contains(".."))
                    {
                        return false;
                    }

                    string[] domainParts = domain.Split('.');
                    string domainWithoutTLD = string.Join(".", domainParts, 0, domainParts.Length - 1);
                    string tld = domainParts[domainParts.Length - 1];

                    if (Regex.IsMatch(domainWithoutTLD, @"^\d+$", RegexOptions.None, TimeSpan.FromMilliseconds(100)))
                    {
                        return false;
                    }

                    if (tld.Length > 6)
                    {
                        return false;
                    }

                    return true;
                }
                catch (RegexMatchTimeoutException)
                {
                    return false;
                }
            }
        }
        //USERNAMEVALIDATOR//
        public static class UsernameValidator
        {
            public static bool IsValidUsername(string input)
            {
                string pattern = "^[a-zA-Z0-9]*$";
                if(!Regex.IsMatch(input, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(100)) || string.IsNullOrWhiteSpace(input) || input.Length < 4 || input.Length > 16)
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
        public static class PasswordValidator
        {
            public static bool IsValidPassword(string password)
            {
                if (password.Length <= 7)
                    return false;
                if (!Regex.IsMatch(password, "[A-Z]", RegexOptions.None, TimeSpan.FromMilliseconds(100)))
                    return false;
                if (!Regex.IsMatch(password, "[0-9]", RegexOptions.None, TimeSpan.FromMilliseconds(100)))
                    return false;
                if (password.Length > 32)
                    return false;
                if (password.Contains(" "))
                    return false;
                if (!Regex.IsMatch(password, "[!@#$%^&*(),.?\":{}|<>]", RegexOptions.None, TimeSpan.FromMilliseconds(100)))
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
        public static class RegistrationValid
        {
            public static bool IsValidRegistration(PictureBox[] picture)
            {
                return picture.All(pictureBox => pictureBox.Tag != null && (string)pictureBox.Tag == "Valid");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S3011", Justification = "Setting DoubleBuffered for performance improvement, will not impact security whatsoever.")]
        public static class BufferHelper
        {
            [ExcludeFromCodeCoverage]
            public static void SetDoubleBuffered(Control control)
            {
                if (BufferTerminalSession())
                    return;

                PropertyInfo propertyInfo = typeof(Control).GetProperty("DoubleBuffered",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                propertyInfo?.SetValue(control, true, null);
            }

            [ExcludeFromCodeCoverage]
            public static bool BufferTerminalSession()
            {
                return SystemInformation.TerminalServerSession;
            }
        }
    }
}