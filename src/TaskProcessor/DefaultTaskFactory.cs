using System;
using System.Collections.Concurrent;

namespace TaskProcessor
{
    public class DefaultTaskFactory : ITaskFactory
    {
        private readonly ConcurrentDictionary<Type, Func<object>> creators = new ConcurrentDictionary<Type, Func<object>>();

        public TTask Create<TTask, TSubject>()
            where TTask : ITask<TSubject>
            where TSubject : class
        {
            creators.TryGetValue(typeof(TTask), out Func<object> creator);

            if(creator == null)
                throw new MissingRegistrationException($"No registration for the {typeof(TTask).Name} task was found.");

            return (TTask) creator();
        }

        public void Register<TTask>(Func<object> creator)
        {
            if(creators.ContainsKey(typeof(TTask)))
                throw new TaskAlreadyRegisteredException($"The task {typeof(TTask).Name} has already been registered.");

            creators.TryAdd(typeof(TTask), creator);
        }
    }
}