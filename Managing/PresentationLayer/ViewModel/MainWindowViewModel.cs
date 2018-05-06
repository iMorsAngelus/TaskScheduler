using System.Collections.Generic;

namespace Managing.PresentationLayer.ViewModel
{
    /// <inheritdoc />
    /// <summary>
    /// Main view model, who contains all other.
    /// </summary>
    class MainWindowViewModel : ViewModelBase
    {
        #region private fields
        private List<ViewModelBase> _viewModelsList;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.        
        /// </summary>
        /// <param name="viewModelList"></param>
        public MainWindowViewModel(List<ViewModelBase> viewModelList)
        {
            ViewModelsList = viewModelList;
        }
        
        /// <summary>
        /// ViewModelsList of applications.
        /// </summary>
        public List<ViewModelBase> ViewModelsList
        {
            get => _viewModelsList;
            set
            {
                _viewModelsList = value;
                OnPropertyChanged();
            }
        }
    }
}