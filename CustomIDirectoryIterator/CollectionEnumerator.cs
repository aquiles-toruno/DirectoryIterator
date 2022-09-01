using CustomDirectoryIterator.Contracts;

namespace CustomDirectoryIterator
{
    internal class CollectionEnumerator
    {
        private readonly string _path;
        private readonly int? _maxParallelProcess;
        private readonly bool _recursive;

        public CollectionEnumerator(string path, bool recursive = false, int? maxParallelProcess = default)
        {
            _path = path;
            _maxParallelProcess = maxParallelProcess;
            _recursive = recursive;
        }

        public ICustomDirectoryCollection ToCustomCollection() => new CustomDirectoryCollection(_path, _recursive, _maxParallelProcess);
    }
}
