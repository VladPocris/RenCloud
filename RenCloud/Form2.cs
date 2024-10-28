using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RenCloud.Program;

namespace RenCloud
{
    
    public partial class Form2 : Form
    {
        //Variables&Objects
        private bool isActive = false;
        private Corners applyCorners;
        private GifAnimation gifAnimation;
        private DragFunctionality dragFunctionality;
        private Timer loadingTimer;

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

        public Form2()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
            gifAnimation = new GifAnimation(pictureBox1);
            gifAnimation.InitializeGifAnimation(Properties.Resources._09242_ezgif_com_optimize);
            applyCorners = new Corners();
            dragFunctionality = new DragFunctionality();
            // Attach load event handler
            this.Load += Form2_Load;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            applyCorners.AttributesRoundCorners(this, isActive);
            dragFunctionality.AttachDraggingEvent(pictureBox1, this);
            LoadingTime_Logic(sender, e);
        }

        private void LoadingTimer_Tick(object sender, EventArgs e)
        {
            // Stop the timer
            loadingTimer.Stop();
            loadingTimer.Dispose();

            // Close the loading form and terminate the application
            Application.Exit();
        }

        private void LoadingTime_Logic(object sender, EventArgs e)
        {
            loadingTimer = new Timer();
            loadingTimer.Interval = 3000; // Set the loading duration (3000 ms = 3 seconds)
            loadingTimer.Tick += LoadingTimer_Tick;
            loadingTimer.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
