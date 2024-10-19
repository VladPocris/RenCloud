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

        //ROUNDCORNERS//

        // Import dwmapi.dll and define DwmSetWindowAttribute.
        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        internal static extern void DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref int pvAttribute, uint cbAttribute);
        
        //Variables
        private bool isActive = false;

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
        public void AttributesRoundCorners()
        {
            Color borderColor = isActive ? Color.FromArgb(255, 153, 164) : Color.FromArgb(89, 76, 255);
            IntPtr hWnd = this.Handle;
            int cornerPreference = (int)DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
            DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref cornerPreference, sizeof(int));
            int colorValue = (borderColor.B << 16) | (borderColor.G << 8) | borderColor.R;
            DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_BORDER_COLOR, ref colorValue, sizeof(int));
        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            isActive = false;
            AttributesRoundCorners();
        }
        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            isActive = true;
            AttributesRoundCorners();
        }

        //SMOOTHGIFANIMATION//

        // Timer for GIF animation.
        private Timer frameTimer;
        private Bitmap animatedImage;
        private int currentFrame;
        //AttachmentEvent
        public void AttachGifEvent()
        {
            pictureBox3.Paint += new PaintEventHandler(pictureBox3_Paint);
        }
        // Initializaton for smooth gif animation.
        private void InitializeGifAnimation()
        {
            animatedImage = Properties.Resources.Background;
            currentFrame = 0;
            frameTimer = new Timer
            {
                Interval = 1
            };
            frameTimer.Tick += new EventHandler(OnFrameChanged);
            frameTimer.Start();
        }
        //Update the current frame of the animation.
        public void FrameUpdate(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (animatedImage != null)
            {
                e.Graphics.DrawImage(animatedImage, new Point(0, 0));
            }
        }
        //Update the frames for the animated image.
        public void AniFrameUpdate(PaintEventArgs e)
        {
            if (animatedImage != null)
            {
                ImageAnimator.UpdateFrames(animatedImage);
                e.Graphics.DrawImage(animatedImage, pictureBox3.ClientRectangle);
            }
        }
        //Tick frame update
        public void TickUpdate()
        {
            currentFrame++;
            if (currentFrame >= animatedImage.GetFrameCount(System.Drawing.Imaging.FrameDimension.Time))
            {
                currentFrame = 0;
            }
            animatedImage.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Time, currentFrame);
            pictureBox3.Invalidate();
        }

        //LOGIN//
        public void LogIn()
        {
            if (tbusername.Text == "admin" && tbpassword.Text == "admin")
            {
                MessageBox.Show("Logged In Successfully");
            }
            else
            {
                MessageBox.Show("     Username or Password incorrect.\n\t    Please try again.");
                tbusername.Text = "";
                tbpassword.Text = "";
                tbusername.Focus();
            }
        }

        //DRAGGING//

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
        public void InitializeDragging(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FormDrag.ReleaseCapture();
                FormDrag.SendMessage(this.Handle, FormDrag.WM_NCLBUTTONDOWN, FormDrag.HTCAPTION, 0);
            }
        }
        //AttachmentEvent
        public void AttachDraggingEvent()
        {
            panel1.MouseDown += new MouseEventHandler(panel1_MouseDown);
            label1.MouseDown += new MouseEventHandler(panel1_MouseDown);
            panel1.MouseMove += new MouseEventHandler(panel1_MouseMove);
            label1.MouseMove += new MouseEventHandler(panel1_MouseMove);
            panel1.MouseUp += new MouseEventHandler(panel1_MouseUp);
            label1.MouseUp += new MouseEventHandler(panel1_MouseUp);
        }

        public Form1()
        {
            InitializeComponent();
            InitializeGifAnimation();
            this.DoubleBuffered = true;
        }  

        private void Form1_Load(object sender, EventArgs e)
        {
            AttributesRoundCorners();
            // Attach the MouseDown event to the panel control.
            AttachDraggingEvent();
            // Attach the Smooth gif event to the panel control.
            AttachGifEvent();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
            }
        }
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            // Stop dragging
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            // Perform dragging
            if (isDragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }
        private void OnFrameChanged(object sender, EventArgs e)
        {
            TickUpdate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            FrameUpdate(e);
        }

        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            AniFrameUpdate(e);
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
            LogIn();
        }

        private void tbpassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
