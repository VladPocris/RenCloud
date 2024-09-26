using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.IO;
namespace RenCloud
{
    public partial class Form1 : Form
    {

        // Import dwmapi.dll and define DwmSetWindowAttribute.
        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        internal static extern void DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref int pvAttribute,uint cbAttribute);

        // Timer for GIF animation.
        private Timer frameTimer;
        private Bitmap animatedImage;
        private int currentFrame;
        public Form1()
        {
            InitializeComponent();
            InitializeGifAnimation();
        }  

        private void Form1_Load(object sender, EventArgs e)
        {
            // Pass attributes for DWMWINDOWATTRIBUTE.
            IntPtr hWnd = this.Handle;
            int cornerPreference = (int)DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
            DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref cornerPreference, sizeof(int));
            Color borderColor = Color.FromArgb(255, 153, 164);
            int colorValue = (borderColor.B << 16) | (borderColor.G << 8) | borderColor.R;
            DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_BORDER_COLOR, ref colorValue, sizeof(int));
            // Attach the MouseDown event to the panel control.
            panel1.MouseDown += new MouseEventHandler(panel1_MouseDown);
            // Attach the Smooth gif event to the panel control.
            pictureBox3.Paint += new PaintEventHandler(pictureBox3_Paint);
        }

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

        // Implementing the drag functionality.
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

        // Handle the MouseDown event to initiate dragging.
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FormDrag.ReleaseCapture();
                FormDrag.SendMessage(this.Handle, FormDrag.WM_NCLBUTTONDOWN, FormDrag.HTCAPTION, 0);
            }
        }

        // Initializaton for smooth gif animation.
        private void InitializeGifAnimation()
        {
            animatedImage = animatedImage = Properties.Resources.Background;
            currentFrame = 0; 
            frameTimer = new Timer();
            frameTimer.Interval = 1;
            frameTimer.Tick += new EventHandler(OnFrameChanged);
            frameTimer.Start();
        }

        // This method is called on every timer tick to update the frame.
        private void OnFrameChanged(object sender, EventArgs e)
        {
            currentFrame++;
            if (currentFrame >= animatedImage.GetFrameCount(System.Drawing.Imaging.FrameDimension.Time))
            {
                currentFrame = 0;
            }
            animatedImage.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Time, currentFrame);
            pictureBox1.Invalidate();
        }

        // Draw the current frame of the animation.
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (animatedImage != null)
            {
                e.Graphics.DrawImage(animatedImage, new Point(0, 0));
            }
        }

        // Update the frames for the animated image.
        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            if (animatedImage != null)
            {
                ImageAnimator.UpdateFrames(animatedImage);
                e.Graphics.DrawImage(animatedImage, pictureBox3.ClientRectangle);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tbusername.Text == "admin" && tbpassword.Text == "admin") {
                MessageBox.Show("Logged In Successfully");
            } else
            {
                MessageBox.Show("Username or Password incorrect.\n\tPlease try again.");
                tbusername.Text = "";
                tbpassword.Text = "";
                tbusername.Focus();
            }
        }

        private void tbpassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
