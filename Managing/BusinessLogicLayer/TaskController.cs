using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Managing.Common.Extension;
using Managing.DataAccessLayer;

namespace Managing.BusinessLogicLayer
{
    class TaskController : ITaskController, IDisposable
    {
        private const string DefaultPathToTasks = "../../source/tasks.bin";
        private const int WaitTimeOutInSeconds = 1;
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

        public TaskController(IFileProvider fileProvider)
            : this(fileProvider, LogManager.GetLogger(typeof(App)))
        {}

        private Task ProcessingTasks()
        {
            return Task.Factory.StartNew(async () =>
            {
                while (!_isDisposed)
                {
                    var currentTime = DateTime.Now;
                    _tasks?.ForEach((item) =>
                    {
                        if (item.StarTime >= currentTime.TimeOfDay &&
                                (item.LastRunDate == null &&
                                    !item.IsRepeatable &&
                                    item.StartDate.Date >= currentTime.Date ||
                                 item.StartDate.Date >= currentTime.Date))
                        {
                            StartTask(item);
                        }
                    });
                    await Task.Delay(TimeSpan.FromSeconds(WaitTimeOutInSeconds));
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
        }

        private void StartTask(ScheduleTask scheduleTask)
        {
            var fileName = $"{scheduleTask.TaskLocation}{scheduleTask.TaskName}";
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

        #region Implementation members of ITaskController

        public async Task<ObservableCollection<ScheduleTask>> LoadTasks(string path)
        {
            _tasks = _fileProvider.Load<List<ScheduleTask>>(DefaultPathToTasks);

            await ProcessingTasks().ConfigureAwait(false);
            return new ObservableCollection<ScheduleTask>(_tasks);
        }

        public ObservableCollection<ScheduleTask> AddTask(ScheduleTask scheduleTask)
        {
            _tasks?.Add(scheduleTask);
            SortCollection();
            return new ObservableCollection<ScheduleTask>(_tasks);
        }

        public ObservableCollection<ScheduleTask> RemoveTask(ScheduleTask scheduleTask)
        {
            _tasks?.Remove(scheduleTask);
            SortCollection();
            return new ObservableCollection<ScheduleTask>(_tasks);
        }

        public ObservableCollection<ScheduleTask> EditTask(int index, ScheduleTask scheduleTask)
        {
            _tasks[index] = scheduleTask;
            SortCollection();
            return new ObservableCollection<ScheduleTask>(_tasks);
        }
        
        #endregion

        public void Dispose()
        {
            _isDisposed = true;
        }
    }
}
