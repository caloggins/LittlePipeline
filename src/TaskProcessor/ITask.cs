namespace TaskProcessor
{
    public interface ITask<in TSubject>
        where TSubject : class
    {
        void Run(TSubject subject);
    }
}