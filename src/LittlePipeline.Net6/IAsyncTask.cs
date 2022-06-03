namespace LittlePipeline.Net6
{
    using System.Threading.Tasks;

    public interface IAsyncTask{

    }

    public interface IAsyncTask<in TSubject> : IAsyncTask
        where TSubject : class
    {
        Task Run(TSubject subject);
    }
}