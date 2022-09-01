using DirectoryIterator.Contracts;
using DirectoryIterator.Models;
using System.Collections;

namespace DirectoryIterator
{
    internal class CustomDirectoryCollection : ICustomDirectoryCollection
    {
        private string _path;
        private readonly string _rootPath;
        private readonly int _maxParallelProcess;
        private readonly IInaccessiblePathCollection _inaccessiblePaths;

        public IInaccessiblePathCollection InaccessiblePathCollection => _inaccessiblePaths;

        public CustomDirectoryCollection(string path, int maxParallelProcess)
        {
            _path = _rootPath = path;
            _maxParallelProcess = maxParallelProcess;
            _inaccessiblePaths = new InaccessiblePathCollection();
        }

        public virtual IEnumerator<string> GetEnumerator()
        {
            foreach (var item in MyEnumerator())
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<string> MyEnumerator()
        {
            IEnumerable<string> directories = Array.Empty<string>();

            try
            {
                directories = Directory.GetDirectories(_path, "*", SearchOption.TopDirectoryOnly);
            }
            catch (Exception ex)
            {
                _inaccessiblePaths.Add(new InaccessiblePath(_path, ex));
            }

            var awaiter = IterateDirectoriesAsync(directories).GetAwaiter();
            var childrenDirectories = awaiter.GetResult();

            foreach (var directory in childrenDirectories)
            {
                _path = directory;

                foreach (var item in MyEnumerator())
                {
                    yield return item;
                }

                yield return directory;
            }
        }

        private async Task<IEnumerable<string>> IterateDirectoriesAsync(IEnumerable<string> directories)
        {
            IEnumerable<string> directoriesFound = Array.Empty<string>();
            IEnumerable<string>[] tasksResult = Array.Empty<IEnumerable<string>>();
            List<Task<IEnumerable<string>>> _tasks = new List<Task<IEnumerable<string>>>();

            int Predicate() => _tasks.Where(t => t.Status == TaskStatus.Running).Count();

            foreach (var directory in directories)
            {
                if (_maxParallelProcess > Predicate())
                    _tasks.Add(GetDirectoriesFromPath(directory));
                else
                    tasksResult = await Task.WhenAll(_tasks);
            }

            tasksResult = await Task.WhenAll(_tasks);

            tasksResult.ToList().ForEach(tr =>
            {
                directoriesFound = directoriesFound.Concat(tr);
            });

            return directoriesFound;
        }

        private Task<IEnumerable<string>> GetDirectoriesFromPath(string path)
        {
            IEnumerable<string> directories = Array.Empty<string>();

            try
            {
                directories = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            }
            catch (Exception ex)
            {
                _inaccessiblePaths.Add(new InaccessiblePath(path, ex));
            }

            return Task.FromResult(directories);
        }
    }
}
