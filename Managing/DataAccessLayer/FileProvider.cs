﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Managing.Common.Extension;

namespace Managing.DataAccessLayer
{
    /// <summary>
    /// Represent an FileProvider class
    /// </summary>
    class FileProvider : IFileProvider
    {
        /// <inheritdoc />
        public Task<T> LoadAsync<T>(string path)
        {
            return Task.Factory.StartNew(() =>
            {
                if (!File.Exists(path))
                    throw new ArgumentException("File doesn't exist.");

                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (var binaryReader = new BinaryReader(fileStream, Encoding.ASCII))
                    {
                        var buffer = new List<byte>();
                        while (binaryReader.PeekChar() != -1)
                        {
                            buffer.Add(binaryReader.ReadByte());
                        }

                        var loadedObject = Unboxing(buffer.ToArray());

                        loadedObject.ThrowIfNull(nameof(loadedObject));

                        return (T) loadedObject;
                    }
                }
            });
        }
        /// <inheritdoc />
        public Task SaveAsync<T>(string path, T content)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (var binaryWriter = new BinaryWriter(fileStream))
                    {
                        var savedObject = Boxing(content);
                        binaryWriter.Write(savedObject, 0, savedObject.Length);
                    }
                }
            });
        }

        private byte[] Boxing(object item)
        {
            var bf = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, item);
                return ms.ToArray();
            }
        }
        private object Unboxing(byte[] item)
        {
            var bf = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                ms.Write(item, 0, item.Length);
                ms.Position = 0;
                return bf.Deserialize(ms);
            }
        }
    }
}