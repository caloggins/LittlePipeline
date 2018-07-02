using FakeItEasy;
using LittlePipeline.Tests.Bits;
using Xunit;

namespace LittlePipeline.Tests.Examples
{
    public class PipelineTestExample
    {
        [Fact]
        public void ThePipelineCanBeTested()
        {
            var pipeline = A.Fake<IPipeline<FirstTestSubject>>();
            var example = new ThingThatUsesThePipeline(pipeline);


            var subject = new FirstTestSubject();
            example.Run(subject);

            A.CallTo(() => pipeline.Subject(subject)).MustHaveHappened()
                .Then(A.CallTo(() => pipeline.Do<Increment>()).MustHaveHappened())
                .Then(A.CallTo(() => pipeline.Do<Square>()).MustHaveHappened());
        }
    }

    public class ThingThatUsesThePipeline
    {
        private readonly IPipeline<FirstTestSubject> pipeline;

        public ThingThatUsesThePipeline(IPipeline<FirstTestSubject> pipeline)
        {
            this.pipeline = pipeline;
        }

        public void Run(FirstTestSubject subject)
        {
            pipeline.Subject(subject);
            pipeline.Do<Increment>();
            pipeline.Do<Square>();
        }
    }
}