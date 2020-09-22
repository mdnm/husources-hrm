using System.Collections.Generic;

namespace HRM.View
{
    /// <summary>
    /// The design-time data for a <see cref="SettingsViewModel"/>
    /// </summary>
    public class SettingsDesignModel : SettingsViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static SettingsDesignModel Instance => new SettingsDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SettingsDesignModel()
        {
            Username = new TextEntryViewModel { Label = "Usuário", OriginalText = "luke" };
            Password = new PasswordEntryViewModel { Label = "Senha", FakePassword = "********" };
        }

        #endregion
    }
}
