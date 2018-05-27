using System;
using Managing.PresentationLayer;

namespace Managing.BusinessLogicLayer
{
    /// <summary>
    /// Represent data model of task.
    /// </summary>
    [Serializable]
    public class ScheduleTask : NotifyPropertyChanged
    {
        private string _taskName;
        private string _taskArgs;
        private string _taskLocation;
        private TimeSpan _starTime;
        private DateTime _startDate = DateTime.Today.Date;
        private bool _isRepeatable = false;
        private int _repeatableIntervalInDays;
        private DateTime? _lastRunDate;

        /// <summary>
        /// Task name.
        /// </summary>
        public string TaskName
        {
            get => _taskName;
            set
            {
                _taskName = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Tasks arguments.
        /// </summary>
        public string TaskArgs
        {
            get => _taskArgs;
            set
            {
                _taskArgs = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Location of task.
        /// </summary>
        public string TaskLocation
        {
            get => _taskLocation;
            set
            {
                _taskLocation = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Start time.
        /// </summary>
        public TimeSpan StarTime
        {
            get => _starTime;
            set
            {
                _starTime = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Start date.
        /// </summary>
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value.Date;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Indicate necessity repeatable
        /// </summary>
        public bool IsRepeatable
        {
            get => _isRepeatable;
            set
            {
                _isRepeatable = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Repeatable interval in days
        /// </summary>
        public int RepeatableIntervalInDays
        {
            get => _repeatableIntervalInDays;
            set
            {
                _repeatableIntervalInDays
                    = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Indicate last run date.
        /// </summary>
        public DateTime? LastRunDate
        {
            get => _lastRunDate;
            set
            {
                _lastRunDate = value;
                IdentifyStartTaskDate();
                OnPropertyChanged();
            }
        }

        private void IdentifyStartTaskDate()
        {
            if (IsRepeatable && LastRunDate.HasValue)
            {
                var amountDayFromLastRun = DateTime.Now.Subtract(LastRunDate.Value);
                StartDate = DateTime.Now.Add(amountDayFromLastRun);
            }
            else if (IsRepeatable)
            {
                var amountInetvalDay = TimeSpan.FromDays(RepeatableIntervalInDays);
                StartDate = DateTime.Now.Add(amountInetvalDay);
            }
            else if (LastRunDate == null && StartDate < DateTime.Now)
            {
                var amountDayToRun = StartDate.Subtract(DateTime.Now);
                StartDate = DateTime.Now.Add(amountDayToRun);
            }
        }
    }
}