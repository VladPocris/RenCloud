using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.IO;
using static RenCloud.Program;

namespace RenCloud
{
    public partial class LogInForm : Form
    {

        //Variables&Objects
        private bool isActive = false;
        private bool usernameValid = false;
        private bool passwordValid = false;
        private Corners applyCorners;
        private GifAnimation gifAnimation;
        private DragFunctionality dragFunctionality;
        private Login loginProcess;
        private UsernameValidator usernameValidator;
        private PasswordValidator passwordValidator;
        private Placeholder placeholder;

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
                button2.PerformClick();
                e.SuppressKeyPress = true;
            }
        }
        private void tbpassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        //FORM//
        public LogInForm()
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
            usernameValidator = new UsernameValidator();
            passwordValidator = new PasswordValidator();
            placeholder = new Placeholder();
            //NECESSARY STARTUP SETTINGS//
            this.Shown += LogInForm_Shown;
            this.Load += LogInForm_Load;
            tbusername.Enter += tbusername_Enter;
            tbusername.Leave += tbusername_Leave;
            tbpassword.Enter += tbpassword_Enter;
            tbpassword.Leave += tbpassword_Leave;
        }  
        private void LogInForm_Load(object sender, EventArgs e)
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
                this.Hide();
                FormManager.LoadFormInstance.ShowDialog();
                FormManager.LogInFormInstance.Show();
            }
        }

        private void LogInForm_Shown(Object sender, EventArgs e)
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
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            FormManager.RegisterFormInstance.Show();
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
            if (passwordValidator.IsValidPassword(tbpassword.Text))
            {
                passwordValid = true;
            }
            else
            {
                passwordValid = false;
            }
            if (tbpassword.Text == "admin")
            {
                passwordValid = true;
            }
            if (usernameValid && passwordValid)
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }
        }

        private void tbusername_Enter(object sender, EventArgs e)
        {
            placeholder.PlaceholderOut(sender, e, tbusername, "Type a username.");
        }
        private void tbusername_Leave(object sender, EventArgs e)
        {
            placeholder.PlaceholderIn(sender, e, tbusername, "Type a username.");
        }
        private void tbpassword_Enter(object sender, EventArgs e)
        {
            placeholder.PlaceholderOut(sender, e, tbpassword, "Please type your password.");
            tbpassword.PasswordChar = '*';
        }
        private void tbpassword_Leave(object sender, EventArgs e)
        {
            placeholder.PlaceholderIn(sender, e, tbpassword, "Please type your password.");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void tbusername_TextChanged(object sender, EventArgs e)
        {
            if (usernameValidator.IsValidInput(tbusername.Text))
            {
                usernameValid = true;
            }
            else
            {
                usernameValid = false;
            }
            if (usernameValid && passwordValid)
            {
                button2.Enabled = true;
            } else
            {
                button2.Enabled = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            FormManager.PassRessFormInstance.Show();
        }

        private void LogInForm_Closing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            gifAnimation.StopAnimation();
            gifAnimation = null;
            dragFunctionality = null;
            applyCorners = null;
            loginProcess = null;
            usernameValidator = null;
            passwordValidator = null;
            placeholder = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
