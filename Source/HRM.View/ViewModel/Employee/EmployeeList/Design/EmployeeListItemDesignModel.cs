namespace HRM.View
{
    /// <summary>
    /// The design-time data for a <see cref="EmployeeListItemViewModel"/>
    /// </summary>
    public class EmployeeListItemDesignModel : EmployeeListItemViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static EmployeeListItemDesignModel Instance => new EmployeeListItemDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public EmployeeListItemDesignModel()
        {
            //Initials = "MN";
            //DisplayName = "Mateus";
            //ProfilePictureRGB = "3099c5";
        }

        #endregion
    }
}
