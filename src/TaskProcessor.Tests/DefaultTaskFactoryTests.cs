using System;
using FluentAssertions;
using NUnit.Framework;
using TaskProcessor.Tests.Bits;

namespace TaskProcessor.Tests
{
    public class DefaultTaskFactoryTests
    {
        private DefaultTaskFactory factory;

        [SetUp]
        public void SetUp()
        {
            factory = new DefaultTaskFactory();
            factory.Register<SuperTask>(() => new SuperTask(5));
        }

        [Test]
        public void ItReturnsRegisteredTasks()
        {
            var superTask = factory.Create<SuperTask, FirstTestSubject>();

            superTask.Should().NotBeNull();
        }

        [Test]
        public void ItThrowsExceptionsWhenTheTaskIsMissing()
        {
            Action act = () => { factory.Create<Increment, FirstTestSubject>(); };

            act.ShouldThrow<MissingRegistrationException>()
                .WithMessage("No registration for the Increment task was found.");
        }

        [Test]
        public void AddingTheSameTaskThrowsAnException()
        {
            Action act = () => { factory.Register<SuperTask>(() => null); };

            act.ShouldThrow<TaskAlreadyRegisteredException>()
                .WithMessage("The task SuperTask has already been registered.");
        }

    }
}