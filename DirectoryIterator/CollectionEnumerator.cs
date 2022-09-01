using DirectoryIterator.Contracts;

namespace DirectoryIterator
{
    internal class CollectionEnumerator
    {
        private readonly string _path;
        private readonly int _maxParallelProcess;

        public CollectionEnumerator(string path, int maxParallelProcess)
        {
            _path = path;
            _maxParallelProcess = maxParallelProcess;
        }

        public ICustomDirectoryCollection ToCustomCollection() => new CustomDirectoryCollection(_path, _maxParallelProcess);
    }
}
