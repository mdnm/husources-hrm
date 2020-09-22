using System.Windows.Input;

namespace HRM.View
{
    /// <summary>
    /// A base view model for any dialogs
    /// </summary>
    public class BaseDialogViewModel : BaseViewModel
    {
        /// <summary>
        /// The title of the dialog
        /// </summary>
        public string Title { get; set; }

        public ICommand YesCommand { get; set; }

        public ICommand NoCommand { get; set; }

        public bool HaveNoButton { get; set; }

        public bool IsYesCommandRunning { get; set; }

        public bool IsNoCommandRunning { get; set; }

        public DialogWindow YesCommandParameter { get; set; }

        public DialogWindow NoCommandParameter { get; set; }
    }
}
