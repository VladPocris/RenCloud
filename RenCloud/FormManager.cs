using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenCloud
{
    public static class FormManager
    {
        private static RegisterForm _registerForm;
        private static LogInForm _loginForm;
        private static LoadForm _loadForm;
        private static PassRes_Form _passResForm;
        private static UserInterfaceForm _userInterfaceForm;
        internal static bool IsTest {  get; set; }

        public static PassRes_Form PassRessFormInstance
        {
            get
            {
                // Only create if it hasn't been created already
                if (_passResForm == null || _passResForm.IsDisposed)
                {
                    _passResForm = new PassRes_Form();
                }
                return _passResForm;
            }
        }
        public static LoadForm LoadFormInstance
        {
            get
            {
                // Only create if it hasn't been created already
                if (_loadForm == null || _loadForm.IsDisposed)
                {
                    _loadForm = new LoadForm();
                }
                return _loadForm;
            }
        }
        public static RegisterForm RegisterFormInstance
        {
            get
            {
                // Only create if it hasn't been created already
                if (_registerForm == null || _registerForm.IsDisposed)
                {
                    _registerForm = new RegisterForm();
                }
                return _registerForm;
            }
        }
        public static LogInForm LogInFormInstance
        {
            get
            {
                // Only create if it hasn't been created already
                if (_loginForm == null || _loginForm.IsDisposed)
                {
                    _loginForm = new LogInForm();
                }
                return _loginForm;
            }
        }

        public static UserInterfaceForm UserInterfaceFormInstance
        {
            get
            {
                // Only create if it hasn't been created already
                if (_userInterfaceForm == null || _userInterfaceForm.IsDisposed)
                {
                    _userInterfaceForm = new UserInterfaceForm(TestingMethod());
                }
                return _userInterfaceForm;
            }
        }

        [ExcludeFromCodeCoverage]
        public static bool TestingMethod()
        {
            if (IsTest)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
