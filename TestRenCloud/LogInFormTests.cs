using RenCloud;
using System.Windows.Forms;

namespace LogInFormTests
{
    [TestClass]
    public class ButtonsStatus
    {

        private LogInForm loginForm = null!;
        private TextBox tbusername = null!;
        private TextBox tbpassword = null!;
        private Button loginButton = null!;
        private string tempPassword = null!;
        private string tempUsername = null!;
        private string tempPasswordWrong = null!;
        private string tempUsernameWrong = null!;

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
            loginForm.SimulateTextBoxesChange();
            Assert.IsTrue(loginButton.Enabled, "Login button should be enabled when both username and password are valid.");
        }

        [TestMethod]
        public void LogInButton_Enabled_UsernameAdmin_And_PasswordAdmin_ReturnsTrue()
        {
            tbusername.Text = "admin";
            tbpassword.Text = "admin";
            loginForm.SimulateTextBoxesChange();
            Assert.IsTrue(loginButton.Enabled, "Login button should be enabled when both username and password are valid.");
        }

        [TestMethod]
        public void LogInButton_Disabled_UsernameNotValid_And_PasswordValid_ReturnsFalse()
        {
            tbusername.Text = tempUsernameWrong;
            tbpassword.Text = tempPassword;
            loginForm.SimulateTextBoxesChange();
            Assert.IsFalse(loginButton.Enabled, "Login button should be disabled when username is not valid and password are valid.");
        }

        [TestMethod]
        public void LogInButton_Disabled_UsernameValid_And_PasswordNotValid_ReturnsFalse()
        {
            tbusername.Text = tempUsername;
            tbpassword.Text = tempPasswordWrong;
            loginForm.SimulateTextBoxesChange();
            Assert.IsFalse(loginButton.Enabled, "Login button should be disabled when username is valid and password invalid.");
        }

