using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RenCloud.Program;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RenCloud
{
    public partial class RegisterForm : Form
    {

        //Variables&Objects
        private bool isActive = false;
        private PictureBox[] pictureBoxes;
        private Corners applyCorners;
        private GifAnimation gifAnimation;
        private DragFunctionality dragFunctionality;
        private Placeholder placeholder;
        private EmailValidator emailValidator;
        private UsernameValidator usernameValidator;
        private PasswordValidator passwordValidator;
        private RegistrationValid registrationValid;

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
        public RegisterForm()
        {
            InitializeComponent();
            //ENABLE DOUBLE BUFFER//
            this.DoubleBuffered = true;
            //GIF ANIMATION INIT//
            gifAnimation = new GifAnimation(pictureBox12);
            gifAnimation.InitializeGifAnimation(Properties.Resources.NetworkconnectionBackgroundHDDarkGeometricAbstractBackdrop_ezgif_com_speed);
            //APPLY ROUND CORNERS//
            applyCorners = new Corners();
            //APPLY DRAGGING FUNCTIONALITY//
            dragFunctionality = new DragFunctionality();
            //START FROM CENTER//
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Shown += RegisterForm_Shown;
            this.Load += RegisterForm_Load;
            tbemail.Enter += tbemail_Enter;
            tbemail.Leave += tbemail_Leave;
            tbemailcon.Enter += tbemailcon_Enter;
            tbemailcon.Leave += tbemailcon_Leave;
            tbusername.Enter += tbusername_Enter;
            tbusername.Leave += tbusername_Leave;
            tbpassword.Enter += tbpassword_Enter;
            tbpassword.Leave += tbpassword_Leave;
            tbpasswordcon.Enter += tbpasswordcon_Enter;
            tbpasswordcon.Leave += tbpasswordcon_Leave;
            tbemail.KeyDown += tbemail_KeyDown;
            tbemailcon.KeyDown += tbemailcon_KeyDown;
            tbusername.KeyDown += tbusername_KeyDown;
            tbpassword.KeyDown += tbpassword_KeyDown;
            tbpasswordcon.KeyDown += tbpasswordcon_KeyDown;
            //REGISTER OBJECT//
            placeholder = new Placeholder();
            emailValidator = new EmailValidator();
            usernameValidator = new UsernameValidator();
            passwordValidator = new PasswordValidator();
            registrationValid = new RegistrationValid();
            pictureBoxes = new PictureBox[] { pictureBox4, pictureBox3, pictureBox6, pictureBox8, pictureBox10 };
            this.FormClosing += RegisterForm_Closing;
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            //ATTACHMENTS AND ON-LOAD FEATURES//
            applyCorners.AttributesRoundCorners(this, isActive);
            dragFunctionality.AttachDraggingEvent(panel1, this);
            dragFunctionality.AttachDraggingEvent(label1, this);
        }

        private void RegisterForm_Shown(Object sender, EventArgs e)
        {
            tbemail.Focus();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            FormManager.LogInFormInstance.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void tbemail_TextChanged(object sender, EventArgs e)
        {
            string email = tbemail.Text;

            if (emailValidator.IsValidEmail(email))
            {
                pictureBox3.Image = Properties.Resources.Check_Mark;
                pictureBox3.Tag = "Valid";
            }
            else
            {
                pictureBox3.Image = Properties.Resources.Cancel;
                pictureBox3.Tag = "Invalid";
            }
            if (tbemail.Text == tbemailcon.Text)
            {
                pictureBox4.Image = Properties.Resources.Check_Mark;
                pictureBox4.Tag = "Valid";
            }
            else
            {
                pictureBox4.Image = Properties.Resources.Cancel;
                pictureBox4.Tag = "Invalid";
            }
            button2.Enabled = registrationValid.IsValidRegistration(pictureBoxes);
        }
        private void tbemailcon_TextChanged(object sender, EventArgs e)
        {
            if (tbemail.Text == tbemailcon.Text)
            {
                pictureBox4.Image = Properties.Resources.Check_Mark;
                pictureBox4.Tag = "Valid";
            }
            else
            {
                pictureBox4.Image = Properties.Resources.Cancel;
                pictureBox4.Tag = "Invalid";
            }
            button2.Enabled = registrationValid.IsValidRegistration(pictureBoxes);
        }

        private void tbemail_Enter(object sender, EventArgs e)
        {
            placeholder.PlaceholderOut(sender, e, tbemail, "Please type your email.");
        }
        private void tbemail_Leave(object sender, EventArgs e)
        {
            placeholder.PlaceholderIn(sender, e, tbemail, "Please type your email.");
        }
        private void tbemailcon_Enter(object sender, EventArgs e)
        {
            placeholder.PlaceholderOut(sender, e, tbemailcon, "Please re-type email.");
        }
        private void tbemailcon_Leave(object sender, EventArgs e)
        {
            placeholder.PlaceholderIn(sender, e, tbemailcon, "Please re-type email.");
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
        private void tbpasswordcon_Enter(object sender, EventArgs e)
        {
            placeholder.PlaceholderOut(sender, e, tbpasswordcon, "Please re-type password.");
            tbpasswordcon.PasswordChar = '*';
        }
        private void tbpasswordcon_Leave(object sender, EventArgs e)
        {
            placeholder.PlaceholderIn(sender, e, tbpasswordcon, "Please re-type password.");
        }

        private void tbusername_TextChanged(object sender, EventArgs e)
        {
            if (usernameValidator.IsValidUsername(tbusername.Text))
            {
                pictureBox6.Image = Properties.Resources.Check_Mark;
                pictureBox6.Tag = "Valid";
            }
            else
            {
                pictureBox6.Image = Properties.Resources.Cancel;
                pictureBox6.Tag = "Invalid";
            }
            button2.Enabled = registrationValid.IsValidRegistration(pictureBoxes);
        }

        private void tbpassword_TextChanged(object sender, EventArgs e)
        {
            if (passwordValidator.IsValidPassword(tbpassword.Text))
            {
                pictureBox8.Image = Properties.Resources.Check_Mark;
                pictureBox8.Tag = "Valid";
            }
            else
            {
                pictureBox8.Image = Properties.Resources.Cancel;
                pictureBox8.Tag = "Invalid";
            }
            if (tbpassword.Text == tbpasswordcon.Text)
            {
                pictureBox10.Image = Properties.Resources.Check_Mark;
                pictureBox10.Tag = "Valid";
            }
            else
            {
                pictureBox10.Image = Properties.Resources.Cancel;
                pictureBox10.Tag = "Invalid";
            }
            button2.Enabled = registrationValid.IsValidRegistration(pictureBoxes);
        }

        private void tbpasswordcon_TextChanged(object sender, EventArgs e)
        {
            if (tbpassword.Text == tbpasswordcon.Text)
            {
                pictureBox10.Image = Properties.Resources.Check_Mark;
                pictureBox10.Tag = "Valid";
            }
            else
            {
                pictureBox10.Image = Properties.Resources.Cancel;
                pictureBox10.Tag = "Invalid";
            }
            button2.Enabled = registrationValid.IsValidRegistration(pictureBoxes);
        }

        private void tbemail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private void tbemailcon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
                e.SuppressKeyPress = true;
            }
        }
        private void tbpasswordcon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            FormManager.LoadFormInstance.ShowDialog();
            FormManager.LogInFormInstance.Show();
        }
        private void RegisterForm_Closing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            gifAnimation.StopAnimation();
            gifAnimation = null;
            dragFunctionality = null;
            applyCorners = null;
            placeholder = null;
            emailValidator = null;
            usernameValidator = null;
            passwordValidator = null;
            registrationValid = null;
            foreach (var picBox in pictureBoxes)
            {
                picBox.Dispose();
            }
            pictureBoxes = null;
            pictureBox12 = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
