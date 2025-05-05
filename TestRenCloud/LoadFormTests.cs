using RenCloud;
using System.Windows.Forms;

namespace LoadFormTests
{
    [TestClass]
    public class CornersValidation
    {

        private LoadForm loadForm = null!;

        [DoNotParallelize]
        [TestMethod]
        public void OnActivated_isActiveStatus()
        {
            loadForm = new LoadForm();
            loadForm.SimulateActivation();
            Assert.IsFalse(loadForm.IsActive, "Corners should be changed in another collor");
        }

        [DoNotParallelize]
        [TestMethod]
        public void OnDeactivated_isActiveStatus()
        {
            loadForm = new LoadForm();
            loadForm.SimulateDeactivation();
            Assert.IsTrue(loadForm.IsActive, "Corners should be changed in another collor");
        }
    }

    [TestClass]
    public class LoadFormLoadingAndInitialisations
    {
        private LoadForm loadForm = null!;

        [TestMethod]
        public void LoadForm_Load_InitializesCorrectly()
        {
            loadForm = new LoadForm();
            loadForm.SimulateLoad();
            Assert.IsFalse(loadForm.IsActive, "isActive not initialised properly or changed inaproppriately. (Not LoadForm Load Occured)");
        }

        [TestMethod]
        public void LoadForm_Closing_DisposeHappens()
        {
            loadForm = new LoadForm();
            loadForm.SimulateClosing();
            Assert.IsNull(loadForm.GifAnimation, "GifAnimation has not been disposed. (OnClose event did not occur)");
        }
    }

    [TestClass]
    public class LoadFormTimerTests
    {
        private LoadForm loadForm = null!;
        private bool CatchClose { get; set; }

        internal void FormClosedCatching(object sender, FormClosedEventArgs e)
        {
            CatchClose = true;
        }

        [TestMethod]
        public void LoadForm_TimerLogic_Initialised()
        {
            loadForm = new LoadForm();
            loadForm.SimulateTimerLoadInit();
            Assert.AreEqual(3000, loadForm.LoadingTimer.Interval, "Timer not initialised properly. (LoadingTime_Logic event did not occur)");
        }

        [DoNotParallelize]
        [TestMethod]
        public void LoadForm_TimerLogic_Stoping()
        {
            loadForm = new LoadForm();

            loadForm.FormClosed += FormClosedCatching;

            loadForm.SimulateTimerLoadInit();
            loadForm.SimulateTickStoping();

            Assert.IsNull(loadForm.LoadingTimer, "Timer did not dispose.");
        }
    }
}
