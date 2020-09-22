using System.ComponentModel;
using System.Windows.Controls;
using static HRM.View.DI;

namespace HRM.View
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();

            // Set data context to settings view model

            // If we are in design mode...
            if (DesignerProperties.GetIsInDesignMode(this))
                // Create new instance of settings view model
                DataContext = new SettingsViewModel();
            else
                DataContext = ViewModelSettings;
        }
    }
}
