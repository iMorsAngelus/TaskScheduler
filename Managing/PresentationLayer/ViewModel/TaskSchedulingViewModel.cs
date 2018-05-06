using Managing.BusinessLogicLayer;
using Managing.DataAccessLayer;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Managing.Common.Extension;
using Managing.PresentationLayer.Command;

namespace Managing.PresentationLayer.ViewModel
{
    class TaskSchedulingViewModel:ViewModelBase
    {
        #region private fields

        private const int HideMessageTimeOutInSecond = 5;
        private readonly ITaskController _taskController;
        private readonly IFileProvider _fileProvider;
        private string _statusMessage;
        private Visibility _statusMessageIsVisible = Visibility.Hidden;

        private ActionCommand _addTaskCommand;
        private ActionCommand _removeTaskCommand;
        private ActionCommand _editTaskCommand;
        private ActionCommand _showTaskDetailsCommand;
        /*
        private ActionCommand _confirmCommand;
        private ActionCommand _cancelCommand;
        private ActionCommand _choisePassCommand;
        private ScheduleTask _scheduleTask;
        */
        private TaskViewModel _taskViewModel;
        private ObservableCollection<ScheduleTask> _tasks;
        private bool _showTaskView = false;
        private bool _showTaskSchedulingView = true;

        #endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskSchedulingViewModel"/> class.        
        /// </summary>
        public TaskSchedulingViewModel(ITaskController taskController, IFileProvider fileProvider)
        {
            taskController.ThrowIfNull(nameof(taskController));
            fileProvider.ThrowIfNull(nameof(fileProvider));

            DisplayName = "Scheduling tasks";
            _taskController = taskController;
            _fileProvider = fileProvider;
            //Events subscribe
        }

        public ObservableCollection<ScheduleTask> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged();
            }
        }

        public TaskViewModel TaskViewModel
        {
            get => _taskViewModel;
            set
            {
                _taskViewModel = value;
                OnPropertyChanged();
            }
        }
        /*
        public ScheduleTask ScheduleTask
        {
            get => _scheduleTask;
            set
            {
                _scheduleTask = value;
                OnPropertyChanged();
            }
        }
        */
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

        public ICommand AddTaskCommand => _addTaskCommand ?? (_addTaskCommand = new ActionCommand(param =>
        {
            TaskViewModel = new TaskViewModel(_fileProvider, null);
            TaskViewModel.Canceled += OnCancel;
            TaskViewModel.Confirmed += OnConfirm;
            ShowTaskView = true;
        }));
        public ICommand RemoveTaskCommand => _removeTaskCommand ?? (_removeTaskCommand = new ActionCommand(param => { }));
        public ICommand EditTaskCommand => _editTaskCommand ?? (_editTaskCommand = new ActionCommand(param => { }));
        public ICommand ShowTaskDetailsCommand => _showTaskDetailsCommand ?? (_showTaskDetailsCommand = new ActionCommand(param => { }));
        /*
        public ICommand ChoisePassCommand => _choisePassCommand ?? (_choisePassCommand = new ActionCommand(param => { }));
        public ICommand ConfirmCommand => _confirmCommand ?? (_confirmCommand = new ActionCommand(param =>
        {
            Tasks = _taskController.AddTask(ScheduleTask);
            ScheduleTask = new ScheduleTask();
            ShowTaskView = false;
        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new ActionCommand(param =>
        {
            ScheduleTask = new ScheduleTask();
            ShowTaskView = false;
        }));
        */
        public bool ShowTaskView
        {
            get => _showTaskView;
            set
            {
                _showTaskView = value;
                ShowTaskSchedulingView = !value;
                OnPropertyChanged();
            }
        }

        public bool ShowTaskSchedulingView
        {
            get => _showTaskSchedulingView;
            set
            {
                _showTaskSchedulingView = value; 
                OnPropertyChanged();
            }
        }

        private void OnConfirm(object sender, EventArgs e)
        {
            ShowTaskView = false;
            TaskViewModel.Canceled -= OnCancel;
            TaskViewModel.Confirmed -= OnConfirm;
            Tasks = _taskController.AddTask(TaskViewModel.ScheduleTask);
            TaskViewModel = null;
        }
        private void OnCancel(object sender, EventArgs e)
        {
            ShowTaskView = false;
            TaskViewModel.Canceled -= OnCancel;
            TaskViewModel = null;
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
