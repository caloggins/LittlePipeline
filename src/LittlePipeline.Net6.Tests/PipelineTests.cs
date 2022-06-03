namespace LittlePipeline.Net6.Tests;

using System;
using System.Threading.Tasks;
using Bits;
using FluentAssertions;
using Xunit;

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

        var act = () => sut.Do<Increment>();

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

    [Fact]
    public async void PipelinesCanExecuteAsyncTasks()
    {
        var sut = GetPipeline<FirstTestSubject>();
        var subject = new FirstTestSubject { Value = 5 };

        sut.Subject(subject);
        await sut.DoAsync<IncrementAsync>();

        subject.Value.Should().Be(6);
    }

    [Fact]
    public async void AsyncTasksRequireSubjects()
    {
        var sut = GetPipeline<FirstTestSubject>();

        Func<Task> act = async () => { await sut.DoAsync<IncrementAsync>(); };

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("No subject has been set, cannot perform any tasks.");
    }

    private static IPipeline<TSubject> GetPipeline<TSubject>()
        where TSubject : class
    {
        var factory = new DefaultTaskFactory();
        factory.Register<Increment>(() => new Increment());
        factory.Register<Square>(() => new Square());
        factory.Register<IncrementAsync>(() => new IncrementAsync());

        return new Pipeline<TSubject>(factory);
    }
}