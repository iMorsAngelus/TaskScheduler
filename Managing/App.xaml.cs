using System;
using System.Collections.Generic;
using System.Windows;
using log4net;
using Managing.BusinessLogicLayer;
using Managing.DataAccessLayer;
using Managing.PresentationLayer.ViewModel;

namespace Managing
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(App));

        /// <summary>Raises the <see cref="E:System.Windows.Application.Startup" /> event.</summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            var version = typeof(App).Assembly.GetName().Version;
            Log.Info($"Managing app started. Programm version: {version}.");

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            base.OnStartup(e);

            var fileProvider = new FileProvider();
            var taskController = new TaskController(fileProvider);

            var taskSchedulingViewModel = new TaskSchedulingViewModel(taskController, fileProvider);
            var viewModelList = new List<ViewModelBase> {taskSchedulingViewModel};

            var mainWindowViewModel = new MainWindowViewModel(viewModelList);
            var mainWindow = new MainWindow {DataContext = mainWindowViewModel};

            Log.Info("Initialize is successful");
            mainWindow.Show();
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;
            Log.Error(e.IsTerminating ? "Application is terminating because of an unhandled exception." : "Unhandled thread exception.", exception);
            if (!e.IsTerminating)
            {
                return;
            }

            var message = $"Unhandled exception has occurred.\n \"{exception.Message}\"\nThe application will be terminated.";
            MessageBox.Show(message, "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            Environment.Exit(0);
        }
    }
}
