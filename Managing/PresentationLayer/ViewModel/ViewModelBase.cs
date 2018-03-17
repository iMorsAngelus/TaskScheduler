using System;
using Managing.PresentationLayer.ViewModel;

namespace Managing.PresentationLayer.ViewModel
{
    /// <summary>
    /// Base class for all view models.
    /// </summary>
    public class ViewModelBase : NotifyPropertyChanged
    {
        /// <summary>
        /// Name of view model.
        /// </summary>
        public String DisplayName { get; set; }
    }
}