using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RenCloud;
using static RenCloud.Program;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TestRenCloud
{
    [TestClass]
    public class InputsValidation
    {
        private UsernameValidator usernameValidator;
        private PasswordValidator passwordValidator;
        private EmailValidator emailValidator;

        [TestInitialize]
        public void Setup()
        {
            usernameValidator = new UsernameValidator();
            passwordValidator = new PasswordValidator();
            emailValidator = new EmailValidator();
        }

        //UsernameValidation START//
        [TestMethod]
        public void IsValidUsername_UsernameValid_ReturnsTrue()
        {
            string validUsername = "ValidUser123";
            bool isValid = usernameValidator.IsValidUsername(validUsername);
            Assert.IsTrue(isValid, "Expected the username to be valid.");
        }

        [TestMethod]
        public void IsValidUsername_EmptyUsername_ReturnsFalse()
        {
            string invalidUsername = "";
            bool isValid = usernameValidator.IsValidUsername(invalidUsername);
            Assert.IsFalse(isValid, "An empty username, should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameWithSpecialCharacters_ReturnsFalse()
        {
            string invalidUsername = "Invalid@User!";
            bool isValid = usernameValidator.IsValidUsername(invalidUsername);
            Assert.IsFalse(isValid, "Usernames with special characters, should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_ValidUsernameWithMinimumLength_ReturnsTrue()
        {
            string validUsername = "User1";
            bool isValid = usernameValidator.IsValidUsername(validUsername);
            Assert.IsTrue(isValid, "The username with minimum valid length, should be valid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameWithSpaces_ReturnsFalse()
        {
            string invalidUsername = "Invalid User";
            bool isValid = usernameValidator.IsValidUsername(invalidUsername);
            Assert.IsFalse(isValid, "Usernames with spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameWithLeadingSpaces_ReturnsFalse()
        {
            string username = " TestUser";
            bool isValid = usernameValidator.IsValidUsername(username);
            Assert.IsFalse(isValid, "Usernames with leading spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameWithTrailingSpaces_ReturnsFalse()
        {
            string username = "TestUser ";
            bool isValid = usernameValidator.IsValidUsername(username);
            Assert.IsFalse(isValid, "Usernames with trailing spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameWithLeadingAndTrailingSpaces_ReturnsFalse()
        {
            string username = " TestUser ";
            bool isValid = usernameValidator.IsValidUsername(username);
            Assert.IsFalse(isValid, "Usernames with leading and trailing spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameWithNonASCIICharacters_ReturnsFalse()
        {
            string invalidUsername = "InvalidøUser";
            bool isValid = usernameValidator.IsValidUsername(invalidUsername);
            Assert.IsFalse(isValid, "Usernames with non-ASCII should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameShort_ReturnsFalse()
        {
            string invalidUsername = "aaa";
            bool isValid = usernameValidator.IsValidUsername(invalidUsername);
            Assert.IsFalse(isValid, "Usernames shorter than the minimum length should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameLong_ReturnsFalse()
        {
            string maxLengthUsername = new string('a', 17);
            bool isValid = usernameValidator.IsValidUsername(maxLengthUsername);
            Assert.IsFalse(isValid, "Usernames longer than the maximum length should be invalid.");
        }

        [TestMethod]
        public void IsValidUsername_UsernameWithMaximumLength_ReturnsTrue()
        {
            string maxLengthUsername = new string('a', 16);
            bool isValid = usernameValidator.IsValidUsername(maxLengthUsername);
            Assert.IsTrue(isValid, "Usernames exactly at the maximum length should be valid.");
        }
        //UsernameValidation END//

        //PasswordValidation Start//
        [TestMethod]
        public void IsValidPassword_EmptyPassword_ReturnsFalse()
        {
            string emptyPassword = "";
            bool isValid = passwordValidator.IsValidPassword(emptyPassword);
            Assert.IsFalse(isValid, "Empty passwords should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_ValidPassword_ReturnsTrue()
        {
            string validPassword = "Strong@123";
            bool isValid = passwordValidator.IsValidPassword(validPassword);
            Assert.IsTrue(isValid, "The validator should accept a valid password.");
        }

        [TestMethod]
        public void IsValidPassword_InvalidPassword_ReturnsFalse()
        {
            string invalidPassword = "weak";
            bool isValid = passwordValidator.IsValidPassword(invalidPassword);
            Assert.IsFalse(isValid, "Weak passwords should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithSpaces_ReturnsFalse()
        {
            string passwordWithSpaces = "Password With Spaces!A1";
            bool isValid = passwordValidator.IsValidPassword(passwordWithSpaces);
            Assert.IsFalse(isValid, "Passwords with spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithLeadingSpaces_ReturnsFalse()
        {
            string password = " 123";
            bool isValid = passwordValidator.IsValidPassword(password);
            Assert.IsFalse(isValid, "Passwords with leading spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithTrailingSpaces_ReturnsFalse()
        {
            string password = "123sdadw!@ ";
            bool isValid = passwordValidator.IsValidPassword(password);
            Assert.IsFalse(isValid, "Passwords with trailing spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithLeadingAndTrailingSpaces_ReturnsFalse()
        {
            string password = " 123sdadw!@ ";
            bool isValid = passwordValidator.IsValidPassword(password);
            Assert.IsFalse(isValid, "Passwords with leading and trailing spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithoutSpecialCharacters_ReturnsFalse()
        {
            string passwordWithSpaces = "Passw0rd1";
            bool isValid = passwordValidator.IsValidPassword(passwordWithSpaces);
            Assert.IsFalse(isValid, "Passwords with no special characters should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithoutNumbers_ReturnsFalse()
        {
            string passwordWithSpaces = "Passw!rd@";
            bool isValid = passwordValidator.IsValidPassword(passwordWithSpaces);
            Assert.IsFalse(isValid, "Passwords with no numbers should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithoutNumbersAndSpecialCharacters_ReturnsFalse()
        {
            string passwordWithSpaces = "Password";
            bool isValid = passwordValidator.IsValidPassword(passwordWithSpaces);
            Assert.IsFalse(isValid, "Passwords with no numbers and special characters should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithoutCharactersAndSpecialCharacters_ReturnsFalse()
        {
            string passwordWithSpaces = "111111111";
            bool isValid = passwordValidator.IsValidPassword(passwordWithSpaces);
            Assert.IsFalse(isValid, "Passwords with no characters and special characters should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithoutNumbersAndCharacters_ReturnsFalse()
        {
            string passwordWithSpaces = "@@@@@@@@";
            bool isValid = passwordValidator.IsValidPassword(passwordWithSpaces);
            Assert.IsFalse(isValid, "Passwords with no numbers and characters should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_ValidPasswordWithAllCriteria_ReturnsTrue()
        {
            string passwordWithSpecialChars = "P!ssw0rd1";
            bool isValid = passwordValidator.IsValidPassword(passwordWithSpecialChars);
            Assert.IsTrue(isValid, "Passwords with special characters, numbers and characters should be valid.");
        }

        [TestMethod]
        public void IsValidPassword_PasswordWithNonASCII_ReturnsFalse()
        {
            string passwordWithASCII = "P@sswørd1";
            bool isValid = passwordValidator.IsValidPassword(passwordWithASCII);
            Assert.IsFalse(isValid, "Passwords with ASCII should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_LongPassword_ReturnsFalse()
        {
            string longPassword = new string('a', 30) + "!1A";
            bool isValid = passwordValidator.IsValidPassword(longPassword);
            Assert.IsFalse(isValid, "Passwords exceeding the maximum length should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_ShortPassword_ReturnsFalse()
        {
            string shortPassword = "123";
            bool isValid = passwordValidator.IsValidPassword(shortPassword);
            Assert.IsFalse(isValid, "Passwords shorter than the minimum length should be invalid.");
        }

        [TestMethod]
        public void IsValidPassword_MaximumLengthPassword_ReturnsTrue()
        {
            string maxLengthPassword = new string('a', 29) + "!1A";
            bool isValid = passwordValidator.IsValidPassword(maxLengthPassword);
            Assert.IsTrue(isValid, "Passwords exactly at the maximum length should be valid.");
        }
        //PasswordValidation End//

        // EmailValidation Start //
        [TestMethod]
        public void IsValidEmail_EmptyEmail_ReturnsFalse()
        {
            string emptyEmail = "";
            bool isValid = emailValidator.IsValidEmail(emptyEmail);
            Assert.IsFalse(isValid, "Empty emails should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_NullEmail_ReturnsFalse()
        {
            string nullEmail = null;
            bool isValid = emailValidator.IsValidEmail(nullEmail);
            Assert.IsFalse(isValid, "Null emails should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            string validEmail = "test@example.com";
            bool isValid = emailValidator.IsValidEmail(validEmail);
            Assert.IsTrue(isValid, "The validator should accept a valid email.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailMissingAtSymbol_ReturnsFalse()
        {
            string invalidEmail = "testexample.com";
            bool isValid = emailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails without an '@' should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailMissingDomain_ReturnsFalse()
        {
            string invalidEmail = "test@.com";
            bool isValid = emailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with an empty domain should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithOnlyNumbersInDomain_ReturnsFalse()
        {
            string invalidEmail = "test@123.com";
            bool isValid = emailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with only numbers in the domain should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithSpecialCharactersInDomain_ReturnsFalse()
        {
            string invalidEmail = "test@ex!ample.com";
            bool isValid = emailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with special characters in the domain should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithInvalidTLD_ReturnsFalse()
        {
            string invalidEmail = "test@example.x";
            bool isValid = emailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with invalid TLDs should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithShortTLD_ReturnsFalse()
        {
            string invalidEmail = "test@example.c";
            bool isValid = emailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with a TLD that is too short should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithLongTLD_ReturnsFalse()
        {
            string invalidEmail = "test@example.toolongtld";
            bool isValid = emailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with a TLD that is too long should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithSpaces_ReturnsFalse()
        {
            string invalidEmail = "test @example.com";
            bool isValid = emailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithLeadingAndTrailingSpaces_ReturnsFalse()
        {
            string invalidEmail = "  test@example.com  ";
            bool isValid = emailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with leading and trailing spaces should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithNonASCIICharacters_ReturnsFalse()
        {
            string invalidEmail = "test@exämple.com";
            bool isValid = emailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with non-ASCII characters should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_ValidEmailWithDifferentCase_ReturnsTrue()
        {
            string validEmail = "Test@Example.com";
            bool isValid = emailValidator.IsValidEmail(validEmail);
            Assert.IsTrue(isValid, "The validator should accept valid emails with different case letters.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithConsecutiveDotsInDomain_ReturnsFalse()
        {
            string invalidEmail = "test@ex..ample.com";
            bool isValid = emailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with consecutive dots in the domain should be invalid.");
        }

        [TestMethod]
        public void IsValidEmail_ValidEmailWithMultipleSubdomains_ReturnsTrue()
        {
            string validEmail = "test@mail.subdomain.example.com";
            bool isValid = emailValidator.IsValidEmail(validEmail);
            Assert.IsTrue(isValid, "The validator should accept emails with multiple subdomains.");
        }

        [TestMethod]
        public void IsValidEmail_ValidEmailWithDigitsInLocalPart_ReturnsTrue()
        {
            string validEmail = "123test@example.com";
            bool isValid = emailValidator.IsValidEmail(validEmail);
            Assert.IsTrue(isValid, "The validator should accept emails with digits in the local part.");
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmailWithInvalidCharactersInLocalPart_ReturnsFalse()
        {
            string invalidEmail = "test#example.com";
            bool isValid = emailValidator.IsValidEmail(invalidEmail);
            Assert.IsFalse(isValid, "Emails with invalid characters in the local part should be invalid.");
        }

        // EmailValidation End //
    }
}
