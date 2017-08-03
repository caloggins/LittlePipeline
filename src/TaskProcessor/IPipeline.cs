namespace TaskProcessor
{
    public interface IPipeline<TSubject>
        where TSubject : class
    {
        void Subject(TSubject newSubject);

        void Do<TTask>()
            where TTask : ITask<TSubject>;
    }
}