        [TestMethod]
        public void LogInButton_Disabled_UsernameNotValid_And_PasswordNotValid_ReturnsFalse()
        {
            tbusername.Text = tempUsernameWrong;
            tbpassword.Text = tempPasswordWrong;
            loginForm.SimulateTextBoxesChange();
            Assert.IsFalse(loginButton.Enabled, "Login button should be disabled when both username and password are invalid.");
        }
    }

    [TestClass]
    public class PlaceholderInteractionTests
    {
        private LogInForm loginForm = null!;
        private TextBox tbusername = null!;
        private TextBox tbpassword = null!;

        [TestInitialize]
        public void Setup()
        {
            loginForm = new LogInForm();
            tbusername = loginForm.UsernameTextBox;
            tbpassword = loginForm.PasswordTextBox;
        }

        [TestMethod]
        public void UsernameTextBox_Enter_ClearsPlaceholder()
        {
            tbusername.Text = "Type a username.";
            loginForm.UsernameTextBox.Focus();
            loginForm.SimulatePlaceHolderInteractionEnterUsername();

            Assert.AreNotEqual("Type a username.", loginForm.UsernameTextBox.Text, "Username placeholder should be cleared on focus.");
        }

        [TestMethod]
        public void UsernameTextBox_Leave_RestoresPlaceholderIfEmpty()
        {
            tbusername.Text = "";
            loginForm.UsernameTextBox.Focus();
            loginForm.SimulatePlaceHolderInteractionLeaveUsername();

            Assert.AreEqual("Type a username.", tbusername.Text, "Username placeholder should be restored when left empty.");
        }

        [TestMethod]
        public void PasswordTextBox_Enter_ClearsPlaceholderAndMasksInput()
        {
            tbpassword.Text = "Please type your password.";
            loginForm.PasswordTextBox.Focus();
            loginForm.SimulatePlaceHolderInteractionEnterPassword();

            Assert.AreNotEqual("Please type your password.", tbpassword.Text, "Password placeholder should be cleared on focus.");
            Assert.AreEqual('*', tbpassword.PasswordChar, "PasswordChar should be set to mask input.");
        }

        [TestMethod]
        public void PasswordTextBox_Leave_RestoresPlaceholderIfEmpty()
        {
            tbpassword.Text = "";
            loginForm.PasswordTextBox.Focus();
            loginForm.SimulatePlaceHolderInteractionLeavePassword();

            Assert.AreEqual("Please type your password.", tbpassword.Text, "Password placeholder should be restored when left empty.");
        }
    }


    [TestClass]
    public class InputsValidation
    {
        //UsernameValidation START//
        [TestMethod]
        public void IsValidUsername_UsernameValid_ReturnsTrue()
        {
            string validUsername = "ValidUser123";
            bool isValid = RenCloud.Program.UsernameValidator.IsValidUsername(validUsername);
            Assert.IsTrue(isValid, "Expected the username to be valid.");
        }

        [TestMethod]
        public void IsValidUsername_EmptyUsername_ReturnsFalse()
        {
            string invalidUsername = "";
            bool isValid = RenCloud.Program.UsernameValidator.IsValidUsername(invalidUsername);
            Assert.IsFalse(isValid, "An empty username, should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameWithSpecialCharacters_ReturnsFalse()
        {
            string invalidUsername = "Invalid@User!";
            bool isValid = RenCloud.Program.UsernameValidator.IsValidUsername(invalidUsername);
            Assert.IsFalse(isValid, "Usernames with special characters, should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_ValidUsernameWithMinimumLength_ReturnsTrue()
        {
            string validUsername = "User1";
            bool isValid = RenCloud.Program.UsernameValidator.IsValidUsername(validUsername);
            Assert.IsTrue(isValid, "The username with minimum valid length, should be valid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameWithSpaces_ReturnsFalse()
        {
            string invalidUsername = "Invalid User";
            bool isValid = RenCloud.Program.UsernameValidator.IsValidUsername(invalidUsername);
            Assert.IsFalse(isValid, "Usernames with spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameWithLeadingSpaces_ReturnsFalse()
        {
            string username = " TestUser";
            bool isValid = RenCloud.Program.UsernameValidator.IsValidUsername(username);
            Assert.IsFalse(isValid, "Usernames with leading spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameWithTrailingSpaces_ReturnsFalse()
        {
            string username = "TestUser ";
            bool isValid = RenCloud.Program.UsernameValidator.IsValidUsername(username);
            Assert.IsFalse(isValid, "Usernames with trailing spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameWithLeadingAndTrailingSpaces_ReturnsFalse()
        {
            string username = " TestUser ";
            bool isValid = RenCloud.Program.UsernameValidator.IsValidUsername(username);
            Assert.IsFalse(isValid, "Usernames with leading and trailing spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameWithNonASCIICharacters_ReturnsFalse()
        {
            string invalidUsername = "InvalidøUser";
            bool isValid = RenCloud.Program.UsernameValidator.IsValidUsername(invalidUsername);
            Assert.IsFalse(isValid, "Usernames with non-ASCII should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameShort_ReturnsFalse()
        {
            string invalidUsername = "aaa";
            bool isValid = RenCloud.Program.UsernameValidator.IsValidUsername(invalidUsername);
            Assert.IsFalse(isValid, "Usernames shorter than the minimum length should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameLong_ReturnsFalse()
        {
            string maxLengthUsername = new('a', 17);
            bool isValid = RenCloud.Program.UsernameValidator.IsValidUsername(maxLengthUsername);
            Assert.IsFalse(isValid, "Usernames longer than the maximum length should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameWithMaximumLength_ReturnsTrue()
        {
            string maxLengthUsername = new('a', 16);
            bool isValid = RenCloud.Program.UsernameValidator.IsValidUsername(maxLengthUsername);
            Assert.IsTrue(isValid, "Usernames exactly at the maximum length should be valid.");
        }
        //UsernameValidation END//

        //PasswordValidation Start//
        [TestMethod]
        public void IsValidPassword_EmptyPassword_ReturnsFalse()
        {
            string emptyPassword = "";
            bool isValid = RenCloud.Program.PasswordValidator.IsValidPassword(emptyPassword);
            Assert.IsFalse(isValid, "Empty passwords should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_ValidPassword_ReturnsTrue()
        {
            string validPassword = "Strong@123";
            bool isValid = RenCloud.Program.PasswordValidator.IsValidPassword(validPassword);
            Assert.IsTrue(isValid, "The validator should accept a valid password.");
        }

        [TestMethod]
        public void IsValidPassword_InvalidPassword_ReturnsFalse()
        {
            string invalidPassword = "weak";
            bool isValid = RenCloud.Program.PasswordValidator.IsValidPassword(invalidPassword);
            Assert.IsFalse(isValid, "Weak passwords should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithSpaces_ReturnsFalse()
        {
            string passwordWithSpaces = "Password With Spaces!A1";
            bool isValid = RenCloud.Program.PasswordValidator.IsValidPassword(passwordWithSpaces);
            Assert.IsFalse(isValid, "Passwords with spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithLeadingSpaces_ReturnsFalse()
        {
            string password = " 123";
            bool isValid = RenCloud.Program.PasswordValidator.IsValidPassword(password);
            Assert.IsFalse(isValid, "Passwords with leading spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithTrailingSpaces_ReturnsFalse()
        {
            string password = "123sdadw!@ ";
            bool isValid = RenCloud.Program.PasswordValidator.IsValidPassword(password);
            Assert.IsFalse(isValid, "Passwords with trailing spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithLeadingAndTrailingSpaces_ReturnsFalse()
        {
            string password = " 123sdadw!@ ";
            bool isValid = RenCloud.Program.PasswordValidator.IsValidPassword(password);
            Assert.IsFalse(isValid, "Passwords with leading and trailing spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithoutSpecialCharacters_ReturnsFalse()
        {
            string passwordWithSpaces = "Passw0rd1";
            bool isValid = RenCloud.Program.PasswordValidator.IsValidPassword(passwordWithSpaces);
            Assert.IsFalse(isValid, "Passwords with no special characters should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithoutNumbers_ReturnsFalse()
        {
            string passwordWithSpaces = "Passw!rd@";
            bool isValid = RenCloud.Program.PasswordValidator.IsValidPassword(passwordWithSpaces);
            Assert.IsFalse(isValid, "Passwords with no numbers should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithoutNumbersAndSpecialCharacters_ReturnsFalse()
        {
            string passwordWithSpaces = "Password";
            bool isValid = RenCloud.Program.PasswordValidator.IsValidPassword(passwordWithSpaces);
            Assert.IsFalse(isValid, "Passwords with no numbers and special characters should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithoutCharactersAndSpecialCharacters_ReturnsFalse()
        {
            string passwordWithSpaces = "111111111";
            bool isValid = RenCloud.Program.PasswordValidator.IsValidPassword(passwordWithSpaces);
            Assert.IsFalse(isValid, "Passwords with no characters and special characters should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithoutNumbersAndCharacters_ReturnsFalse()
        {
            string passwordWithSpaces = "@@@@@@@@";
            bool isValid = RenCloud.Program.PasswordValidator.IsValidPassword(passwordWithSpaces);
            Assert.IsFalse(isValid, "Passwords with no numbers and characters should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_ValidPasswordWithAllCriteria_ReturnsTrue()
        {
            string passwordWithSpecialChars = "P!ssw0rd1";
            bool isValid = RenCloud.Program.PasswordValidator.IsValidPassword(passwordWithSpecialChars);
            Assert.IsTrue(isValid, "Passwords with special characters, numbers and characters should be valid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithNonASCII_ReturnsFalse()
        {
            string passwordWithASCII = "P@sswørd1";
            bool isValid = RenCloud.Program.PasswordValidator.IsValidPassword(passwordWithASCII);
            Assert.IsFalse(isValid, "Passwords with ASCII should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_LongPassword_ReturnsFalse()
        {
            string longPassword = new string('a', 30) + "!1A";
            bool isValid = RenCloud.Program.PasswordValidator.IsValidPassword(longPassword);
            Assert.IsFalse(isValid, "Passwords exceeding the maximum length should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_ShortPassword_ReturnsFalse()
        {
            string shortPassword = "123";
            bool isValid = RenCloud.Program.PasswordValidator.IsValidPassword(shortPassword);
            Assert.IsFalse(isValid, "Passwords shorter than the minimum length should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_MaximumLengthPassword_ReturnsTrue()
        {
            string maxLengthPassword = new string('a', 29) + "!1A";
            bool isValid = RenCloud.Program.PasswordValidator.IsValidPassword(maxLengthPassword);
            Assert.IsTrue(isValid, "Passwords exactly at the maximum length should be valid.");
        }
        //PasswordValidation End//

        // EmailValidation Start //
        [TestMethod]
        public void IsValidEmail_EmptyEmail_ReturnsFalse()
        {
            string emptyEmail = "";
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(emptyEmail);
            Assert.IsFalse(isValid, "Empty emails should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_NullEmail_ReturnsFalse()
        {
            string nullEmail = String.Empty;
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(nullEmail);
            Assert.IsFalse(isValid, "Null emails should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            string validEmail = "test@example.com";
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(validEmail);
            Assert.IsTrue(isValid, "The validator should accept a valid email.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailMissingAtSymbol_ReturnsFalse()
        {
            string invalidEmail = "testexample.com";
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails without an '@' should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailMissingDomain_ReturnsFalse()
        {
            string invalidEmail = "test@.com";
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with an empty domain should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithOnlyNumbersInDomain_ReturnsFalse()
        {
            string invalidEmail = "test@123.com";
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with only numbers in the domain should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithSpecialCharactersInDomain_ReturnsFalse()
        {
            string invalidEmail = "test@ex!ample.com";
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with special characters in the domain should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithInvalidTLD_ReturnsFalse()
        {
            string invalidEmail = "test@example.x";
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with invalid TLDs should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithShortTLD_ReturnsFalse()
        {
            string invalidEmail = "test@example.c";
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with a TLD that is too short should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithLongTLD_ReturnsFalse()
        {
            string invalidEmail = "test@example.toolongtld";
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with a TLD that is too long should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithSpaces_ReturnsFalse()
        {
            string invalidEmail = "test @example.com";
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithLeadingAndTrailingSpaces_ReturnsFalse()
        {
            string invalidEmail = "  test@example.com  ";
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with leading and trailing spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithNonASCIICharacters_ReturnsFalse()
        {
            string invalidEmail = "test@exämple.com";
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with non-ASCII characters should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_ValidEmailWithDifferentCase_ReturnsTrue()
        {
            string validEmail = "Test@Example.com";
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(validEmail);
            Assert.IsTrue(isValid, "The validator should accept valid emails with different case letters.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithConsecutiveDotsInDomain_ReturnsFalse()
        {
            string invalidEmail = "test@ex..ample.com";
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with consecutive dots in the domain should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_ValidEmailWithMultipleSubdomains_ReturnsTrue()
        {
            string validEmail = "test@mail.subdomain.example.com";
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(validEmail);
            Assert.IsTrue(isValid, "The validator should accept emails with multiple subdomains.");
        }

        [TestMethod]
        public void IsValidEmail_ValidEmailWithDigitsInLocalPart_ReturnsTrue()
        {
            string validEmail = "123test@example.com";
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(validEmail);
            Assert.IsTrue(isValid, "The validator should accept emails with digits in the local part.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithInvalidCharactersInLocalPart_ReturnsFalse()
        {
            string invalidEmail = "test#example.com";
            bool isValid = RenCloud.Program.EmailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with invalid characters in the local part should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_RegexTimeout_ReturnsFalse()
        {
            string longNumericDomain = new string('1', 100000000);
            string email = $"user@{longNumericDomain}.com";

            bool result = Program.EmailValidator.IsValidEmail(email);

            Assert.IsFalse(result, "Expected false due to RegexMatchTimeoutException (DDOS Protection).");
        }

        // EmailValidation End //
    }

    //Other Tests START//
    [TestClass]
    public class CornersValidation
    {

        private LogInForm loginForm = null!;

        [DoNotParallelize]
        [TestMethod]
        public void OnActivated_isActiveStatus()
        {
            loginForm = new LogInForm();
            loginForm.SimulateActivation();
            Assert.IsFalse(loginForm.IsActive, "Corners should be changed in another collor");
        }

        [DoNotParallelize]
        [TestMethod]
        public void OnDeactivated_isActiveStatus()
        {
            loginForm = new LogInForm();
            loginForm.SimulateDeactivation();
            Assert.IsTrue(loginForm.IsActive, "Corners should be changed in another collor");
        }
    }

    [TestClass]
    public class LoginLoadingAndInitialisations
    {
        private LogInForm loginForm = null!;

        [TestMethod]
        public void LogInForm_Load_InitializesCorrectly()
        {
            loginForm = new LogInForm();
            loginForm.SimulateLoad();
            Assert.IsFalse(loginForm.IsActive, "isActive not initialised properly or changed inaproppriately. (Not LoginFormLoad Occured)");
        }

        [TestMethod]
        public void LogInForm_KeyDownEvents_Triggered()
        {
            loginForm = new LogInForm();
            KeyEventArgs key = new KeyEventArgs(Keys.Enter);
            loginForm.SimulateKeyDowns(key);
            Assert.IsTrue(loginForm.KeyDownTriggered, "KeyDown events have not been triggered. (An error occured or code changes applied inaproppriately)");
        }

        [TestMethod]
        public void LogInForm_KeyDownEvents_NotTriggeredOnNonEnterKey()
        {
            loginForm = new LogInForm();

            KeyEventArgs key = new KeyEventArgs(Keys.Escape);
            loginForm.SimulateKeyDowns(key);

            Assert.IsFalse(loginForm.KeyDownTriggered, "KeyDown should not be triggered for non-Enter keys.");
        }

        [TestMethod]
        public void LogInForm_OnLoad_UsernameBoxFocused()
        {
            loginForm = new LogInForm();

            Assert.IsTrue(loginForm.UserBoxFocus, "Username box should be the one focused on application start.");
            Assert.IsTrue(loginForm.SimulateFormShow(), "Form must be shown");
        }
    }

    [TestClass]
    public class LoginFormClicks
    {
        private LogInForm loginForm = null!;

        [TestMethod]
        public void LogInForm_Button2Click_InValidLogin_ClearInputsAndFocusOnPassword()
        {
            loginForm = new LogInForm();

            loginForm.SimulateInvalidLoginClick("adminWRONG", "kyekV48b!");
            loginForm.LoginButton.PerformClick();

            Assert.IsFalse(loginForm.LoginSuccess, "CRITICAL: LOGIN IS APROVED INSTEAD OF BEING DENIED");
            bool cleared = loginForm.PasswordTextBox.Text == string.Empty;
            Assert.IsTrue(cleared, "Expected password to be cleared after invalid login.");
            bool focused = loginForm.PassBoxFocus;
            Assert.IsTrue(focused, "Password textbox should be focused.");
        }

        [TestMethod]
        public void LogInForm_Button3Click_ApplicationCloses()
        {
            loginForm = new LogInForm();

            loginForm.SimulateButton3Click();

            loginForm.CloseButton.PerformClick();

            Assert.IsFalse(loginForm.LoginSuccess, "This button should skip the login process.");
        }

        [TestMethod]
        public void LogInForm_Button2Click_ValidLogin_HideTheformAndLoadOther()
        {
            loginForm = new LogInForm();

            loginForm.SimulateValidLoginClick("admin", "admin");
            
            loginForm.LoginButton.PerformClick();

            Assert.IsTrue(loginForm.LoginSuccess, "CRITICAL: LOGIN IS APROVED INSTEAD OF BEING DENIED");
        }

        [TestMethod]
        public void LogInForm_Button1Click_ApplicationCloses()
        {
            loginForm = new LogInForm();

            loginForm.SimuluateCloseButtonClick();
            loginForm.CloseButton.PerformClick();

            Assert.IsTrue(loginForm.ClosingForm, "Application must be closing");
        }

        [TestMethod]
        public void LogInForm_LinkLabel2Click_OpensRegistrationForm()
        {
            loginForm = new LogInForm();
            loginForm.SimuluateLink2LabelClick();

            Assert.IsTrue(loginForm.LinkClicked, "Click for link label 2 not regitered.");
            loginForm.LinkClicked = false;
        }

        [TestMethod]
        public void LogInForm_LinkLabel1Click_OpensRestorePasswordForm()
        {
            loginForm = new LogInForm();
            loginForm.SimuluateLink1LabelClick();

            Assert.IsTrue(loginForm.LinkClicked, "Click for link label 1 not regitered.");
            loginForm.LinkClicked = false;
        }
    }
    //Other Tests End//
}