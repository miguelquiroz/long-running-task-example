using System;
namespace long_running_task_example.BackgroundTask
{
    public interface IBackgroundTaskQueue
    {
        // Enqueues the given task.
        void EnqueueTask(Func<IServiceScopeFactory, CancellationToken, Task> task);

        // Dequeues and returns one task. This method blocks until a task becomes available.
        Task<Func<IServiceScopeFactory, CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}

