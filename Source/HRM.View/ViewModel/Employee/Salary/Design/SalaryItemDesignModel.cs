using System;

namespace HRM.View
{
    /// <summary>
    /// The design-time data for a <see cref="SalaryItemViewModel"/>
    /// </summary>
    public class SalaryItemDesignModel : SalaryItemViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static SalaryItemDesignModel Instance => new SalaryItemDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SalaryItemDesignModel()
        {
        }

        #endregion
    }
}
