using System;
using Managing.PresentationLayer;

namespace Managing.BusinessLogicLayer
{
    public class ScheduleTask : NotifyPropertyChanged
    {
        private string _taskName;
        private string _taskArgs;
        private string _taskLocation;
        private TimeSpan _starTime;
        private DateTime _startDate;
        private bool _isRepeatable;
        private int _repeatableIntervalInDays;
        private DateTime? _lastRunDate;

        public string TaskName
        {
            get => _taskName;
            set
            {
                _taskName = value;
                OnPropertyChanged();
            }
        }

        public string TaskArgs
        {
            get => _taskArgs;
            set
            {
                _taskArgs = value;
                OnPropertyChanged();
            }
        }

        public string TaskLocation
        {
            get => _taskLocation;
            set
            {
                _taskLocation = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan StarTime
        {
            get => _starTime;
            set
            {
                _starTime = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        public bool IsRepeatable
        {
            get => _isRepeatable;
            set
            {
                _isRepeatable = value;
                OnPropertyChanged();
            }
        }

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

        public DateTime? LastRunDate
        {
            get => _lastRunDate;
            set
            {
                _lastRunDate = value;
                identifyStartTaskDate();
                OnPropertyChanged();
            }
        }

        private void identifyStartTaskDate()
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