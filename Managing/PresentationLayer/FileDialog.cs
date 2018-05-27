using System.Windows;
using Microsoft.Win32;

namespace Managing.PresentationLayer
{
    /// <summary>
    /// Instance to provide file dialog
    /// </summary>
    public class FileDialog : IFileDialog
    {
        private const string DefaultPath = "../../default.bin";
        private readonly SaveFileDialog _saveFileDialog;
        private readonly OpenFileDialog _openFileDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDialog"/> class.
        /// </summary>
        public FileDialog()
        {
            _saveFileDialog = new SaveFileDialog { FileName = string.Empty, DefaultExt = ".bin", Filter = "Binary files (*.bin)|*.bin"};
            _openFileDialog = new OpenFileDialog { FileName = string.Empty, DefaultExt = ".bin", Filter = "Binary files (*.bin)|*.bin" };
        }

        /// <inheritdoc />
        public string LoadDialog()
        {
            return (_openFileDialog.ShowDialog()?? false)? _openFileDialog.FileName : UseDefaultPath("load from");
        }


        /// <inheritdoc />
        public string SaveDialog()
        {
            _saveFileDialog.OverwritePrompt = true;
            return (_saveFileDialog.ShowDialog() ?? false) ? _saveFileDialog.FileName : UseDefaultPath("save by");
        }

        private string UseDefaultPath(string msg)
        {
            return MessageBox.Show($"Do you want {msg} default path: {DefaultPath}",
                       "Operation canceled",
                       MessageBoxButton.YesNo,
                       MessageBoxImage.Question) == MessageBoxResult.Yes
                ? DefaultPath
                : null;
        }
    }
}