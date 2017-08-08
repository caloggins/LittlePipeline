# LittlePipeline

[![Build status](https://ci.appveyor.com/api/projects/status/github/caloggins/littlepipeline?svg=true)](https://ci.appveyor.com/project/caloggins/littlepipeline/branch/master)
[![NuGet](http://img.shields.io/nuget/v/LittlePipeline.svg)](https://www.nuget.org/packages/LittlePipeline/)

### What is it?

A pipeline, process, or whatever is a sequence of actions that need to be performed to complete an operation. LittlePipeline is a small example of how to create a pipeline that can be tested, and used with any common IoC container. This is a very basic example. It is not designed to handle concurrent tasks. The idea is to use it to express a sequence of steps that must happen to complete a process.

### Getting started.

You will need a subject. This is the class upon which the process is being performed. Subjects must be a `class`.

```csharp
public class Counter
{
    public int Value { get; set; }
}
```

You will need one or more tasks. Tasks must implement the `ITask<TSubject>` interface.

```csharp
public class Increment : ITask<Counter>
{
    public void Run(Counter counter)
    {
        counter.Value++;
    }
}
```

Once your subject, and tasks are defined, it's time to setup the task factory. LittlePipeline comes with a very basic task factory which can be used if you're not using an IoC container in your application. Each task must be registered in the task factory.

```csharp
var pipeline = MakePipeline.ForSubject<FirstTestSubject>()
    .With<Increment>(() => new Increment())
    .Build();
```

Using the pipeline is pretty easy after that:

```csharp
var subject = new FirstTestSubject();
pipeline.Subject(subject);
pipeline.Do<Increment>();
```

### How do I use it with my IoC container?

Using the pipeline with an IoC container is pretty easy. Just register the task factory, tasks, and pipeline with your container. Here's an example using [Ninject](http://www.ninject.org/):

```csharp
var kernel = new StandardKernel();
kernel.Bind<ITaskFactory>().ToFactory();

kernel.Bind<Increment>().ToSelf();
kernel.Bind<Square>().ToSelf();

kernel.Bind(typeof(IPipeline<>)).To(typeof(Pipeline<>));
```

Then, just resolve and use the pipeline normally (or inject it into another class):

```csharp
var pipeline = kernel.Get<IPipeline<FirstTestSubject>>();

var subject = new FirstTestSubject();
pipeline.Subject(subject);
pipeline.Do<Increment>();
```

### How do I test the pipeline?

Testing the pipeline's use is as easy as testing that calls were made in the correct order. First, the class that's using a pipeline:

```csharp
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
```

And here's the tests using [FakeItEasy](https://fakeiteasy.github.io/):

```csharp
var pipeline = A.Fake<IPipeline<FirstTestSubject>>();
var example = new ThingThatUsesThePipeline(pipeline);


var subject = new FirstTestSubject();
example.Run(subject);

A.CallTo(() => pipeline.Subject(subject)).MustHaveHappened()
    .Then(A.CallTo(() => pipeline.Do<Increment>()).MustHaveHappened())
    .Then(A.CallTo(() => pipeline.Do<Square>()).MustHaveHappened());
```
