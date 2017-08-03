namespace LittlePipeline.Tests.Bits
{
    public class Increment : ITask<FirstTestSubject>, ITask<SecondTestSubject>
    {
        public void Run(FirstTestSubject subject)
        {
            subject.Value++;
        }

        public void Run(SecondTestSubject subject)
        {
            subject.Value++;
        }
    }
}