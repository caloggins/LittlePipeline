namespace LittlePipeline
{
    public interface ITask
    {
        
    }

    public interface ITask<in TSubject> : ITask
        where TSubject : class
    {
        void Run(TSubject subject);
    }
}