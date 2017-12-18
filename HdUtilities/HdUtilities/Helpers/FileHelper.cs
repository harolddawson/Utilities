using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HdUtilities.Helpers
{
    public class FileHelper
    {
        private static readonly object dictionaryLock = new object();
        private static readonly Dictionary<string, object> fileWriteLocks = new Dictionary<string, object>();

        public List<FileInfo> GetFileInfoList(string directory, bool includeSubDirectories)
        {
            if (!Directory.Exists(directory))
            {
                throw new FileNotFoundException("Unable to find directory: " + directory);
            }
            var fileInfo = Directory.GetFiles(directory).Select(f => new FileInfo(f)).ToList();

            if (includeSubDirectories)
            {
                foreach (var d in Directory.GetDirectories(directory))
                {
                    fileInfo.AddRange(GetFileInfoList(d, true));
                }
            }

            return fileInfo.OrderBy(f => f.Directory?.FullName).ThenBy(f => f.Name).ToList();
        }

        public List<string> GetFilePathList(string directory, bool includeSubDirectories)
        {
            if (!Directory.Exists(directory))
            {
                throw new FileNotFoundException("Unable to find directory: " + directory);
            }
            var fileInfo = Directory.GetFiles(directory).OrderBy(f => f).ToList();

            if (includeSubDirectories)
            {
                foreach (var d in Directory.GetDirectories(directory).OrderBy(d => d))
                {
                    fileInfo.AddRange(GetFilePathList(d, true));
                }
            }

            return fileInfo.ToList();
        }

        public void CreateDirectoryIfNotExists(string directory)
        {
            if (Directory.Exists(directory))
            {
                return;
            }
            Directory.CreateDirectory(directory);
        }

        public void CleanupZeroByteFiles(string directory)
        {
            var subDirs = Directory.GetDirectories(directory);
            foreach (var dir in subDirs)
            {
                CleanupZeroByteFiles(dir);
            }

            var zeroByteFiles =
                Directory.GetFiles(directory).Select(f => new FileInfo(f)).Where(fi => fi.Exists && fi.Length == 0);

            foreach (var f in zeroByteFiles)
            {
                f.Delete();
            }
        }

        public void CopyFile(FileInfo file, string trnsfrpth)
        {
            Console.WriteLine($"Copying {file} to {trnsfrpth}");
            if (!File.Exists(trnsfrpth))
            {
                file.CopyTo(trnsfrpth, false);
            }
        }

        private object GetFileWriteLock(string fileName)
        {
            lock (dictionaryLock)
            {
                object l;

                if (fileWriteLocks.TryGetValue(fileName, out l))
                {
                    return l;
                }

                l = new object();
                fileWriteLocks.Add(fileName, l);
                return l;
            }
        }

        public void AppendToFile(string filePath, string textToWrite)
        {
            var lockObj = GetFileWriteLock(filePath);

            lock (lockObj)
            {
                File.AppendAllLines(filePath, new[] {textToWrite});
            }
        }

        public void WriteToFile(string filePath, string textToWrite)
        {
            var lockObj = GetFileWriteLock(filePath);

            lock (lockObj)
            {
                File.WriteAllLines(filePath, new[] { textToWrite });
            }
        }

        public void AppendAllLinesToFile(string filePath, IEnumerable<string> textToWrite)
        {
            var lockObj = GetFileWriteLock(filePath);

            lock (lockObj)
            {
                File.AppendAllLines(filePath, textToWrite);
            }
        }

        public void WriteAllLinesToFile(string filePath, IEnumerable<string> textToWrite)
        {
            var lockObj = GetFileWriteLock(filePath);

            lock (lockObj)
            {
                File.WriteAllLines(filePath, textToWrite);
            }
        }
    }
}