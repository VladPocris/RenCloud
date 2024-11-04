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
        private Corners applyCorners;
        private GifAnimation gifAnimation;
        private DragFunctionality dragFunctionality;
        private Placeholder placeholder;
        private Email emailValid;

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
            //REGISTER OBJECT//
            placeholder = new Placeholder();
            emailValid = new Email();
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

            if (emailValid.IsValidEmail(email))
            {
                pictureBox3.Image = Properties.Resources.Check_Mark;
            }
            else
            {
                pictureBox3.Image = Properties.Resources.Cancel;
            }
        }
        private void tbemailcon_TextChanged(object sender, EventArgs e)
        {
            if (tbemail.Text == tbemailcon.Text)
            {
                pictureBox4.Image = Properties.Resources.Check_Mark;
            }
            else
            {
                pictureBox4.Image = Properties.Resources.Cancel;
            }
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
        }
        private void tbpassword_Leave(object sender, EventArgs e)
        {
            placeholder.PlaceholderIn(sender, e, tbpassword, "Please type your password.");
        }
        private void tbpasswordcon_Enter(object sender, EventArgs e)
        {
            placeholder.PlaceholderOut(sender, e, tbpasswordcon, "Please re-type password.");
        }
        private void tbpasswordcon_Leave(object sender, EventArgs e)
        {
            placeholder.PlaceholderIn(sender, e, tbpasswordcon, "Please re-type password.");
        }
    }
}
