using System;
using System.Diagnostics;

namespace TaskProcessor
{
    public class Pipeline<TSubject>
        where TSubject : class
    {
        private TSubject subject;

        [DebuggerStepThrough]
        public void Subject(TSubject newSubject)
        {
            subject = newSubject;
        }

        [DebuggerStepThrough]
        public void Do<TTask>()
            where TTask : ITask<TSubject>, new()
        {
            if (subject == null)
                throw new InvalidOperationException("No subject has been set, cannot perform any tasks.");

            var task = new TTask();
            task.Run(subject);
        }
    }
}
