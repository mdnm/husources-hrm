using System.Collections.Generic;

namespace HRM.View
{
    /// <summary>
    /// The design-time data for a <see cref="EmployeeListViewModel"/>
    /// </summary>
    public class EmployeeListDesignModel : EmployeeListViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static EmployeeListDesignModel Instance => new EmployeeListDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public EmployeeListDesignModel()
        {
            //Items = new List<EmployeeListItemViewModel>
            //{
            //    new EmployeeListItemViewModel
            //    {
            //        Initials = "MN",
            //        DisplayName = "Mateus",
            //        ProfilePictureRGB = "3099c5"
            //    }
            //};
        }

        #endregion
    }
}
