using PrintToCash.Pages;
using System;
using System.Windows;

namespace PrintToCash
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Start the loading process (simulated by a timer)
            StartLoading();
        }

        private void StartLoading()
        {
            // Create a new main window
            var mainWindow = new MainWindow();

            // Show the main window
            mainWindow.Show();

            // Simulate loading using a timer
            var loadingTimer = new System.Windows.Threading.DispatcherTimer();
            loadingTimer.Interval = TimeSpan.FromSeconds(1.5);
            loadingTimer.Tick += (sender, args) =>
            {
                // Stop the timer
                loadingTimer.Stop();

                // Navigate to the MainMenuPage
                var mainMenuPage = new MainMenuPage();
                mainWindow.Content = mainMenuPage;
            };

            // Start the loading timer
            loadingTimer.Start();
        }
    }
}
