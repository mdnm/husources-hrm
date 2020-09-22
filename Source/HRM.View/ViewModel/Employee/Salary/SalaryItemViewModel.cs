using HRM.Model.Entities.Interfaces;
using HRM.Model.Enum.Salary;
using System;
using System.Windows;
using System.Windows.Input;

namespace HRM.View
{
    /// <summary>
    /// A view model for each chat message thread item in a chat thread
    /// </summary>
    public class SalaryItemViewModel : BaseViewModel
    {
        private string mName;
        /// <summary>
        /// The display name of the sender of the message
        /// </summary>
        public string Name 
        {
            get => mName;
            set 
            {
                mName = value;
                DisplayName = value;
            } 
        }

        private string mDisplayName;
        /// <summary>
        /// The latest message from this chat
        /// </summary>
        public string DisplayName
        {
            get => mDisplayName;
            set => mDisplayName = $"{value}: ";
        }

        private decimal mAmount;
        /// <summary>
        /// The initials to show for the profile picture background
        /// </summary>
        public decimal Amount 
        { 
            get => mAmount;
            set 
            {
                mAmount = value;
                FormattedAmount = mAmount.ToString("C");
            } 
        }

        /// <summary>
        /// The RGB values (in hex) for the background color of the profile picture
        /// For example FF00FF for Red and Blue mixed
        /// </summary>
        public string FormattedAmount { get; set; } 

        /// <summary>
        /// A flag indicating if this item was added since the first main list of items was created
        /// Used as a flag for animating in
        /// </summary>
        public bool NewSalary { get; set; }

        public SalaryType SalaryType { get; set; }

        public ICommand DeleteCommand { get; set; }

        public bool DeleteIsRunning { get; set; }
    }
}
