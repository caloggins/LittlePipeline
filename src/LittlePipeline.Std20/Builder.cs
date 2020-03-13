using System;

namespace LittlePipeline
{
    public class Builder<TSubject> where TSubject : class
    {
        private readonly DefaultTaskFactory factory = new DefaultTaskFactory();

        public Builder<TSubject> With<TTask>(Func<object> creator)
        {
            factory.Register<TTask>(creator);
            return this;
        }

        public Pipeline<TSubject> Build()
        {
            return new Pipeline<TSubject>(factory);
        }
    }
}