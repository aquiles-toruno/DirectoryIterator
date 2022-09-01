using CustomDirectoryIterator.Contracts;

namespace CustomDirectoryIterator
{
    public static class CustomDirectory
    {
        public static ICustomDirectoryCollection EnumerateDirectories(string path, int? maxParallelProcess = default)
        {
            var collection = new CollectionEnumerator(path, maxParallelProcess).ToCustomCollection();

            return collection;
        }

        public static ICustomDirectoryCollection EnumerateDirectories(string path, out IInaccessiblePathCollection inaccessiblePaths, int? maxParallelProcess = default)
        {
            var collection = new CollectionEnumerator(path, maxParallelProcess).ToCustomCollection();
            inaccessiblePaths = collection.InaccessiblePathCollection;

            return collection;
        }
    }
}