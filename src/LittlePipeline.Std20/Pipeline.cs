using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LittlePipeline
{
    public class Pipeline<TSubject>(ITaskFactory factory) : IPipeline<TSubject>
        where TSubject : class
    {
        private TSubject subject = null!;
        
        [DebuggerStepThrough]
        public void Subject(TSubject newSubject)
        {
            subject = newSubject;
        }

        [DebuggerStepThrough]
        public void Do<TTask>()
            where TTask : ITask<TSubject>
        {
            CheckForSubject();

            var task = factory.Create<TTask>();
            task.Run(subject);
        }

        [DebuggerStepThrough]
        public async Task DoAsync<TTask>()
            where TTask: IAsyncTask<TSubject>
        {
            CheckForSubject();

            var task = factory.CreateAsync<TTask>();
            await task.Run(subject);
        }

        private void CheckForSubject()
        {
            if (subject == null)
                throw new InvalidOperationException("No subject has been set, cannot perform any tasks.");
        }
    }
}
