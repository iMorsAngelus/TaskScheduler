using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Managing.Common.Extension;
using Managing.DataAccessLayer;

namespace Managing.BusinessLogicLayer
{
    /// <summary>
    /// Instence of a task controller
    /// </summary>
    class TaskController : ITaskController, IDisposable
    {
        private const int WaitTimeOutInSeconds = 60;
        private bool _isDisposed = false;
        private List<ScheduleTask> _tasks;
        private readonly ILog _log;
        private readonly IFileProvider _fileProvider;

        private TaskController(IFileProvider fileProvider, ILog log)
        {
            fileProvider.ThrowIfNull(nameof(fileProvider));

            _fileProvider = fileProvider;
            _log = log;

            _tasks = new List<ScheduleTask>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskController"/> class.
        /// </summary>
        /// <param name="fileProvider">File provider.</param>
        public TaskController(IFileProvider fileProvider)
            : this(fileProvider, LogManager.GetLogger(typeof(App)))
        {}

        #region Implementation members of ITaskController

        /// <inheritdoc />
        public async Task<ObservableCollection<ScheduleTask>> LoadTasks(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            try
            {
                var loadedCollection = await _fileProvider.LoadAsync<ObservableCollection<ScheduleTask>>(path);
                _tasks = new List<ScheduleTask>(loadedCollection);
                return new ObservableCollection<ScheduleTask>(_tasks);
            }
            catch (ArgumentException exception)
            {
                _log.Error(exception.Message);
            }

            return null;
        }
        /// <inheritdoc />
        public ObservableCollection<ScheduleTask> AddTask(ScheduleTask scheduleTask)
        {
            _tasks?.Add(scheduleTask);
            SortCollection();
            return new ObservableCollection<ScheduleTask>(_tasks);
        }
        /// <inheritdoc />
        public ObservableCollection<ScheduleTask> RemoveTask(ScheduleTask scheduleTask)
        {
            _tasks?.Remove(scheduleTask);
            SortCollection();
            return new ObservableCollection<ScheduleTask>(_tasks);
        }
        /// <inheritdoc />
        public ObservableCollection<ScheduleTask> EditTask(int index, ScheduleTask scheduleTask)
        {
            _tasks[index] = scheduleTask;
            SortCollection();
            return new ObservableCollection<ScheduleTask>(_tasks);
        }
        /// <inheritdoc />
        public Task ProcessingTasks()
        {
            return Task.Factory.StartNew(async () =>
            {
                while (!_isDisposed)
                {
                    var currentDate = DateTime.Now;
                    var currentTime = new TimeSpan(0, currentDate.TimeOfDay.Hours, currentDate.TimeOfDay.Minutes, 0);
                    _tasks?.ForEach((item) =>
                    {
                        if (item.StarTime.Equals(currentTime) &&
                            (item.LastRunDate == null &&
                             !item.IsRepeatable &&
                             item.StartDate.Date == currentDate.Date ||
                             item.IsRepeatable &&
                             item.StartDate.Date == currentDate.Date))
                        {
                            StartTask(item);
                        }
                    });
                    await Task.Delay(TimeSpan.FromSeconds(WaitTimeOutInSeconds));
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
        }

        #endregion

        public void Dispose()
        {
            _isDisposed = true;
        }

        private void StartTask(ScheduleTask scheduleTask)
        {
            var fileName = $"{scheduleTask.TaskLocation}\\{scheduleTask.TaskName}";
            Process.Start(fileName, scheduleTask.TaskArgs);
            scheduleTask.LastRunDate = DateTime.Now.Date;
        }

        private void SortCollection()
        {
            if (_tasks == null || _tasks.Count <= 1)
            {
                return;
            }

            _tasks.Sort((x, y) => x.StarTime.CompareTo(y.StarTime));
        }
    }
}
