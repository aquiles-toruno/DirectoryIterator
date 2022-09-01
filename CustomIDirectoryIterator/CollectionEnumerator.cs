using CustomDirectoryIterator.Contracts;

namespace CustomDirectoryIterator
{
    internal class CollectionEnumerator
    {
        private readonly string _path;
        private readonly int? _maxParallelProcess;

        public CollectionEnumerator(string path, int? maxParallelProcess = default)
        {
            _path = path;
            _maxParallelProcess = maxParallelProcess;
        }

        public ICustomDirectoryCollection ToCustomCollection() => new CustomDirectoryCollection(_path, _maxParallelProcess);
    }
}
