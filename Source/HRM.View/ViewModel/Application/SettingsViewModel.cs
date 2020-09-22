using Dna;
using HRM.Model;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using static HRM.View.DI;
using static Dna.FrameworkDI;
using System.Linq.Expressions;
using System.Diagnostics;
using HRM.Service;
using HRM.Model.Entities;

namespace HRM.View
{
    /// <summary>
    /// The settings state as a view model
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// The text to show while loading text
        /// </summary>
        private string mLoadingText = "...";

        #endregion

        #region Public Properties
        /// <summary>
        /// The current users username
        /// </summary>
        public TextEntryViewModel Username { get; set; }

        /// <summary>
        /// The current users password
        /// </summary>
        public PasswordEntryViewModel Password { get; set; }

        /// <summary>
        /// The text for the logout button
        /// </summary>
        public string LogoutButtonText { get; set; }

        #region Transactional Properties

        /// <summary>
        /// Indicates if the first name is current being saved
        /// </summary>
        public bool FirstNameIsSaving { get; set; }

        /// <summary>
        /// Indicates if the last name is current being saved
        /// </summary>
        public bool LastNameIsSaving { get; set; }

        /// <summary>
        /// Indicates if the username is current being saved
        /// </summary>
        public bool UsernameIsSaving { get; set; }

        /// <summary>
        /// Indicates if the email is current being saved
        /// </summary>
        public bool EmailIsSaving { get; set; }

        /// <summary>
        /// Indicates if the password is current being changed
        /// </summary>
        public bool PasswordIsChanging { get; set; }

        /// <summary>
        /// Indicates if the settings details are currently being loaded
        /// </summary>
        public bool SettingsLoading { get; set; }

        /// <summary>
        /// Indicates if the user is currently logging out
        /// </summary>
        public bool LoggingOut { get; set; }

        #endregion

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to open the settings menu
        /// </summary>
        public ICommand OpenCommand { get; set; }

        /// <summary>
        /// The command to close the settings menu
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command to logout of the application
        /// </summary>
        public ICommand LogoutCommand { get; set; }

        /// <summary>
        /// The command to clear the users data from the view model
        /// </summary>
        public ICommand ClearUserDataCommand { get; set; }

        /// <summary>
        /// Loads the settings data from the client data store
        /// </summary>
        public ICommand LoadCommand { get; set; }

        /// <summary>
        /// Saves the current username to the server
        /// </summary>
        public ICommand SaveUsernameCommand { get; set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SettingsViewModel()
        {
            // Create Username
            Username = new TextEntryViewModel
            {
                Label = "Usuário",
                OriginalText = mLoadingText,
                CommitAction = SaveUsernameAsync
            };

            // Create Password
            Password = new PasswordEntryViewModel
            {
                Label = "Senha",
                FakePassword = "********",
                CommitAction = SavePasswordAsync
            };

            // Create commands
            OpenCommand = new RelayCommand(Open);
            CloseCommand = new RelayCommand(Close);
            LogoutCommand = new RelayCommand(async () => await LogoutAsync());
            ClearUserDataCommand = new RelayCommand(ClearUserData);
            LoadCommand = new RelayCommand(async () => await LoadAsync());
            SaveUsernameCommand = new RelayCommand(async () => await SaveUsernameAsync());

            LogoutButtonText = "Sair";
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Open the settings menu
        /// </summary>
        public void Open()
        {
            // Close settings menu
            ViewModelApplication.SettingsMenuVisible = true;
        }

        /// <summary>
        /// Closes the settings menu
        /// </summary>
        public void Close()
        {
            // Close settings menu
            ViewModelApplication.SettingsMenuVisible = false;
        }

        /// <summary>
        /// Logs the user out
        /// </summary>
        public async Task LogoutAsync()
        {
            // Lock this command to ignore any other requests while processing
            await RunCommandAsync(() => LoggingOut, async () =>
            {
                // Clear any user data/cache
                ClientDataStore.ClearAllLoginCredentials();

                // Clean all application level view models that contain
                // any information about the current user
                ClearUserData();

                await Task.Delay(1000);
                // Go to login page
                ViewModelApplication.GoToPage(ApplicationPage.Login);
            });
        }

        /// <summary>
        /// Clears any data specific to the current user
        /// </summary>
        public void ClearUserData()
        {
            // Clear all view models containing the users info
            Username.OriginalText = mLoadingText;
        }

        /// <summary>
        /// Sets the settings view model properties based on the data in the client data store
        /// </summary>
        public async Task LoadAsync()
        {
            // Lock this command to ignore any other requests while processing
            await RunCommandAsync(() => SettingsLoading, async () =>
            {
                // Store single transcient instance of client data store
                var scopedClientDataStore = ClientDataStore;

                // Get values from local cache
                var storedData = scopedClientDataStore.GetLoginCredentials();

                if (storedData == null)
                    return;

                // Load user profile details form server
                var response = new BaseService<HumanResourcesEmployee>().Get(storedData.Value);

                // If the response has an error...
                if (await response.HandleErrorIfFailedAsync())
                    // We are done
                    return;

                var serverData = (HumanResourcesEmployee)response.Data;

                // Save the new information in the data store
                scopedClientDataStore.SaveLoginCredentials(storedData.Value);

                // Update values from server
                UpdateValuesFromLocalStore(serverData);
            });
        }

        /// <summary>
        /// Saves the new Username to the server
        /// </summary>
        /// <returns>Returns true if successful, false otherwise</returns>
        public async Task<bool> SaveUsernameAsync()
        {
            // Lock this command to ignore any other requests while processing
            return await RunCommandAsync(() => UsernameIsSaving, async () =>
            {
                // Update the Username value on the server...
                return await UpdateUserCredentialsValueAsync(
                    // Display name
                    "Username",
                    // Update the first name
                    (credentials) => credentials.Username,
                    // To new value
                    Username.OriginalText,
                    // Set Api model value
                    (apiModel, value) => apiModel.Username = value
                    );
            });
        }

        /// <summary>
        /// Saves the new Password to the server
        /// </summary>
        /// <returns>Returns true if successful, false otherwise</returns>
        public async Task<bool> SavePasswordAsync()
        {
            // Lock this command to ignore any other requests while processing
            return await RunCommandAsync(() => PasswordIsChanging, async () =>
            {
                return true;
            });
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Loads the settings from the local data store and binds them 
        /// to this view model
        /// </summary>
        /// <returns></returns>
        private void UpdateValuesFromLocalStore(HumanResourcesEmployee storedData)
        {
            // Set username
            Username.OriginalText = storedData?.Username;
        }

        /// <summary>
        /// Updates a specific value from the client data store for the user profile details
        /// and attempts to update the server to match those details.
        /// For example, updating the first name of the user.
        /// </summary>
        /// <param name="displayName">The display name for logging and display purposes of the property we are updating</param>
        /// <param name="propertyToUpdate">The property from the <see cref="LoginCredentialsDataModel"/> to be updated</param>
        /// <param name="newValue">The new value to update the property to</param>
        /// <param name="setApiModel">Sets the correct property in the <see cref="UpdateUserProfileApiModel"/> model that this property maps to</param>
        /// <returns></returns>
        private async Task<bool> UpdateUserCredentialsValueAsync(string displayName, Expression<Func<HumanResourcesEmployee, string>> propertyToUpdate, string newValue, Action<UpdateUserProfileApiModel, string> setApiModel)
        {
            return true;
        }

        #endregion
    }
}
