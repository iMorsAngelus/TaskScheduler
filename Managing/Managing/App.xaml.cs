using System.Windows;
using Managing.PresentationLayer.ViewModel;

namespace Managing
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var taskSchedulingViewModel = new TaskSchedulingViewModel();
            var mainWindowViewModel = new MainWindowViewModel(taskSchedulingViewModel);
            var mainWindow = new MainWindow() {DataContext = mainWindowViewModel};

            mainWindow.Show();
        }
    }
}
