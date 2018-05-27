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
    /// <summary>
    /// Instence of task scheduling view model
    /// </summary>
    class TaskSchedulingViewModel : ViewModelBase
    {
        #region private fields

        private const int HideMessageTimeOutInSecond = 5;
        private readonly ITaskController _taskController;
        private readonly IFileProvider _fileProvider;
        private readonly IFileDialog _fileDialog;
        private string _statusMessage;
        private Visibility _statusMessageIsVisible = Visibility.Hidden;

        private ActionCommand _addTaskCommand;
        private ActionCommand _removeTaskCommand;
        private ActionCommand _editTaskCommand;
        private ActionCommand _loadTasksCommand;
        private ActionCommand _saveTasksCommand;

        private TaskViewModel _taskViewModel;
        private ObservableCollection<ScheduleTask> _tasks;
        private bool _showTaskView = false;
        private bool _showTaskSchedulingView = true;
        private int _selectedIndex;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskSchedulingViewModel"/> class.        
        /// </summary>
        public TaskSchedulingViewModel(ITaskController taskController, IFileProvider fileProvider,
            IFileDialog fileDialog)
        {
            taskController.ThrowIfNull(nameof(taskController));
            fileProvider.ThrowIfNull(nameof(fileProvider));

            DisplayName = "Scheduling tasks";
            _taskController = taskController;
            _fileProvider = fileProvider;
            _fileDialog = fileDialog;
            //Events subscribe
        }

        /// <summary>
        /// Represent a collection of tasks.
        /// </summary>
        public ObservableCollection<ScheduleTask> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                _taskController.ProcessingTasks();
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Represent current task view model.
        /// </summary>
        public TaskViewModel TaskViewModel
        {
            get => _taskViewModel;
            set
            {
                _taskViewModel = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Status message.
        /// </summary>
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
        /// <summary>
        /// Selected task index. 
        /// Responsible for processing task like edit \ delete.
        /// </summary>
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Indicate visiblity of status message
        /// </summary>
        public Visibility StatusMessageIsVisible
        {
            get => _statusMessageIsVisible;
            set
            {
                _statusMessageIsVisible = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Command that add task to collection.
        /// </summary>
        public ICommand AddTaskCommand => _addTaskCommand ?? (_addTaskCommand = new ActionCommand(param =>
        {
            TaskViewModel = new TaskViewModel(_fileProvider, _fileDialog, null);
            TaskViewModel.Canceled += OnCancel;
            TaskViewModel.Confirmed += OnConfirm;
            ShowTaskView = true;
        }));
        /// <summary>
        /// Command that responding for load tasks from file.
        /// </summary>
        public ICommand LoadTasksCommand => _loadTasksCommand ?? (_loadTasksCommand = new ActionCommand(async param =>
        {
            Tasks = await _taskController.LoadTasks(_fileDialog.LoadDialog());
            StatusMessage = "Tasks loading is succesfull";
        }));
        /// <summary>
        /// Command that responding for save tasks to file.
        /// </summary>
        public ICommand SaveTasksCommand => _saveTasksCommand ?? (_saveTasksCommand = new ActionCommand(async param =>
        {
            await _fileProvider.SaveAsync(_fileDialog.SaveDialog(), Tasks);
        }));
        /// <summary>
        /// Command that remove task from collection.
        /// </summary>
        public ICommand RemoveTaskCommand => _removeTaskCommand ?? (_removeTaskCommand = new ActionCommand(param =>
        {
            Tasks = _taskController.RemoveTask(Tasks[SelectedIndex]);
        }));
        /// <summary>
        /// Command that respose for task editing.
        /// </summary>
        public ICommand EditTaskCommand => _editTaskCommand ?? (_editTaskCommand = new ActionCommand(param =>
        {
            TaskViewModel = new TaskViewModel(_fileProvider, _fileDialog, Tasks[SelectedIndex]);
            TaskViewModel.Canceled += OnCancel;
            TaskViewModel.Confirmed += OnConfirmEdit;
            ShowTaskView = true;
        }));
        /// <summary>
        /// Indicate that it is time to show task view.
        /// </summary>
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
        /// <summary>
        /// Indicate visiblity of managing task view.
        /// </summary>
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

        private void OnConfirmEdit(object sender, EventArgs e)
        {
            ShowTaskView = false;
            TaskViewModel.Canceled -= OnCancel;
            TaskViewModel.Confirmed -= OnConfirm;
            Tasks = _taskController.EditTask(SelectedIndex, TaskViewModel.ScheduleTask);
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
            await Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(HideMessageTimeOutInSecond));
                    StatusMessageIsVisible = Visibility.Hidden;
                }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default)
                .ConfigureAwait(false);
        }
    }
}