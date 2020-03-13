using System.Threading.Tasks;

namespace LittlePipeline
{
    public interface IPipeline<TSubject>
        where TSubject : class
    {
        void Subject(TSubject newSubject);

        void Do<TTask>()
            where TTask : ITask<TSubject>;

        Task DoAsync<TTask>()
            where TTask : IAsyncTask<TSubject>;
    }
}