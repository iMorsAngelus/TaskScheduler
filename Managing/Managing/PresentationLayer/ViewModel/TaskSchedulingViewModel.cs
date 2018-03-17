using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Managing.PresentationLayer.ViewModel
{
    class TaskSchedulingViewModel:ViewModelBase
    {
        #region private fields

        private const int HideMessageTimeOutInSecond = 5;
        private string _statusMessage;
        private Visibility _statusMessageIsVisible = Visibility.Hidden;

        #endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskSchedulingViewModel"/> class.        
        /// </summary>
        public TaskSchedulingViewModel()
        {
            this.DisplayName = "Task scheduling";
            //Events subscribe
        }

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
