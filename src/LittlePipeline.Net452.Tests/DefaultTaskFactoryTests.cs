using System;
using FluentAssertions;
using LittlePipeline.Tests.Bits;
using Xunit;

namespace LittlePipeline.Net452.Tests
{
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
            Action act = () => { factory.Create<Increment>(); };

            act.Should().Throw<MissingRegistrationException>()
                .WithMessage("No registration for the Increment task was found.");
        }

        [Fact]
        public void AddingTheSameTaskThrowsAnException()
        {
            Action act = () => { factory.Register<SuperTask>(() => null); };

            act.Should().Throw<TaskAlreadyRegisteredException>()
                .WithMessage("The task SuperTask has already been registered.");
        }

    }
}