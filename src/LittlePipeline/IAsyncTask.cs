using System.Threading.Tasks;

namespace LittlePipeline
{
    public interface IAsyncTask{

    }

    public interface IAsyncTask<in TSubject> : IAsyncTask
        where TSubject : class
    {
        Task Run(TSubject subject);
    }
}