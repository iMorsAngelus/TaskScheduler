using System;
using System.Windows.Input;
using Managing.BusinessLogicLayer;
using Managing.Common.Extension;
using Managing.DataAccessLayer;
using Managing.PresentationLayer.Command;

namespace Managing.PresentationLayer.ViewModel
{
    /// <summary>
    /// Instance of task view model
    /// </summary>
    class TaskViewModel : ViewModelBase
    {
        #region private fields

        private IFileProvider _fileProvider;
        private readonly IFileDialog _fileDialog;
        private ActionCommand _confirmCommand;
        private ActionCommand _cancelCommand;
        private ActionCommand _choisePassCommand;
        private ScheduleTask _scheduleTask;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskViewModel"/> class.
        /// </summary>
        /// <param name="fileProvider">File provider.</param>
        /// <param name="fileDialog">File dialog.</param>
        /// <param name="scheduleTask">Processed task.</param>
        public TaskViewModel(IFileProvider fileProvider, IFileDialog fileDialog, ScheduleTask scheduleTask)
        {
            fileProvider.ThrowIfNull(nameof(fileProvider));
            fileDialog.ThrowIfNull(nameof(fileDialog));

            _fileProvider = fileProvider;
            _fileDialog = fileDialog;
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
        /// <summary>
        /// Processed taks.
        /// </summary>
        public ScheduleTask ScheduleTask
        {
            get => _scheduleTask;
            set
            {
                _scheduleTask = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Choise task folder command.
        /// </summary>
        public ICommand ChoisePassCommand => _choisePassCommand ?? (_choisePassCommand = new ActionCommand(param =>
        {
            ScheduleTask.TaskLocation = _fileDialog.LoadDialog();
        }));
        /// <summary>
        /// Confirm command.
        /// </summary>
        public ICommand ConfirmCommand => _confirmCommand ?? (_confirmCommand = new ActionCommand(param => { OnConfirm(); }));
        /// <summary>
        /// Cancel command.
        /// </summary>
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