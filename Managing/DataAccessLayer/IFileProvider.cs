using System.Threading.Tasks;

namespace Managing.DataAccessLayer
{
    /// <summary>
    /// Represent an interface of IFileProvider
    /// </summary>
    interface IFileProvider
    {
        /// <summary>
        /// Loaad object from binary file
        /// </summary>
        /// <param name="path">path to </param>
        /// <returns>Canceled object</returns>
        Task<T> LoadAsync<T>(string path);
        /// <summary>
        /// SaveAsync object to binary file
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="path">Path to file</param>
        /// <param name="content">Saved object</param>
        Task SaveAsync<T>(string path, T content);
    }
}
