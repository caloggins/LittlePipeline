namespace TaskProcessor
{
    public interface ITaskFactory
    {
        TTask Create<TTask>()
            where TTask : ITask;
    }
}