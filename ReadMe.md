# LittlePipeline

[![Build Status](https://github.com/caloggins/LittlePipeline/actions/workflows/build-test.yaml/badge.svg)](https://github.com/caloggins/LittlePipeline/actions/workflows/build-test.yaml)
[![NuGet](https://img.shields.io/nuget/v/LittlePipeline)](https://www.nuget.org/packages/LittlePipeline/)
![Downloads](https://img.shields.io/nuget/dt/LittlePipeline)

## What is it?

A pipeline, process, or whatever is a sequence of actions that need to be performed to complete an operation. LittlePipeline is a small example of how to create a pipeline that can be tested, and used with any common IoC container. This is a very basic example. It is not designed to handle concurrent tasks. The idea is to use it to express a sequence of steps that must happen to complete a process.

**Note: v3 is something of a breaking change. The library now only targets .NET Std 2.0.**

## Getting started.

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
## What about async methods?
LittlePipeline supports async methods. To use async operations, the tasks must be defined as asynchronous:

```csharp
public class Incrementer : IAsyncTask<Counter>
{
    public Task Run(Counter counter)
    {
        await Task.Run(() => counter.Value++);
    }
}
```

Then, simply use the `DoAsync<T>()` method, instead of the normal method:

```csharp
var subject = new FirstTestSubject();
pipeline.Subject(subject);
await pipeline.DoAsync<Incrementer>();
```

## How do I use it with my IoC container?
### Ninject
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
### Autofac
```csharp
var builder = new ContainerBuilder();

builder.RegisterType<TaskFactory>().As<ITaskFactory>().SingleInstance();

builder.RegisterType<Increment>().AsSelf();
builder.RegisterType<Square>().AsSelf();

builder.RegisterGeneric(typeof(Pipeline<>)).As(typeof(IPipeline<>)).AsImplementedInterfaces();

var container = builder.Build();

var pipeline = container.Resolve<IPipeline<FirstTestSubject>>();

var subject = new FirstTestSubject();
pipeline.Subject(subject);
pipeline.Do<Increment>();
pipeline.Do<Increment>();
pipeline.Do<Square>();
```
A very simple factory class...
```csharp
public class TaskFactory(IComponentContext container) : ITaskFactory
{
    public TTask Create<TTask>() where TTask : ITask =>
        container.Resolve<TTask>();

    public TTask CreateAsync<TTask>() where TTask : IAsyncTask =>
        container.Resolve<TTask>();
}
```

## How do I test the pipeline?

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

## Known Issues

No known issues.

## Version History

__v3.0.0__
- Solely targets .NET Std 2.0.

__v2.0.0__
- Upgraded to use .NET Standard

__v1.1.0__
- Support for async tasks.
- Support for .NET 4.6.1.
- Various dependencies updated.

__v1.0.0__
- Initial release, supporting .NET 4.5.2, and 4.6.2.