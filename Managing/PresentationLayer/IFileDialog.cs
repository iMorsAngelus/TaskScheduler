namespace Managing.PresentationLayer
{
    /// <summary>
    /// Represents an interface for file dialog
    /// </summary>
    public interface IFileDialog
    {
        /// <summary>
        /// Open Dialog for load something.
        /// </summary>
        string LoadDialog();
        /// <summary>
        /// Open dialog for save something.
        /// </summary>
        string SaveDialog();

    }
}