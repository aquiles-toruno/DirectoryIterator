using DirectoryIterator.Contracts;

namespace DirectoryIterator
{
    public static class CustomDirectory
    {
        public static ICustomDirectoryCollection EnumerateDirectories(string path, int maxParallelProcess, out IInaccessiblePathCollection inaccessiblePaths)
        {
            var collection = new CollectionEnumerator(path, maxParallelProcess).ToCustomCollection();
            inaccessiblePaths = collection.InaccessiblePathCollection;

            return collection;
        }
    }
}