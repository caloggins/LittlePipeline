namespace TaskProcessor
{
    public interface ITaskFactory
    {
        TTask Create<TTask, TSubject>()
            where TTask : ITask<TSubject>
            where TSubject : class;
    }
}