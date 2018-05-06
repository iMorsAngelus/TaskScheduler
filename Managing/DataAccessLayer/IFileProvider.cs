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
        T Load<T>(string path);
        /// <summary>
        /// Save object to binary file
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="path">Path to file</param>
        /// <param name="content">Saved object</param>
        void Save<T>(string path, T content);
    }
}
