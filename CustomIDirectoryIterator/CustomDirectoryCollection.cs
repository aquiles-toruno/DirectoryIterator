using CustomDirectoryIterator.Contracts;
using CustomDirectoryIterator.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomDirectoryIterator
{
    internal class CustomDirectoryCollection : ICustomDirectoryCollection
    {
        private string _path;
        private readonly string _rootPath;
        private readonly int? _maxParallelProcess;
        private readonly IInaccessiblePathCollection _inaccessiblePaths;
        private readonly TasksManager<IEnumerable<string>> _tasks;

        public IInaccessiblePathCollection InaccessiblePathCollection => _inaccessiblePaths;

        public CustomDirectoryCollection(string path, int? maxParallelProcess = default)
        {
            _path = _rootPath = path;
            _maxParallelProcess = maxParallelProcess;
            _inaccessiblePaths = new InaccessiblePathCollection();
            _tasks = new TasksManager<IEnumerable<string>>();

            if (maxParallelProcess.HasValue)
                _tasks = new TasksManager<IEnumerable<string>>(maxParallelProcess.Value);
        }

        public virtual IEnumerator<string> GetEnumerator()
        {
            foreach (var item in MyEnumerator())
            {
                yield return item;
            }

            _path = _rootPath;
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

            foreach (var directory in directories)
            {
                yield return directory;
            }

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

            foreach (var directory in directories)
            {
                _tasks.AddTask(GetDirectoriesFromPath, directory);
            }

            _tasks.RunTasks();
            _tasks.WaitTasks();

            tasksResult = await _tasks.ReceiveTasks();

            tasksResult.ToList().ForEach(tr =>
            {
                directoriesFound = directoriesFound.Concat(tr);
            });

            return directoriesFound;
        }

        private IEnumerable<string> GetDirectoriesFromPath(object path)
        {
            var _path = path.ToString();
            IEnumerable<string> directories = Array.Empty<string>();

            try
            {
                directories = Directory.GetDirectories(_path, "*", SearchOption.TopDirectoryOnly);
            }
            catch (Exception ex)
            {
                _inaccessiblePaths.Add(new InaccessiblePath(_path, ex));
            }

            return directories;
        }
    }
}
