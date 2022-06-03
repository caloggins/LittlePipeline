namespace LittlePipeline.Net6.Tests;

using System.Diagnostics;
using System.Linq;
using Bits;
using FluentAssertions;
using Xunit;

public class DefaultTaskFactoryTests
{
    private readonly DefaultTaskFactory factory;

    public DefaultTaskFactoryTests()
    {
        factory = new DefaultTaskFactory();
        factory.Register<SuperTask>(() => new SuperTask(5));
    }

    [Fact]
    public void ItReturnsRegisteredTasks()
    {
        var superTask = factory.Create<SuperTask>();

        superTask.Should().NotBeNull();
    }

    [Fact]
    public void ItThrowsExceptionsWhenTheTaskIsMissing()
    {
        var act = () => { factory.Create<Increment>(); };

        act.Should().Throw<MissingRegistrationException>()
            .WithMessage("No registration for the Increment task was found.");
    }

    [Fact]
    public void AddingTheSameTaskThrowsAnException()
    {
        var act = () => { factory.Register<SuperTask>(() => null); };

        act.Should().Throw<TaskAlreadyRegisteredException>()
            .WithMessage("The task SuperTask has already been registered.");
    }

    [Fact]
    public void TheMethodsShouldHaveDebuggerStepThrough()
    {
        factory.GetType()
            .Methods()
            .ThatArePublicOrInternal
            .ThatAreNotDecoratedWith<DebuggerStepThroughAttribute>()
            .Count()
            .Should().Be(0);
    }
}