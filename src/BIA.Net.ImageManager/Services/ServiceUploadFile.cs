namespace BIA.Net.ImageManager.Services
{
    using DTO;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Service UploadFile
    /// </summary>
    public static class ServiceUploadFile
    {
        /// <summary>
        /// Singleton
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// Saves an image on a physical disk.
        /// </summary>
        /// <param name="directoryPath">directory path</param>
        /// <param name="uploadFile">upload file</param>
        /// <param name="isOnlyOne">is Only One</param>
        public static void UploadImage(string directoryPath, FileDTO uploadFile, bool isOnlyOne = true)
        {
            if (uploadFile != null && uploadFile.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                UploadFile(directoryPath, uploadFile, isOnlyOne);
            }
        }

        /// <summary>
        /// Downloads a file that was uploaded.
        /// </summary>
        /// <param name="directoryPath">directory path</param>
        /// <param name="fileName">file name</param>
        /// <returns>a file</returns>
        public static FileDTO GetUploadedFile(string directoryPath, string fileName = null)
        {
            FileDTO fileDTO = null;

            if (!string.IsNullOrWhiteSpace(directoryPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);

                if (dirInfo != null && dirInfo.Exists)
                {
                    FileInfo fileInfo = null;

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        fileInfo = dirInfo.GetFiles(fileName).FirstOrDefault();
                    }
                    else
                    {
                        fileInfo = dirInfo.GetFiles().FirstOrDefault();
                    }

                    if (fileInfo != null && fileInfo.Exists)
                    {
                        fileDTO = new FileDTO();

                        fileDTO.Binary = File.ReadAllBytes(fileInfo.FullName);
                        fileDTO.Name = fileInfo.Name;
                    }
                }
            }

            return fileDTO;
        }

        /// <summary>
        /// Delete an image.
        /// </summary>
        /// <param name="directoryPath">directory path</param>
        /// <param name="fileName">file name</param>
        public static void Delete(string directoryPath, string fileName = null)
        {
            if (!string.IsNullOrWhiteSpace(directoryPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);

                if (dirInfo != null && dirInfo.Exists)
                {
                    lock (SyncRoot)
                    {
                        Delete(dirInfo, fileName);
                    }
                }
            }
        }

        /// <summary>
        /// Delete an image.
        /// </summary>
        /// <param name="dirInfo">directory path</param>
        /// <param name="fileName">file name</param>
        private static void Delete(DirectoryInfo dirInfo, string fileName = null)
        {
            List<FileInfo> fileInfos = null;

            if (!string.IsNullOrEmpty(fileName))
            {
                fileInfos = dirInfo.GetFiles(fileName).ToList();
            }
            else
            {
                fileInfos = dirInfo.GetFiles().ToList();
            }

            // If it exists, the files with the same name are deleted.
            if (fileInfos != null && fileInfos.Any())
            {
                foreach (FileInfo fileInfo in fileInfos)
                {
                    if (fileInfo != null && fileInfo.Exists)
                    {
                        File.Delete(fileInfo.FullName);
                    }
                }
            }
        }

        /// <summary>
        /// Saves an image on a physical disk.
        /// </summary>
        /// <param name="directoryPath">directory path</param>
        /// <param name="uploadFile">upload file</param>
        /// <param name="isOnlyOne">if only one image for this entity</param>
        private static void UploadFile(string directoryPath, FileDTO uploadFile, bool isOnlyOne)
        {
            if (!string.IsNullOrWhiteSpace(directoryPath) && uploadFile != null && uploadFile.Binary != null && !string.IsNullOrWhiteSpace(uploadFile.Name))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);

                lock (SyncRoot)
                {
                    if (dirInfo == null || !dirInfo.Exists)
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    dirInfo = new DirectoryInfo(directoryPath);

                    if (dirInfo != null && dirInfo.Exists)
                    {
                        string fileName = Path.GetFileName(uploadFile.Name);

                        if (isOnlyOne)
                        {
                            Delete(dirInfo);
                        }
                        else
                        {
                            Delete(dirInfo, fileName);
                        }

                        // The file is created on the path specified
                        string pathFile = string.Format("{0}\\{1}", dirInfo.FullName, fileName);
                        using (FileStream fileStream = File.Create(pathFile))
                        {
                            fileStream.Write(uploadFile.Binary, 0, uploadFile.Binary.Length);
                        }
                    }
                }
            }
        }
    }
}
