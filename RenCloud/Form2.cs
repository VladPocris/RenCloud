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
            //START FROM CENTER//
            this.StartPosition = FormStartPosition.CenterScreen;
            //ENABLE DOUBLE BUFFER//
            this.DoubleBuffered = true;
            //GIF ANIMATION INIT//
            gifAnimation = new GifAnimation(pictureBox1);
            gifAnimation.InitializeGifAnimation(Properties.Resources._09242_ezgif_com_optimize);
            //APPLY ROUND CORNERS//
            applyCorners = new Corners();
            //APPLY DRAGGING FUNCTIONALITY//
            dragFunctionality = new DragFunctionality();
            //NECESSARY STARTUP SETTINGS//
            this.Load += Form2_Load;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //ATTACHMENTS AND ON-LOAD FEATURES//
            applyCorners.AttributesRoundCorners(this, isActive);
            dragFunctionality.AttachDraggingEvent(pictureBox1, this);
            LoadingTime_Logic(sender, e);
        }

        //TEMP LOADING FINISH//
        private void LoadingTimer_Tick(object sender, EventArgs e)
        {
            // Stop the timer
            loadingTimer.Stop();
            loadingTimer.Dispose();
            //Terminate the application.
            Application.Exit();
        }

        //TEMP LOADING FUNCTIONALITY//
        private void LoadingTime_Logic(object sender, EventArgs e)
        {
            //NEW TIMER CREATION//
            loadingTimer = new Timer();
            //LOADING DURATION (3000MS = 3S)//
            loadingTimer.Interval = 3000;
            //AT THE END OF DURATION//
            loadingTimer.Tick += LoadingTimer_Tick;
            //START THE TIMER//
            loadingTimer.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
