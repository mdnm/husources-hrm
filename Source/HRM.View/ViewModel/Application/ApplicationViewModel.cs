using HRM.Model;
using System.Threading.Tasks;
using static HRM.View.DI;
using static HRM.Model.CoreDI;
using System.Windows.Input;
using HRM.Model.Entities;
using System;
using System.Collections.Generic;
using HRM.Service;
using System.Linq;

namespace HRM.View
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// True if the settings menu should be shown
        /// </summary>
        private bool mSettingsMenuVisible;

        private bool mNewEmployeeMenuVisible;

        #endregion

        #region Public Properties

        /// <summary>
        /// The current page of the application
        /// </summary>
        public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.Login;

        /// <summary>
        /// The view model to use for the current page when the CurrentPage changes
        /// NOTE: This is not a live up-to-date view model of the current page
        ///       it is simply used to set the view model of the current page 
        ///       at the time it changes
        /// </summary>
        public BaseViewModel CurrentPageViewModel { get; set; }

        /// <summary>
        /// True if the side menu should be shown
        /// </summary>
        public bool SideMenuVisible { get; set; } = false;

        /// <summary>
        /// True if the settings menu should be shown
        /// </summary>
        public bool SettingsMenuVisible
        {
            get => mSettingsMenuVisible;
            set
            {
                // If property has not changed...
                if (mSettingsMenuVisible == value)
                    // Ignore
                    return;

                // Set the backing field
                mSettingsMenuVisible = value;

                // If the settings menu is now visible...
                if (value)
                    // Reload settings
                    TaskManager.RunAndForget(ViewModelSettings.LoadAsync);
            }
        }

        /// <summary>
        /// True if the new employee menu should be shown
        /// </summary>
        public bool NewEmployeeMenuVisible
        {
            get => mNewEmployeeMenuVisible;
            set
            {
                // If property has not changed...
                if (mNewEmployeeMenuVisible == value)
                    // Ignore
                    return;

                // Set the backing field
                mNewEmployeeMenuVisible = value;
            }
        }

        /// <summary>
        /// Determines the currently visible side menu content
        /// </summary>
        public SideMenuContent CurrentSideMenuContent { get; set; } = SideMenuContent.Employees;

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to change the side menu to the Chat
        /// </summary>
        public ICommand OpenEmployeeListCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public ApplicationViewModel()
        {
            // Create the commands
            OpenEmployeeListCommand = new RelayCommand(OpenEmployeesList);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Changes the current side menu to the Employees List
        /// </summary>
        public void OpenEmployeesList()
        {
            // Set the current side menu to Employees List
            CurrentSideMenuContent = SideMenuContent.Employees;
            ViewModelEmployeeList.AddEmployees();
        }

        #endregion

        #region Public Helper Methods

        /// <summary>
        /// Navigates to the specified page
        /// </summary>
        /// <param name="page">The page to go to</param>
        /// <param name="viewModel">The view model, if any, to set explicitly to the new page</param>
        public void GoToPage(ApplicationPage page, BaseViewModel viewModel = null)
        {
            // Always hide settings page if we are changing pages
            SettingsMenuVisible = false;
            NewEmployeeMenuVisible = false;

            // Set the view model
            CurrentPageViewModel = viewModel;

            // See if page has changed
            var different = CurrentPage != page;

            // Set the current page
            CurrentPage = page;

            // If the page hasn't changed, fire off notification
            // So pages still update if just the view model has changed
            if (!different)
                OnPropertyChanged(nameof(CurrentPage));

            // Show side menu or not?
            SideMenuVisible = page == ApplicationPage.Employee || page == ApplicationPage.NullPage;

        }

        /// <summary>
        /// Handles what happens when we have successfully logged in
        /// </summary>
        /// <param name="userId">The results from the successful login</param>
        public async Task HandleSuccessfulLoginAsync(Guid userId)
        {
            // Store this in the client data store
            ClientDataStore.SaveLoginCredentials(userId);

            // Load new settings
            await ViewModelSettings.LoadAsync();

            // Go to chat page
            ViewModelApplication.GoToPage(ApplicationPage.NullPage);
        }

        #endregion
    }
}
