using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.IO;
using static RenCloud.Program;
namespace RenCloud
{
    public partial class Form1 : Form
    {

        //Variables&Objects
        private bool isActive = false;
        private Form loadForm;
        private Corners applyCorners;
        private GifAnimation gifAnimation;
        private DragFunctionality dragFunctionality;
        private Login loginProcess;

        //ROUNDCORNERS LOGIC//
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            isActive = false;
            applyCorners.AttributesRoundCorners(this, isActive);
        }
        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            isActive = true;
            applyCorners.AttributesRoundCorners(this, isActive);
        }

        //LOGIN FUNCTIONALITY//
        private void tbusername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Trigger the login button click event
                button2.PerformClick();
                e.SuppressKeyPress = true; // Optional: suppress the key press so it doesn't add a newline
            }
        }
        private void tbpassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Trigger the login button click event
                button2.PerformClick();
                e.SuppressKeyPress = true; // Optional: suppress the key press so it doesn't add a newline
            }
        }

        //FORM//
        public Form1()
        {
            InitializeComponent();
            //ENABLE DOUBLE BUFFER//
            this.DoubleBuffered = true;
            //GIF ANIMATION INIT//
            gifAnimation = new GifAnimation(pictureBox3);
            gifAnimation.InitializeGifAnimation(Properties.Resources.NetworkconnectionBackgroundHDDarkGeometricAbstractBackdrop_ezgif_com_speed);
            //APPLY ROUND CORNERS//
            applyCorners = new Corners();
            //APPLY DRAGGING FUNCTIONALITY//
            dragFunctionality = new DragFunctionality();
            //LOGIN PROCESS INIT//
            loginProcess = new Login();
            //NECESSARY STARTUP SETTINGS//
            this.Shown += Form1_Shown;
        }  
        private void Form1_Load(object sender, EventArgs e)
        {
            //ATTACHMENTS AND ON-LOAD FEATURES//
            applyCorners.AttributesRoundCorners(this, isActive);
            dragFunctionality.AttachDraggingEvent(panel1, this);
            dragFunctionality.AttachDraggingEvent(label1, this);
            tbusername.KeyDown += tbusername_KeyDown;
            tbpassword.KeyDown += tbpassword_KeyDown;
        }

        //LOGIN BUTTON//
        private void button2_Click(object sender, EventArgs e)
        {
            string username = tbusername.Text;
            string password = tbpassword.Text;

            bool loginSuccess = loginProcess.LogIn(username, password);

            if (!loginSuccess)
            {
                tbusername.Clear();
                tbpassword.Clear();
                tbusername.Focus();
            }
            else
            {
                loadForm = new Form2();
                loadForm.Show();
                this.Hide();
            }
        }
        private void Form1_Shown(Object sender, EventArgs e)
        {
            tbusername.Focus();
        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
        }

        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {   
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

        private void tbpassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }
    }
}
