using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace LittlePipeline
{
    public class DefaultTaskFactory : ITaskFactory
    {
        private readonly ConcurrentDictionary<Type, Func<object>> creators = new ConcurrentDictionary<Type, Func<object>>();

        [DebuggerStepThrough]
        public TTask Create<TTask>()
            where TTask : ITask
        {
            creators.TryGetValue(typeof(TTask), out Func<object> creator);

            if(creator == null)
                throw new MissingRegistrationException($"No registration for the {typeof(TTask).Name} task was found.");

            return (TTask) creator();
        }

        [DebuggerStepThrough]
        public TTask CreateAsync<TTask>() where TTask : IAsyncTask
        {
            creators.TryGetValue(typeof(TTask), out Func<object> creator);

            if (creator == null)
                throw new MissingRegistrationException($"No registration for the {typeof(TTask).Name} task was found.");

            return (TTask)creator();
        }

        [DebuggerStepThrough]
        public void Register<TTask>(Func<object> creator)
        {
            if(creators.ContainsKey(typeof(TTask)))
                throw new TaskAlreadyRegisteredException($"The task {typeof(TTask).Name} has already been registered.");

            creators.TryAdd(typeof(TTask), creator);
        }
    }
}