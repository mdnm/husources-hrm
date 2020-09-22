namespace HRM.View
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class EmployeePage : BasePage<EmployeePageViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public EmployeePage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public EmployeePage(EmployeePageViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        #endregion
    }
}
