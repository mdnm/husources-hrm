using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HRM.View
{
    /// <summary>
    /// The design-time data for a <see cref="EmployeePageViewModel"/>
    /// </summary>
    public class SalaryDesignModel : EmployeePageViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static SalaryDesignModel Instance => new SalaryDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SalaryDesignModel()
        {
        }

        #endregion
    }
}
