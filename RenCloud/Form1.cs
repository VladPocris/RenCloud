using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
namespace RenCloud
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None; // Removes the border
            this.BackColor = Color.FromArgb(67, 38, 88); // Set background color
            this.Paint += Form1_Paint;
            panel1.Paint += Panel1_Paint;
            pictureBox3.Paint += PictureBox3_Paint;
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            RoundAllCorners(this, 45);
        }
        private void PictureBox3_Paint(object sender, PaintEventArgs e)
        {
            RoundAllCorners(this.pictureBox3, 45);
        }
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            RoundTopCorners(panel1, 45);
        }
        private void Form1_Load(object sender, EventArgs e)
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
        private void RoundTopCorners(Control control, int cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();

            // Ensure the control's width and height are positive
            if (control.Width > 0 && control.Height > 0)
            {
                path.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90); // Top left corner
                path.AddArc(control.Width - cornerRadius, 0, cornerRadius, cornerRadius, 270, 90); // Top right corner
                path.AddLine(control.Width, cornerRadius, control.Width, control.Height); // Right vertical line
                path.AddLine(control.Width, control.Height, 0, control.Height); // Bottom horizontal line
                path.AddLine(0, control.Height, 0, cornerRadius); // Left vertical line

                path.CloseFigure();

                control.Region = new Region(path);
            }
        }
        private void RoundBottomCorners(Control control, int cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();

            // Ensure the control's width and height are positive
            if (control.Width > 0 && control.Height > 0)
            {
                path.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90); // Top left corner
                path.AddArc(control.Width - cornerRadius, 0, cornerRadius, cornerRadius, 270, 90); // Top right corner
                path.AddLine(control.Width, cornerRadius, control.Width, control.Height); // Right vertical line
                path.AddLine(control.Width, control.Height, 0, control.Height); // Bottom horizontal line
                path.AddLine(0, control.Height, 0, cornerRadius); // Left vertical line

                path.CloseFigure();

                control.Region = new Region(path);
            }
        }
        private void RoundAllCorners(Control control, int cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();

            // Ensure the control's width and height are positive
            if (control.Width > 0 && control.Height > 0)
            {
                path.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90);
                path.AddArc(control.Width - cornerRadius, 0, cornerRadius, cornerRadius, 270, 90);
                path.AddArc(control.Width - cornerRadius, control.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
                path.AddArc(0, control.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
                path.CloseFigure();
                control.Region = new Region(path);
            }
        }
    }
}
