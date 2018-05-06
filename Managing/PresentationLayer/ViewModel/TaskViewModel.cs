using System;
using System.Windows.Input;
using Managing.BusinessLogicLayer;
using Managing.Common.Extension;
using Managing.DataAccessLayer;
using Managing.PresentationLayer.Command;

namespace Managing.PresentationLayer.ViewModel
{
    class TaskViewModel : ViewModelBase
    {
        #region private fields
        private IFileProvider _fileProvider;
        private ActionCommand _confirmCommand;
        private ActionCommand _cancelCommand;
        private ActionCommand _choisePassCommand;
        private ScheduleTask _scheduleTask;

        #endregion

        public TaskViewModel(IFileProvider fileProvider, ScheduleTask scheduleTask)
        {
            fileProvider.ThrowIfNull(nameof(fileProvider));

            _fileProvider = fileProvider;
            DisplayName = "Add scheduling task";
            _scheduleTask = scheduleTask?? new ScheduleTask();
        }
       
        /// <summary>
        /// Event that fires when user is click cancel
        /// </summary>
        public event EventHandler Canceled;
        /// <summary>
        /// Event that fires when user is click confirm
        /// </summary>
        public event EventHandler Confirmed;
        public ScheduleTask ScheduleTask
        {
            get => _scheduleTask;
            set
            {
                _scheduleTask = value;
                OnPropertyChanged();
            }
        }
        public ICommand ChoisePassCommand => _choisePassCommand ?? (_choisePassCommand = new ActionCommand(param => { }));
        public ICommand ConfirmCommand => _confirmCommand ?? (_confirmCommand = new ActionCommand(param => { OnConfirm(); }));
        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new ActionCommand(param => { OnCancel(); }));

        private void OnConfirm()
        {
            Confirmed?.Invoke(this, EventArgs.Empty);
        }
        private void OnCancel()
        {
            Canceled?.Invoke(this, EventArgs.Empty);
        }
    }
}