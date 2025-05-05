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
    
    public partial class LoadForm : Form
    {
        //Exposing securely for testing
        public void SimulateActivation()
        {
            OnActivated(EventArgs.Empty);
        }

        public void SimulateDeactivation()
        {
            OnDeactivate(EventArgs.Empty);
        }

        public void SimulateLoad()
        {
            Form2_Load(this, EventArgs.Empty);
        }

        internal void SimulateClosing()
        {
            LoadForm_Closing(this, new FormClosingEventArgs(CloseReason.UserClosing, false));
        }

        internal void SimulateTimerLoadInit()
        {
            LoadingTime_Logic(this, EventArgs.Empty);
        }

        internal void SimulateTickStoping()
        {
            LoadingTimer_Tick(this, EventArgs.Empty);
        }

        internal bool IsActive {  get; set; }

        internal GifAnimation GifAnimation { get; set; }

        internal Timer LoadingTimer { get; set; }

        //Variables&Objects
        private DragFunctionality dragFunctionality;

        //ROUNDCORNERS LOGIC//
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            IsActive = false;
            Program.Corners.AttributesRoundCorners(this, IsActive);
        }
        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            IsActive = true;
            Program.Corners.AttributesRoundCorners(this, IsActive);
        }

        public LoadForm()
        {
            InitializeComponent();
            //START FROM CENTER//
            this.StartPosition = FormStartPosition.CenterScreen;
            //ENABLE DOUBLE BUFFER//
            this.DoubleBuffered = true;
            //GIF ANIMATION INIT//
            GifAnimation = new GifAnimation(pictureBox1);
            GifAnimation.InitializeGifAnimation(Properties.Resources._09242_ezgif_com_optimize);
            //APPLY DRAGGING FUNCTIONALITY//
            dragFunctionality = new DragFunctionality();
            //NECESSARY STARTUP SETTINGS//
            this.Load += Form2_Load;
            this.FormClosing += LoadForm_Closing;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //ATTACHMENTS AND ON-LOAD FEATURES//
            IsActive = false;
            Program.Corners.AttributesRoundCorners(this, IsActive);
            dragFunctionality.AttachDraggingEvent(pictureBox1, this);
            LoadingTime_Logic(sender, e);
        }

        //TEMP LOADING FINISH//
        private void LoadingTimer_Tick(object sender, EventArgs e)
        {
            // Stop the timer
            LoadingTimer.Stop();
            LoadingTimer.Dispose();
            LoadingTimer = null;
            //Terminate the form.
            this.Close();
        }

        //TEMP LOADING FUNCTIONALITY//
        private void LoadingTime_Logic(object sender, EventArgs e)
        {
            //NEW TIMER CREATION//
            LoadingTimer = new Timer();
            //LOADING DURATION (3000MS = 3S)//
            LoadingTimer.Interval = 3000;
            //AT THE END OF DURATION//
            LoadingTimer.Tick += LoadingTimer_Tick;
            //START THE TIMER//
            LoadingTimer.Start();
        }

        private void LoadForm_Closing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            GifAnimation.StopAnimation();
            GifAnimation = null;
            dragFunctionality = null;
            pictureBox1.Dispose();
        }
    }
}
