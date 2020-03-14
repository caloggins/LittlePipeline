namespace LittlePipeline.Std21.Tests.Bits
{
    public class Square : ITask<FirstTestSubject>
    {
        public void Run(FirstTestSubject subject)
        {
            subject.Value = subject.Value * 2;
        }
    }
}