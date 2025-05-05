using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RenCloud.Program;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RenCloud
{
    [ExcludeFromCodeCoverage]
    public partial class RegisterForm : Form
    {

        //Variables&Objects
        private bool isActive = false;
        private PictureBox[] pictureBoxes;
        private GifAnimation gifAnimation;
        private DragFunctionality dragFunctionality;
        public const string valid = "Valid";
        public const string invalid = "Invalid";

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
        public RegisterForm()
        {
            InitializeComponent();
            //ENABLE DOUBLE BUFFER//
            this.DoubleBuffered = true;
            //GIF ANIMATION INIT//
            gifAnimation = new GifAnimation(pictureBox12);
            gifAnimation.InitializeGifAnimation(Properties.Resources.NetworkconnectionBackgroundHDDarkGeometricAbstractBackdrop_ezgif_com_speed);
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
            pictureBoxes = new PictureBox[] { pictureBox4, pictureBox3, pictureBox6, pictureBox8, pictureBox10 };
            this.FormClosing += RegisterForm_Closing;
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            //ATTACHMENTS AND ON-LOAD FEATURES//
            Program.Corners.AttributesRoundCorners(this, isActive);
            dragFunctionality.AttachDraggingEvent(panel1, this);
            dragFunctionality.AttachDraggingEvent(label1, this);
        }

        private void RegisterForm_Shown(Object sender, EventArgs e)
        {
            tbemail.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            FormManager.LogInFormInstance.Show();
        }

        private void tbemail_TextChanged(object sender, EventArgs e)
        {
            string email = tbemail.Text;

            if (Program.EmailValidator.IsValidEmail(email))
            {
                pictureBox3.Image = Properties.Resources.Check_Mark;
                pictureBox3.Tag = valid;
            }
            else
            {
                pictureBox3.Image = Properties.Resources.Cancel;
                pictureBox3.Tag = invalid;
            }
            if (tbemail.Text == tbemailcon.Text)
            {
                pictureBox4.Image = Properties.Resources.Check_Mark;
                pictureBox4.Tag = valid;
            }
            else
            {
                pictureBox4.Image = Properties.Resources.Cancel;
                pictureBox4.Tag = invalid;
            }
            button2.Enabled = Program.RegistrationValid.IsValidRegistration(pictureBoxes);
        }
        private void tbemailcon_TextChanged(object sender, EventArgs e)
        {
            if (tbemail.Text == tbemailcon.Text)
            {
                pictureBox4.Image = Properties.Resources.Check_Mark;
                pictureBox4.Tag = valid;
            }
            else
            {
                pictureBox4.Image = Properties.Resources.Cancel;
                pictureBox4.Tag = invalid;
            }
            button2.Enabled = Program.RegistrationValid.IsValidRegistration(pictureBoxes);
        }

        private const string emErrorVal = "Please type your email.";
        private const string emError2Val = "Please re-type your email.";
        private const string usernameVal = "Type a username.";
        private const string passwordVal = "Please type your password.";
        private const string password2Val = "Please re-type your password.";

        private void tbemail_Enter(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderOut(sender, e, tbemail, emErrorVal);
        }
        private void tbemail_Leave(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderIn(sender, e, tbemail, emErrorVal);
        }
        private void tbemailcon_Enter(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderOut(sender, e, tbemailcon, emError2Val);
        }
        private void tbemailcon_Leave(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderIn(sender, e, tbemailcon, emError2Val);
        }
        private void tbusername_Enter(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderOut(sender, e, tbusername, usernameVal);
        }
        private void tbusername_Leave(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderIn(sender, e, tbusername, usernameVal);
        }
        private void tbpassword_Enter(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderOut(sender, e, tbpassword, passwordVal);
            tbpassword.PasswordChar = '*';
        }
        private void tbpassword_Leave(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderIn(sender, e, tbpassword, passwordVal);
        }
        private void tbpasswordcon_Enter(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderOut(sender, e, tbpasswordcon, password2Val);
            tbpasswordcon.PasswordChar = '*';
        }
        private void tbpasswordcon_Leave(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderIn(sender, e, tbpasswordcon, password2Val);
        }

        private void tbusername_TextChanged(object sender, EventArgs e)
        {
            if (Program.UsernameValidator.IsValidUsername(tbusername.Text))
            {
                pictureBox6.Image = Properties.Resources.Check_Mark;
                pictureBox6.Tag = valid;
            }
            else
            {
                pictureBox6.Image = Properties.Resources.Cancel;
                pictureBox6.Tag = invalid;
            }
            button2.Enabled = Program.RegistrationValid.IsValidRegistration(pictureBoxes);
        }

        private void tbpassword_TextChanged(object sender, EventArgs e)
        {
            if (Program.PasswordValidator.IsValidPassword(tbpassword.Text))
            {
                pictureBox8.Image = Properties.Resources.Check_Mark;
                pictureBox8.Tag = valid;
            }
            else
            {
                pictureBox8.Image = Properties.Resources.Cancel;
                pictureBox8.Tag = invalid;
            }
            if (tbpassword.Text == tbpasswordcon.Text)
            {
                pictureBox10.Image = Properties.Resources.Check_Mark;
                pictureBox10.Tag = valid;
            }
            else
            {
                pictureBox10.Image = Properties.Resources.Cancel;
                pictureBox10.Tag = invalid;
            }
            button2.Enabled = Program.RegistrationValid.IsValidRegistration(pictureBoxes);
        }

        private void tbpasswordcon_TextChanged(object sender, EventArgs e)
        {
            if (tbpassword.Text == tbpasswordcon.Text)
            {
                pictureBox10.Image = Properties.Resources.Check_Mark;
                pictureBox10.Tag = valid;
            }
            else
            {
                pictureBox10.Image = Properties.Resources.Cancel;
                pictureBox10.Tag = invalid;
            }
            button2.Enabled = Program.RegistrationValid.IsValidRegistration(pictureBoxes);
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
            foreach (var picBox in pictureBoxes)
            {
                picBox.Dispose();
            }
            pictureBoxes = null;
            pictureBox12 = null;
        }
    }
}
