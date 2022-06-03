namespace LittlePipeline.Net6
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