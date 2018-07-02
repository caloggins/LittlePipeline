using System;
using FluentAssertions;
using LittlePipeline.Tests.Bits;
using NUnit.Framework;

namespace LittlePipeline.Tests
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
            var superTask = factory.Create<SuperTask>();

            superTask.Should().NotBeNull();
        }

        [Test]
        public void ItThrowsExceptionsWhenTheTaskIsMissing()
        {
            Action act = () => { factory.Create<Increment>(); };

            act.Should().Throw<MissingRegistrationException>()
                .WithMessage("No registration for the Increment task was found.");
        }

        [Test]
        public void AddingTheSameTaskThrowsAnException()
        {
            Action act = () => { factory.Register<SuperTask>(() => null); };

            act.Should().Throw<TaskAlreadyRegisteredException>()
                .WithMessage("The task SuperTask has already been registered.");
        }

    }
}