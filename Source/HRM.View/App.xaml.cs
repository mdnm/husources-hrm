using Dna;
using HRM.Model;
using HRM.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows;
using static Dna.FrameworkDI;
using static HRM.Model.CoreDI;
using static HRM.View.DI;
using Hanssens.Net;
using HRM.Model.Entities;
using System.Linq;

namespace HRM.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Custom startup so we load our IoC immediately before anything else
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            // Let the base application do what it needs
            base.OnStartup(e);

            // Setup the main application 
            await ApplicationSetupAsync();

            // Setup the application view model based on if we are logged in
            ViewModelApplication.GoToPage(
                // If we are logged in...
                ClientDataStore.HasCredentials() ?
                // Go to nulls page
                ApplicationPage.NullPage : 
                // Otherwise, go to login page
                ApplicationPage.Login);

            // Show the main window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }

        /// <summary>
        /// Configures our application ready for use
        /// </summary>
        private Task ApplicationSetupAsync()
        {
            // Setup the Dna Framework
            Framework.Construct<DefaultFrameworkConstruction>()
                .AddFileLogger()
                .AddClientDataStore()
                .AddViewModels()
                .AddClientServices()
                .Build();

            // Load new settings
            TaskManager.RunAndForget(ViewModelSettings.LoadAsync);

            return Task.CompletedTask;
        }
    }
}
