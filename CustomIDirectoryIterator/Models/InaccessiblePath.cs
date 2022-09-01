using System;

namespace CustomDirectoryIterator.Models
{
    public class InaccessiblePath
    {
        public InaccessiblePath(string path, Exception exception)
        {
            Path = path;
            Exception = exception;
        }

        public string Path { get; private set; }
        public Exception Exception { get; private set; }
    }
}
