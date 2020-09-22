using HRM.Model;
using HRM.Model.Entities;
using HRM.Service;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static HRM.View.DI;

namespace HRM.View
{
    /// <summary>
    /// A view model for each chat list item in the overview chat list
    /// </summary>
    public class EmployeeListItemViewModel : BaseViewModel
    {
        #region Protected Properties
        protected Employee mEmployee;
        #endregion

        #region Public Properties

        /// <summary>
        /// The display name of this chat list
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The initials to show for the profile picture background
        /// </summary>
        public string Initials { get; set; }

        /// <summary>
        /// The RGB values (in hex) for the background color of the profile picture
        /// For example FF00FF for Red and Blue mixed
        /// </summary>
        public string ProfilePictureRGB { get; set; }

        public bool IsAddButton { get; set; }

        /// <summary>
        /// True if this item is currently selected
        /// </summary>
        public bool IsSelected { get; set; }

        public Employee Employee
        {
            get => mEmployee;
            set
            {
                mEmployee = value;
                DisplayName = value.Name;
                Initials = value.Name.Split(' ').Length > 1 ? value.Name.Substring(0, 1).ToUpper() + value.Name.Split(' ')[1].Substring(0, 1).ToUpper() : value.Name.Substring(0, 2).ToUpper();
            }
        }
        #endregion

        #region Public Commands

        /// <summary>
        /// Opens the current message thread
        /// </summary>
        public ICommand OpenEmployeePageCommand { get; set; }

        #endregion

        #region Private Properties
        private BaseService<Employee> mEmployeeService;
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public EmployeeListItemViewModel(bool isAddButton = false)
        {
            mEmployeeService = new BaseService<Employee>();

            ProfilePictureRGB = "3099c5";
            IsAddButton = isAddButton;

            // Create commands
            OpenEmployeePageCommand = new RelayCommand(OpenEmployeePage);
        }

        #endregion

        #region Command Methods

        public void OpenEmployeePage()
        {
            mEmployeeService.Get();

            if (!IsAddButton)
            {
                ViewModelApplication.GoToPage(ApplicationPage.Employee, new EmployeePageViewModel
                {
                    Employee = mEmployee
                });
            }
            else
            {
                ViewModelApplication.NewEmployeeMenuVisible = true;
            }
        }

        #endregion
    }
}
