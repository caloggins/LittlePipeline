namespace LittlePipeline.Net6.Tests.Bits
{
    using System.Threading.Tasks;

    public class IncrementAsync : IAsyncTask<FirstTestSubject>
    {
        public async Task Run(FirstTestSubject subject)
        {
            await Task.Run(() => subject.Value++);
        }
    }
}