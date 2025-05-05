using System;
using System.Windows.Forms;
using static RenCloud.Program;
using System.Diagnostics.CodeAnalysis;

namespace RenCloud
{
    public partial class LogInForm : Form
    {
        //PropertiesTESTCASES
        public TextBox UsernameTextBox
        {
            get { return tbusername; }
        }

        public TextBox PasswordTextBox
        {
            get { return tbpassword; }
        }

        public Button LoginButton
        {
            get { return button2; }
        }

        public Button CloseButton
        {
            get { return button1; }
        }

        public bool LoginSuccess
        {
            get { return loginSuccess; }
        }

        public bool IsActive
        {
            get { return isActive; }
        }

        public bool KeyDownTriggered
        {
            get { return keyDownTriggered; }
        }

        public bool IsTest { get; set; }

        public bool PassBoxFocus
        {
            get { return passBoxFocus; }
        }

        public bool UserBoxFocus
        {
            get { return userBoxFocus; }
        }

        public bool ClosingForm { get { return closingForm; } }

        public bool LinkClicked { get; set; }

        public void SimulateActivation()
        {
            OnActivated(EventArgs.Empty);
        }

        public bool SimulateFormShow()
        {
            LogInForm_Shown(this, EventArgs.Empty);
            return UserBoxFocus;
        }

        public void SimulateDeactivation()
        {
            OnDeactivate(EventArgs.Empty);
        }

        public void SimulateLoad()
        {
            LogInForm_Load(this, EventArgs.Empty);
        }

        public void SimulateKeyDowns(KeyEventArgs key)
        {
            tbusername_KeyDown(tbusername, key);
            tbpassword_KeyDown(tbpassword, key);
        }

        public void SimuluateCloseButtonClick()
        {
            closingForm = true;
            button1_Click(this, EventArgs.Empty);
            Application.Exit();
        }

        public void SimulateInvalidLoginClick(string username, string password)
        {
            IsTest = true;

            tbusername.Text = username;
            tbpassword.Text = password;

            string user = tbusername.Text;
            string pass = tbpassword.Text;

            loginSuccess = loginProcess.LogIn(user, pass, IsTest);

            button2_Click(this, EventArgs.Empty);

            IsTest = false;

            if (!LoginSuccess)
            {
                tbpassword.Text = string.Empty;
                tbpassword.Focus();
                passBoxFocus = true;
                userBoxFocus = false;
            }
        }

        public void SimulateValidLoginClick(string username, string password)
        {
            IsTest = true;
            tbusername.Text = username;
            tbpassword.Text = password;
            button2_Click(this, EventArgs.Empty);
            IsTest = false;
        }

        public void SimulateButton3Click()
        {
            IsTest = true;
            button3_Click(this, EventArgs.Empty);
            IsTest = false;
        }
        public void SimuluateLink2LabelClick()
        {
            IsTest = true;
            linkLabel2_LinkClicked(this, new LinkLabelLinkClickedEventArgs(new LinkLabel.Link()));
            IsTest = false;
        }

        public void SimuluateLink1LabelClick()
        {
            IsTest = true;
            linkLabel1_LinkClicked(this, new LinkLabelLinkClickedEventArgs(new LinkLabel.Link()));
            IsTest = false;
        }

        public void SimulateTextBoxesChange()
        {
            tbusername_TextChanged(tbusername, EventArgs.Empty);
            tbpassword_TextChanged(tbpassword, EventArgs.Empty);
        }

        public void SimulatePlaceHolderInteractionEnterUsername()
        {
            tbusername_Enter(tbusername, EventArgs.Empty);
        }

        public void SimulatePlaceHolderInteractionLeaveUsername()
        {
            tbusername_Leave(tbusername, EventArgs.Empty);
        }

        public void SimulatePlaceHolderInteractionEnterPassword()
        {
            tbpassword_Enter(tbpassword, EventArgs.Empty);
        }

        public void SimulatePlaceHolderInteractionLeavePassword()
        {
            tbpassword_Leave(tbpassword, EventArgs.Empty);
        }

        //Variables&Objects
        private bool isActive = false;
        private bool keyDownTriggered = false;
        private bool usernameValid = false;
        private bool passwordValid = false;
        private bool loginSuccess = false;
        private bool passBoxFocus = false;
        private bool userBoxFocus = true;
        private bool closingForm = false;
        private GifAnimation gifAnimation;
        private DragFunctionality dragFunctionality;
        private Login loginProcess;

