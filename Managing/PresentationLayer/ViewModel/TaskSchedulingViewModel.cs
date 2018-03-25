using Managing.BusinessLogicLayer;
using Managing.DataAccessLayer;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Managing.Common.Extension;

namespace Managing.PresentationLayer.ViewModel
{
    class TaskSchedulingViewModel:ViewModelBase
    {
        #region private fields

        private const int HideMessageTimeOutInSecond = 5;
        private ITaskController _taskController;
        private IFileProvider _fileProvider;
        private string _statusMessage;
        private Visibility _statusMessageIsVisible = Visibility.Hidden;
        //private ObservableCollection<ScheduleTask> _tasks;

        #endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskSchedulingViewModel"/> class.        
        /// </summary>
        public TaskSchedulingViewModel(ITaskController taskController, IFileProvider fileProvider)
        {
            taskController.ThrowIfNull(nameof(taskController));
            fileProvider.ThrowIfNull(nameof(fileProvider));

            this.DisplayName = "ScheduleTask scheduling";
            _taskController = taskController;
            _fileProvider = fileProvider;
            //Events subscribe
        }

        public ObservableCollection<ScheduleTask> Tasks { get; set; }
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                WaitAndHideStatusMessage();
                _statusMessage = value;
                OnPropertyChanged();
            }
        }
        public Visibility StatusMessageIsVisible
        {
            get => _statusMessageIsVisible;
            set
            {
                _statusMessageIsVisible = value;
                OnPropertyChanged();
            }
        }

        private async void WaitAndHideStatusMessage()                                                                                                                                                                                                                    
        {
            StatusMessageIsVisible = Visibility.Visible;
            await Task.Factory.StartNew(() =>
                {
                    SpinWait.SpinUntil(() => false, TimeSpan.FromSeconds(HideMessageTimeOutInSecond));
                    StatusMessageIsVisible = Visibility.Hidden;
                }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default)
                .ConfigureAwait(false);
        }
    }
}
