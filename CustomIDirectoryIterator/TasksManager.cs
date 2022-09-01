using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace CustomDirectoryIterator
{
    internal class TasksManager<T>
    {
        private readonly ConcurrentQueue<Task<T>> _pendingTasks;
        private readonly ConcurrentQueue<Task<T>> _runningTasks;
        private readonly int _maxParallelProcess;

        public TasksManager()
        {
            _maxParallelProcess = (Environment.ProcessorCount * 2) - 1;
            _pendingTasks = new ConcurrentQueue<Task<T>>();
            _runningTasks = new ConcurrentQueue<Task<T>>();
        }

        public TasksManager(int maxParallelProcess)
        {
            _maxParallelProcess = maxParallelProcess;
            _pendingTasks = new ConcurrentQueue<Task<T>>();
            _runningTasks = new ConcurrentQueue<Task<T>>();
        }

        public void AddTask(Func<object, T> action, object actionArguments)
        {
            _ = action ?? throw new ArgumentNullException(nameof(action));

            var newTask = new Task<T>(action, actionArguments);
            _pendingTasks.Enqueue(newTask);
        }

        public void AddTask(Func<T> action)
        {
            _ = action ?? throw new ArgumentNullException(nameof(action));

            var newTask = new Task<T>(action);
            _pendingTasks.Enqueue(newTask);
        }

        public void RunTasks()
        {
            while (_pendingTasks.Any())
            {
                var runningtaskCount = GetTasksRunningCount();

                if (runningtaskCount >= _maxParallelProcess)
                    Task.WaitAny(_runningTasks.ToArray());

                var canBeTaken = _pendingTasks.TryDequeue(out Task<T> task);

                if (canBeTaken)
                {
                    task.Start();
                    _runningTasks.Enqueue(task);
                }
            }
        }

        public void WaitTasks()
        {
            Task.WaitAll(_runningTasks.ToArray());
        }

        //public async Task WaitTasksAsync()
        //{
        //    await Task.WhenAll(_runningTasks.ToArray());
        //}

        public async Task<T[]> ReceiveTasks()
        {
            var runningtasksArr = _runningTasks.ToArray();
            _runningTasks.Clear();
            return await Task.WhenAll(runningtasksArr);
        }

        private int GetTasksRunningCount() => _runningTasks.Where(t => t.Status == TaskStatus.Running).Count();
    }
}
