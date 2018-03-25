using System;

namespace Managing.Common.Extension
{
    /// <summary>
    /// Provides extensions for <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Throws ArgumentNullException if obj parameter is null.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="obj">Object to be checked.</param>
        /// <param name="argumentName">Argument name to be specified in the exception.</param>
        /// <returns>The target object (always not null).</returns>
        public static T ThrowIfNull<T>(this T obj, string argumentName)
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException(argumentName);
            }

            return obj;
        }
    }
}
