using FluentAssertions;
using LittlePipeline.Std21.Tests.Bits;
using Ninject;
using Ninject.Extensions.Factory;
using Xunit;

namespace LittlePipeline.Std21.Tests.Examples
{
    public class NinjectExample
    {
        [Fact]
        public void ItCanUseOtherContainers()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ITaskFactory>().ToFactory();

            kernel.Bind<Increment>().ToSelf();
            kernel.Bind<Square>().ToSelf();

            kernel.Bind(typeof(IPipeline<>)).To(typeof(Pipeline<>));

            var pipeline = kernel.Get<IPipeline<FirstTestSubject>>();

            var subject = new FirstTestSubject();
            pipeline.Subject(subject);
            pipeline.Do<Increment>();
            pipeline.Do<Increment>();
            pipeline.Do<Square>();

            subject.Value.Should().Be(4);
        }
    }
}