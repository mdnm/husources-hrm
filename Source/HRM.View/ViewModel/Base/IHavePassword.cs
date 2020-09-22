using System.Collections.Generic;
using System.Security;

namespace HRM.View
{
    /// <summary>
    /// An interface for a class that can provide a secure password
    /// </summary>
    public interface IHavePassword
    {
        /// <summary>
        /// The secure password
        /// </summary>
        SecureString SecurePassword { get; }

        SecureString SecurePasswordConfirmation { get; }
        bool HasConfirmation { get; }
    }
}