        //ROUNDCORNERS LOGIC//
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            isActive = false;
            Program.Corners.AttributesRoundCorners(this, isActive);
        }
        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            isActive = true;
            Program.Corners.AttributesRoundCorners(this, isActive);
        }

        //LOGIN FUNCTIONALITY//
        private void tbusername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
                e.SuppressKeyPress = true;
                keyDownTriggered = true;
            }
            else
            {
                keyDownTriggered = false;
            }
        }
        private void tbpassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
                e.SuppressKeyPress = true;
                keyDownTriggered = true;
            }
            else
            {
                keyDownTriggered = false;
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
            //APPLY DRAGGING FUNCTIONALITY//
            dragFunctionality = new DragFunctionality();
            //LOGIN PROCESS INIT//
            loginProcess = new Login();
            //FOR TESTS//
            AllowDrop = false;
            //NECESSARY STARTUP SETTINGS//
            this.Shown += LogInForm_Shown;
            this.Load += LogInForm_Load;
            Program.BufferHelper.SetDoubleBuffered(pictureBox3);
            Program.BufferHelper.SetDoubleBuffered(this);
            Program.BufferHelper.SetDoubleBuffered(panel1);
            tbusername.Enter += tbusername_Enter;
            tbusername.Leave += tbusername_Leave;
            tbpassword.Enter += tbpassword_Enter;
            tbpassword.Leave += tbpassword_Leave;
        }
        private void LogInForm_Load(object sender, EventArgs e)
        {
            //ATTACHMENTS AND ON-LOAD FEATURES//
            Program.Corners.AttributesRoundCorners(this, isActive);
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

            loginSuccess = loginProcess.LogIn(username, password, IsTest);

            if (!loginSuccess)
            {
                tbpassword.Clear();
                tbpassword.Focus();
            }
            else
            {
                ShowFormsOnSuccess();
            }
        }

        [ExcludeFromCodeCoverage]
        private void ShowFormsOnSuccess()
        {
            if (!IsTest)
            {
                this.Hide();
                FormManager.LoadFormInstance.ShowDialog();
                FormManager.UserInterfaceFormInstance.Show();
            }
        }

        private void LogInForm_Shown(Object sender, EventArgs e)
        {
            tbusername.Focus();
            userBoxFocus = true;
            passBoxFocus = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            closingForm = true;
            Application.Exit();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkClicked = true;
            ShowRegisterForm();
        }

        [ExcludeFromCodeCoverage]
        private void ShowRegisterForm()
        {
            if (IsTest) return;
            this.Hide();
            FormManager.RegisterFormInstance.Show();
        }

        private void tbpassword_TextChanged(object sender, EventArgs e)
        {
            if (Program.PasswordValidator.IsValidPassword(tbpassword.Text))
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

        private const string usernameNVal = "Type a username.";
        private const string passwordNVal = "Please type your password.";

        private void tbusername_Enter(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderOut(sender, e, tbusername, usernameNVal);
        }
        private void tbusername_Leave(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderIn(sender, e, tbusername, usernameNVal);
        }
        private void tbpassword_Enter(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderOut(sender, e, tbpassword, passwordNVal);
            tbpassword.PasswordChar = '*';
        }
        private void tbpassword_Leave(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderIn(sender, e, tbpassword, passwordNVal);
        }

        private void tbusername_TextChanged(object sender, EventArgs e)
        {
            if (Program.UsernameValidator.IsValidUsername(tbusername.Text) || tbusername.Text == "admin")
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
            }
            else
            {
                button2.Enabled = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkClicked = true;
            ShowRestorePassword();
        }

        [ExcludeFromCodeCoverage]
        public void ShowRestorePassword()
        {
            if (IsTest) return;
            this.Hide();
            FormManager.PassRessFormInstance.Show();
        }

        [ExcludeFromCodeCoverage]
        private void LogInForm_Closing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            gifAnimation.StopAnimation();
            gifAnimation = null;
            dragFunctionality = null;
            loginProcess = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            loginSuccess = false;
            ShowMainForm();
        }

        [ExcludeFromCodeCoverage]
        public void ShowMainForm()
        {
            if (IsTest) return;
            this.Hide();
            FormManager.UserInterfaceFormInstance.Show();
        }
    }
}
