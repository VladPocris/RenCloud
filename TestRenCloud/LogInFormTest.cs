using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RenCloud;

namespace TestRenCloud
{
    [TestClass]
    public class LogInFormTest
    {
        private Program.UsernameValidator usernameValidator;
        private LogInForm logInForm;

        [TestInitialize]
        public void Setup()
        {
            logInForm = new LogInForm();
            var usernameTextBox = logInForm.UsernameTextBox;
            usernameValidator = new Program.UsernameValidator();
        }

        //UsernameValidation START
        [TestMethod]
        public void LogInFormUsernameValidationTest_ValidUsername()
        {
            string validUsername = "ValidUser123";
            bool isValid = usernameValidator.IsValidInput(validUsername);
            Assert.IsTrue(isValid, "Expected the username to be valid.");
        }

        [TestMethod]
        public void LogInFormUsernameValidationTest_InvalidUsername_Empty()
        {
            string invalidUsername = "";
            bool isValid = usernameValidator.IsValidInput(invalidUsername);
            Assert.IsFalse(isValid, "An empty username, should be invalid.");
        }

        [TestMethod]
        public void LogInFormUsernameValidationTest_InvalidUsername_SpecialCharacters()
        {
            string invalidUsername = "Invalid@User!";
            bool isValid = usernameValidator.IsValidInput(invalidUsername);
            Assert.IsFalse(isValid, "Usernames with special characters, should be invalid.");
        }

        [TestMethod]
        public void LogInFormUsernameValidationTest_ValidUsername_MinimumLength()
        {
            string validUsername = "User1";
            bool isValid = usernameValidator.IsValidInput(validUsername);
            Assert.IsTrue(isValid, "The username with minimum valid length, should be valid.");
        }

        [TestMethod]
        public void LogInFormUsernameValidationTest_UsernameWithSpaces()
        {
            string invalidUsername = "Invalid User";
            bool isValid = usernameValidator.IsValidInput(invalidUsername);
            Assert.IsFalse(isValid, "Usernames with spaces should be invalid.");
        }
        //UsernameValidation END
    }
}
