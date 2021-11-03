using System;
namespace long_running_task_example.BackgroundTask
{


    public interface ILongRunningWork
    {
        Task Run(string data);
    }


    public class LongRunningWork : ILongRunningWork
    {
        public LongRunningWork()
        {
        }

        public async Task Run(string data)
        {
            await Task.CompletedTask;
        }
    }
}

