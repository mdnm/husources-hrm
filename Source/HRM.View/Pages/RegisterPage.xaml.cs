using HRM.Model;
using System.Collections.Generic;
using System.Security;

namespace HRM.View
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : BasePage<RegisterViewModel>, IHavePassword
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RegisterPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        public RegisterPage(RegisterViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        public SecureString SecurePassword => PasswordText.SecurePassword;

        public SecureString SecurePasswordConfirmation => PasswordConfirmationText.SecurePassword;

        bool IHavePassword.HasConfirmation => true;
        #endregion
    }
}
