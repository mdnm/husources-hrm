using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.View
{
    public class NewEmployeeDesignModel : NewEmployeeViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static NewEmployeeDesignModel Instance => new NewEmployeeDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public NewEmployeeDesignModel()
        {
            Name = new TextEntryViewModel { Label = "Nome", OriginalText = "luke" };
        }

        #endregion
    }
}
