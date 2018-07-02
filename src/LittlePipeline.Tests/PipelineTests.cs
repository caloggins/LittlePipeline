using System;
using FluentAssertions;
using LittlePipeline.Tests.Bits;
using Xunit;

namespace LittlePipeline.Tests
{
    /* 
     * process.Subject(thing)
     * process.Do<Task>()
     */
    public class PipelineTests
    {
        [Fact]
        public void ItPerformsTasks()
        {
            var sut = GetPipeline<FirstTestSubject>();
            var subject = new FirstTestSubject { Value = 0 };

            sut.Subject(subject);
            sut.Do<Increment>();

            subject.Value.Should().Be(1);
        }

        [Fact]
        public void ItThrowsExceptionsWhenThereIsNoSubject()
        {
            var sut = GetPipeline<FirstTestSubject>();
            Action act = () => sut.Do<Increment>();

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("No subject has been set, cannot perform any tasks.");
        }

        [Fact]
        public void ItAcceptsDifferentSubjects()
        {
            var sut = GetPipeline<SecondTestSubject>();
            var subject = new SecondTestSubject { Value = 0 };

            sut.Subject(subject);
            sut.Do<Increment>();

            subject.Value.Should().Be(1);
        }

        [Fact]
        public void ItPerformsMultipleTasks()
        {
            var sut = GetPipeline<FirstTestSubject>();
            var subject = new FirstTestSubject { Value = 0 };

            sut.Subject(subject);
            sut.Do<Increment>();
            sut.Do<Increment>();
            sut.Do<Square>();

            subject.Value.Should().Be(4);
        }

        [Fact]
        public void ItCanUseAFactoryForTasks()
        {
            var sut = GetPipeline<FirstTestSubject>();
            var subject = new FirstTestSubject { Value = 5 };

            sut.Subject(subject);
            sut.Do<Increment>();

            subject.Value.Should().Be(6);
        }

        private static Pipeline<TSubject> GetPipeline<TSubject>()
            where TSubject : class 
        {
            var factory = new DefaultTaskFactory();
            factory.Register<Increment>(() => new Increment());
            factory.Register<Square>(() => new Square());

            return new Pipeline<TSubject>(factory);
        }

    }

}
