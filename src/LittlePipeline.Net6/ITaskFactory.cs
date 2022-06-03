namespace LittlePipeline.Net6
{
    public interface ITaskFactory
    {
        TTask Create<TTask>()
            where TTask : ITask;

        TTask CreateAsync<TTask>() where TTask : IAsyncTask;
    }
}