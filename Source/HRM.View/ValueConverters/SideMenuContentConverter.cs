using HRM.Model;
using System;
using System.Globalization;

namespace HRM.View
{
    /// <summary>
    /// A converter that takes a <see cref="SideMenuContent"/> and converts it to the 
    /// correct UI element
    /// </summary>
    public class SideMenuContentConverter : BaseValueConverter<SideMenuContentConverter>
    {
        #region Protected Members

        /// <summary>
        /// An instance of the current chat list control
        /// </summary>
        protected EmployeeListControl mEmployeeListControl = new EmployeeListControl();

        #endregion

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get the side menu type
            var sideMenuType = (SideMenuContent)value;

            // Switch based on type
            switch (sideMenuType)
            {
                // Employees 
                case SideMenuContent.Employees:
                    return mEmployeeListControl;

                // Unknown
                default:
                    return "No UI yet, sorry :)";
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
