using System;

namespace LittlePipeline.Tests.Bits
{
    public class SuperTask : ITask<FirstTestSubject>
    {
        private readonly int threshold;

        public SuperTask(int threshold)
        {
            this.threshold = threshold;
        }

        public void Run(FirstTestSubject subject)
        {
            if (subject.Value > threshold)
                throw new InvalidOperationException("Value is too large!");
        }
    }
}