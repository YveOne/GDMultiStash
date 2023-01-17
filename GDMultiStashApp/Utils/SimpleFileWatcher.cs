using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Utils
{

    public class SimpleFileWatcher
    {

        public class FileChangedEventArgs : EventArgs
        {
            public string FilePath { get; private set; }
            public string FileName { get; private set; }
            public FileChangedEventArgs(string filePath)
            {
                FilePath = filePath;
                FileName = Path.GetFileName(filePath);
            }
        }

        public EventHandler<FileChangedEventArgs> FileChanged;

        private class FileAttibutes
        {
            public string FilePath;
            public bool SkipNext;
            public bool FileExist;
            public DateTime LastWriteTime;
        }

        private readonly string _directory;
        private readonly Dictionary<string, FileAttibutes> _files;
        private readonly List<string> _filesToAdd;

        public SimpleFileWatcher(string directory)
        {
            _directory = directory;
            _files = new Dictionary<string, FileAttibutes>();
            _filesToAdd = new List<string>();
            ThreadPool.QueueUserWorkItem(o => ThreadSub());
        }

        public void AddFile(string fileName)
        {
            lock (_filesToAdd)
            {
                _filesToAdd.Add(fileName);
            }
        }

        public void AddFiles(IEnumerable<string> files)
        {
            foreach (var f in files)
                AddFile(f);
        }

        public void AddFiles(IList<string> files)
        {
            foreach (var f in files)
                AddFile(f);
        }

        public void SkipNextFile(string fileName)
        {
            if (_files.ContainsKey(fileName))
            {
                _files[fileName].SkipNext = true;
            }
        }

        private void ThreadSub()
        {
            try
            {
                while (true)
                {
                    lock (_filesToAdd)
                    {
                        foreach (string fileName in _filesToAdd)
                        {
                            var filePath = Path.Combine(_directory, fileName);
                            var exists = File.Exists(filePath);
                            lock (_filesToAdd)
                            {
                                _files.Add(fileName, new FileAttibutes()
                                {
                                    FilePath = filePath,
                                    SkipNext = false,
                                    FileExist = exists,
                                    LastWriteTime = exists
                                        ? File.GetLastWriteTime(filePath)
                                        : DateTime.MinValue
                                });
                            }
                        }
                        _filesToAdd.Clear();
                    }

                    foreach (var f in _files.Values)
                    {
                        if (File.Exists(f.FilePath))
                        {
                            var lastWrite = File.GetLastWriteTime(f.FilePath);
                            if (f.LastWriteTime != lastWrite)
                            {
                                f.LastWriteTime = lastWrite;
                                if (f.SkipNext)
                                {
                                    f.SkipNext = false;
                                    continue;
                                }
                                FileChanged?.Invoke(this, new FileChangedEventArgs(f.FilePath));
                            }
                        }
                        else
                        {

                        }
                    }
                    Thread.Sleep(50);
                }
            }
            catch (ThreadAbortException)
            {
            }
        }
    }

}
