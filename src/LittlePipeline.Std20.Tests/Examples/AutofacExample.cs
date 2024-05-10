using Autofac;
using FluentAssertions;
using LittlePipeline.Std20.Tests.Bits;
using Xunit;

namespace LittlePipeline.Std20.Tests.Examples;

public class TaskFactory(IComponentContext container) : ITaskFactory
{
    public TTask Create<TTask>() where TTask : ITask =>
        container.Resolve<TTask>();

    public TTask CreateAsync<TTask>() where TTask : IAsyncTask =>
        container.Resolve<TTask>();
}

public class AutofacExample
{
    [Fact]
    public void Autofac_Example()
    {
        var builder = new ContainerBuilder();

        builder.RegisterType<TaskFactory>().As<ITaskFactory>().SingleInstance();

        builder.RegisterType<Increment>().AsSelf();
        builder.RegisterType<Square>().AsSelf();

        builder.RegisterGeneric(typeof(Pipeline<>)).As(typeof(IPipeline<>)).AsImplementedInterfaces();

        var container = builder.Build();

        var pipeline = container.Resolve<IPipeline<FirstTestSubject>>();

        var subject = new FirstTestSubject();
        pipeline.Subject(subject);
        pipeline.Do<Increment>();
        pipeline.Do<Increment>();
        pipeline.Do<Square>();

        subject.Value.Should().Be(4);
    }
}