using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RenCloud.FormManager;
using static RenCloud.Program;

namespace RenCloud
{
    public partial class PassRes_Form : Form
    {
        //OBJECTS&VARIABLES
        private bool isActive = false;
        private GifAnimation gifAnimation;
        private DragFunctionality dragFunctionality;
        private PictureBox[] pictureBoxes;

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
        public PassRes_Form()
        {
            InitializeComponent();
            //GIF ANIMATION INIT//
            gifAnimation = new GifAnimation(pictureBox12);
            gifAnimation.InitializeGifAnimation(Properties.Resources.NetworkconnectionBackgroundHDDarkGeometricAbstractBackdrop_ezgif_com_speed);
            //APPLY DRAGGING FUNCTIONALITY//
            dragFunctionality = new DragFunctionality();
            //START FROM CENTER//
            this.StartPosition = FormStartPosition.CenterScreen;
            pictureBoxes = new PictureBox[] { pictureBox3, pictureBox6 };
            this.Load += PassResForm_Load;
            this.Shown += PassResForm_Shown;
            tbemail.Enter += tbemail_Enter;
            tbemail.Leave += tbemail_Leave;
            tbusername.Enter += tbusername_Enter;
            tbusername.Leave += tbusername_Leave;
            tbusername.KeyDown += tbusername_KeyDown;
            tbemail.KeyDown += tbemail_KeyDown;
            this.FormClosing += PassResForm_Closing;
        }

        private void PassResForm_Load(object sender, EventArgs e)
        {
            //ATTACHMENTS AND ON-LOAD FEATURES//
            Program.Corners.AttributesRoundCorners(this, isActive);
            dragFunctionality.AttachDraggingEvent(panel1, this);
            dragFunctionality.AttachDraggingEvent(label1, this);
        }

        private void PassResForm_Shown(Object sender, EventArgs e)
        {
            tbemail.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            FormManager.LogInFormInstance.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            FormManager.LoadFormInstance.ShowDialog();
            FormManager.LogInFormInstance.Show();
        }

        private void tbemail_TextChanged(object sender, EventArgs e)
        {
            string email = tbemail.Text;

            if (Program.EmailValidator.IsValidEmail(email))
            {
                pictureBox3.Image = Properties.Resources.Check_Mark;
                pictureBox3.Tag = "Valid";
            }
            else
            {
                pictureBox3.Image = Properties.Resources.Cancel;
                pictureBox3.Tag = "Invalid";
            }
            button2.Enabled = Program.RegistrationValid.IsValidRegistration(pictureBoxes);
        }

        private void tbusername_TextChanged(object sender, EventArgs e)
        {
            if (Program.UsernameValidator.IsValidUsername(tbusername.Text))
            {
                pictureBox6.Image = Properties.Resources.Check_Mark;
                pictureBox6.Tag = "Valid";
            }
            else
            {
                pictureBox6.Image = Properties.Resources.Cancel;
                pictureBox6.Tag = "Invalid";
            }
            button2.Enabled = Program.RegistrationValid.IsValidRegistration(pictureBoxes);
        }

        private void tbemail_Enter(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderOut(sender, e, tbemail, "Please type your email.");
        }
        private void tbemail_Leave(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderIn(sender, e, tbemail, "Please type your email.");
        }
        private void tbusername_Enter(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderOut(sender, e, tbusername, "Type a username.");
        }
        private void tbusername_Leave(object sender, EventArgs e)
        {
            Program.Placeholder.PlaceholderIn(sender, e, tbusername, "Type a username.");
        }

        private void tbusername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
                e.SuppressKeyPress = true;
            }
        }
        private void tbemail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private void PassResForm_Closing(object sender, FormClosingEventArgs e)
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
