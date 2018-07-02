using FluentAssertions;
using LittlePipeline.Tests.Bits;
using Xunit;

namespace LittlePipeline.Tests
{
    public class PipelineFactoryTests
    {
        [Fact]
        public void ItCreatesPipelines()
        {
            MakePipeline.ForSubject<FirstTestSubject>().Build()
                .Should().NotBeNull();
        }

        [Fact]
        public void ItCanAddTasks()
        {
            var pipeline = MakePipeline.ForSubject<FirstTestSubject>()
                .With<Increment>(() => new Increment())
                .Build();

            var subject = new FirstTestSubject();
            pipeline.Subject(subject);
            pipeline.Do<Increment>();

            subject.Value.Should().Be(1);
        }
    }
}