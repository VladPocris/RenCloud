using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;
using static RenCloud.Program;

namespace RenCloud.Tests
{
    [TestClass]
    public class LogInFormTests
    {

        private LogInForm loginForm;
        private TextBox tbusername;
        private TextBox tbpassword;
        private Button loginButton;
        private string tempPassword;
        private string tempUsername;
        private string tempPasswordWrong;
        private string tempUsernameWrong;

        [TestInitialize]
        public void Setup()
        {
            loginForm = new LogInForm();
            tbpassword = loginForm.PasswordTextBox;
            tbusername = loginForm.UsernameTextBox;
            loginButton = loginForm.LoginButton;
            tempPassword = "testPass3!";
            tempUsername = "testPass";
            tempPasswordWrong = "test";
            tempUsernameWrong = "tes";
        }

        [TestMethod]
        public void LogInButton_Enabled_UsernameValid_And_PasswordValid_ReturnsTrue()
        {
            tbusername.Text = tempUsername;
            tbpassword.Text = tempPassword;
            Assert.IsTrue(loginButton.Enabled, "Login button should be enabled when both username and password are valid.");
        }

        [TestMethod]
        public void LogInButton_Enabled_UsernameAdmin_And_PasswordAdmin_ReturnsTrue()
        {
            tbusername.Text = "admin";
            tbpassword.Text = "admin";
            Assert.IsTrue(loginButton.Enabled, "Login button should be enabled when both username and password are valid.");
        }

        [TestMethod]
        public void LogInButton_Disabled_UsernameNotValid_And_PasswordValid_ReturnsFalse()
        {
            tbusername.Text = tempUsernameWrong;
            tbpassword.Text = tempPassword;
            Assert.IsFalse(loginButton.Enabled, "Login button should be disabled when username is not valid and password are valid.");
        }

        [TestMethod]
        public void LogInButton_Disabled_UsernameValid_And_PasswordNotValid_ReturnsFalse()
        {
            tbusername.Text = tempUsername;
            tbpassword.Text = tempPasswordWrong;
            Assert.IsFalse(loginButton.Enabled, "Login button should be disabled when username is valid and password invalid.");
        }

        [TestMethod]
        public void LogInButton_Disabled_UsernameNotValid_And_PasswordNotValid_ReturnsFalse()
        {
            tbusername.Text = tempUsernameWrong;
            tbpassword.Text = tempPasswordWrong;
            Assert.IsFalse(loginButton.Enabled, "Login button should be disabled when both username and password are invalid.");
        }
    }
}