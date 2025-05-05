using RenCloud;
using System.Windows.Forms;

namespace FormManagerTests
{
    [TestClass]

    public class InstanceCreationAndReturningTests
    {
        PassRes_Form passResForm = null!;
        LoadForm loadForm = null!;
        RegisterForm registerForm = null!;
        LogInForm loginForm = null!;
        UserInterfaceForm userInterfaceForm = null!;

        [DoNotParallelize]
        [TestMethod]
        public void PassRessFormInstance_ReturnsSameInstance()
        {
            passResForm = FormManager.PassRessFormInstance;

            PassRes_Form newInstance = FormManager.PassRessFormInstance;

            Assert.AreSame(passResForm, newInstance, "Should return the same instance if not disposed.");

            passResForm.Dispose();
            newInstance.Dispose();
        }

        [DoNotParallelize]
        [TestMethod]
        public void LoadFormInstance_ReturnsSameInstance()
        {
            loadForm = FormManager.LoadFormInstance;

            LoadForm newInstance = FormManager.LoadFormInstance;

            Assert.AreSame(loadForm, newInstance, "Should return the same instance if not disposed.");

            loadForm.Dispose();
            newInstance.Dispose();
        }

        [DoNotParallelize]
        [TestMethod]
        public void RegisterFormInstance_ReturnsSameInstance()
        {
            registerForm = FormManager.RegisterFormInstance;

            RegisterForm newInstance = FormManager.RegisterFormInstance;

            Assert.AreSame(registerForm, newInstance, "Should return the same instance if not disposed.");

            registerForm.Dispose();
            newInstance.Dispose();
        }

        [DoNotParallelize]
        [TestMethod]
        public void LoginFormInstance_ReturnsSameInstance()
        {
            loginForm = FormManager.LogInFormInstance;

            LogInForm newInstance = FormManager.LogInFormInstance;

            Assert.AreSame(loginForm, newInstance, "Should return the same instance if not disposed.");

            loginForm.Dispose();
            newInstance.Dispose();
        }

        [TestMethod]
        public void userInterfaceFormInstance_ReturnsSameInstance()
        {
            FormManager.IsTest = true;
            userInterfaceForm = FormManager.UserInterfaceFormInstance;

            UserInterfaceForm newInstance = FormManager.UserInterfaceFormInstance;

            Assert.AreSame(userInterfaceForm, newInstance, "Should return the same instance if not disposed.");

            userInterfaceForm.Dispose();
            newInstance.Dispose();
        }
    }
}
