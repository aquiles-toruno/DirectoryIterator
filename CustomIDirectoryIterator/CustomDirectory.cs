using CustomDirectoryIterator.Contracts;

namespace CustomDirectoryIterator
{
    public static class CustomDirectory
    {
        public static ICustomDirectoryCollection EnumerateDirectories(string path, bool recursive = false, int? maxParallelProcess = default)
        {
            var collection = new CollectionEnumerator(path, recursive, maxParallelProcess).ToCustomCollection();

            return collection;
        }

        public static ICustomDirectoryCollection EnumerateDirectories(string path, out IInaccessiblePathCollection inaccessiblePaths, bool recursive = false, int? maxParallelProcess = default)
        {
            var collection = new CollectionEnumerator(path, recursive, maxParallelProcess).ToCustomCollection();
            inaccessiblePaths = collection.InaccessiblePathCollection;

            return collection;
        }
    }
}