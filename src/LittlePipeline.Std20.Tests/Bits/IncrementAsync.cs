using System.Threading.Tasks;

namespace LittlePipeline.Std20.Tests.Bits
{
    public class IncrementAsync : IAsyncTask<FirstTestSubject>
    {
        public async Task Run(FirstTestSubject subject)
        {
            await Task.Run(() => subject.Value++);
        }
    }
}