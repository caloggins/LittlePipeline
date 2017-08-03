namespace LittlePipeline
{
    public interface ITaskFactory
    {
        TTask Create<TTask>()
            where TTask : ITask;
    }
}