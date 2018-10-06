# AkkaTestingHelpers

This NuGet package offers helper classes for both unit testing and integration testing with Akka.NET.

For a detailed explanation as to why I created the package and how it works, see my [blog post](http://connelhooley.uk/blog/2017/09/30/introducing-akka-testing-helpers-di).

## Unit testing

The `UnitTestFramework` class in the package allows you to test an actor class in full isolation. The framework creates the actor to be tested with a mock parent in the form of a `TestProbe` object. It also replaces any children that the actor under test creates using `Akka.DI` with mocks in the form of `TestProbe` objects. This means you have to create your children like this for the children to be replaced with `TestProbe`s:

``` csharp
var child = Context.ActorOf(Context.DI().Props<ChildActor>(), "child-1");
```

If you want to test `Actor D` in the following hierarchy:
![Actor hierarchy highlighted where testing actor D](http://connelhooley.uk/assets/images/introducing-akka-testing-helpers-di/actor-hierarchy-testing-d.png)

The `UnitTestFramework` framework configures `Actor D` like this:
![Actor hierarchy highlighted where testing actor D with mocks](http://connelhooley.uk/assets/images/introducing-akka-testing-helpers-di/actor-hierarchy-testing-d-mocks.png)

It can be used to test the following scenarios:

- [That an actor gives a child the correct name & type](#asserting-children-are-created-with-the-correct-names--types)
- [That an actor sends the correct messages to its children](#asserting-children-are-sent-the-correct-messages)
- [That an actor processes replies from its children correctly](#asserting-replies-from-children-are-processed-correctly)
- [That an actor gives a child the correct supervisor strategy](#asserting-the-supervisor-strategies-that-children-are-created-with)
- [That an actor sends the correct messages to its parent](#asserting-the-correct-messages-are-sent-to-the-parent)
- [That an actor processes replies from its parent correctly](#asserting-replies-from-the-parent-are-processed-correctly)
- [That an actor throws an exception](#asserting-that-exceptions-are-thrown)

It also has a couple features that you may find useful:

- [Adding delays to your tests that honour the timefactor](#adding-a-delay-to-a-test-that-honours-timefactor)
- [Specifying the decider for the actor under test](#specifying-the-decider)

The following examples can also be found in the [examples](AkkaTestingHelpers.MediumTests/UnitTestFrameworkTests/Examples) folder in this repo.

### Asserting Children Are Created With The Correct Names & Types

``` csharp
public class Example : TestKit
{
    [Fact]
    public void SutActor_Constructor_CreatesChildWithCorrectTypeAndName()
    {
        //act
        UnitTestFramework<SutActor> framework = UnitTestFrameworkSettings
            .Empty
            .CreateFramework<SutActor>(this, 2);

        //assert
        framework
            .ResolvedType("child-actor-1")
            .Should().Be<ChildActor>();
    }

    public class ChildActor : ReceiveActor { } // Replaced by a TestProbe by the UnitTestFramework

    public class SutActor : ReceiveActor
    {
        public SutActor() => Context.ActorOf(Context.DI().Props<ChildActor>(), "child-actor-1");
    }
}
```

Note how we pass `2` into the `CreateFramework` method. The framework blocks the current thread until the expected number of children have been created.

When Akka tries to use DI to resolve `ChildActor` the `UnitTestFramework` resolves it to a `TestProbe`. The `ResolvedType` method allows you to still check that the actor under test is requesting the correct type of child actor.

You can also use the `TellMessageAndWaitForChildren` method if an actor creates children when it receives a message. The method blocks the thread until the given number of children are resolved. You can then use the `ResolvedTestProbe` and `ResolvedType` methods to run assertions against the newly created actors.

``` csharp
UnitTestFramework<ParentActor> framework = UnitTestFrameworkSettings
    .Empty
    .CreateFramework<SutActor>(this);

framework.TellMessageAndWaitForChildren(new MessageThatCreatesTwoChildren(), 2);
```

### Asserting Children Are Sent The Correct Messages

``` csharp
public class Example : TestKit
{
    [Fact]
    public void SutActor_Constructor_SendsChildCorrectMessage()
    {
        //act
        UnitTestFramework<SutActor> framework = UnitTestFrameworkSettings
            .Empty
            .CreateFramework<SutActor>(this, 2);

        //assert
        framework
            .ResolvedTestProbe("child-actor-1")
            .ExpectMsg("hello actor 1");
    }  

    public class ChildActor : ReceiveActor { } // Replaced by a TestProbe by the UnitTestFramework

    public class SutActor : ReceiveActor
    {
        public SutActor()
        {
            var child1 = Context.ActorOf(Context.DI().Props<ChildActor>(), "child-actor-1");
            var child2 = Context.ActorOf(Context.DI().Props<ChildActor>(), "child-actor-2");
            child1.Tell("hello actor 1");
            child2.Tell(42);
        }
    }
}
```

The `ResolvedTestProbe` method returns the `TestProbe` instance that the `UnitTestFramework` resolved that child as. It allows you assert messages are sent to specific children.

### Asserting Replies From Children Are Processed Correctly

``` csharp
public class Example : TestKit
{
    [Fact]
    public void SutActor_ReceiveSaveMessage_StoresModifiedSaveMessageFromChildInRepo()
    {
        //arrange
        Mock<IRepository> repoMock = new Mock<IRepository>();
        UnitTestFramework<SutActor> framework = UnitTestFrameworkSettings
            .Empty
            .RegisterChildHandler<ChildActor, SutActor.Save>(s => new ChildActor.ModifiedSave(s.Value.ToUpper()))
            .CreateFramework<SutActor>(this, Props.Create(() => new SutActor(repoMock.Object)), 1);

        //act
        framework.Sut.Tell(new SutActor.Save("hello world"));

        //assert
        AwaitAssert(() => repoMock.Verify(repo => repo.Save("HELLO WORLD"), Times.Once));
    }

    public class ChildActor : ReceiveActor // Replaced by a TestProbe by the UnitTestFramework
    {
        public class ModifiedSave // Message that the ChildActor receives
        {
            public string Value { get; }

            public ModifiedSave(string value)
            {
                Value = value;
            }
        }
    }

    public interface IRepository // A dependency injected into SutActor
    {
        void Save(string value);
    }

    public class SutActor : ReceiveActor
    {
        public SutActor(IRepository repo)
        {
            var child = Context.ActorOf(Context.DI().Props<ChildActor>(), "child-actor-1");
            Receive<Save>(s => child.Tell(s));
            Receive<ChildActor.ModifiedSave>(s => repo.Save(s.Value));
        }

        public class Save // Message that the SutActor receives
        {
            public string Value { get; }

            public Save(string value)
            {
                Value = value;
            }
        }
    }
}
```

Above, the `RegisterChildHandler` method is used to register a handler against the `ChildActor` type:

``` csharp
RegisterChildHandler<ChildActor, SutActor.Save>(s => new ChildActor.ModifiedSave(s.Value.ToUpper()))
```

This example results in the following: TestProbes that are replacing `ChildActor` instances, will receive `SutActor.Save` messages and reply with upper-cased `ChildActor.ModifiedSave` messages. You can then test that the actor under test handles messages from its children correctly, without having to use the full implementation of the child.

### Asserting The Supervisor Strategies That Children Are Created With

``` csharp
public class Example : TestKit
{
    [Fact]
    public void SutActor_Constructor_CreatesChild1WithCorrectStrategy()
    {
        //act
        UnitTestFramework<SutActor> framework = UnitTestFrameworkSettings
            .Empty
            .CreateFramework<SutActor>(this, 2);

        //assert
        framework.ResolvedSupervisorStrategy("child-1")
            .Should().BeOfType<OneForOneStrategy>();

        framework.ResolvedSupervisorStrategy("child-1")
            .As<OneForOneStrategy>().MaxNumberOfRetries
            .Should().Be(1);

        framework.ResolvedSupervisorStrategy("child-1")
            .As<OneForOneStrategy>().WithinTimeRangeMilliseconds
            .Should().Be(1000);

        framework.ResolvedSupervisorStrategy("child-1")
            .As<OneForOneStrategy>().Decider.Decide(new Exception())
            .Should().Be(Directive.Stop);
    }

    [Fact]
    public void SutActor_Constructor_CreatesChild2WithCorrectStrategy()
    {
        //act
        UnitTestFramework<SutActor> framework = UnitTestFrameworkSettings
            .Empty
            .CreateFramework<SutActor>(this, 2);

        //assert
        framework.ResolvedSupervisorStrategy("child-2")
            .Should().BeOfType<AllForOneStrategy>();

        framework.ResolvedSupervisorStrategy("child-2")
            .As<AllForOneStrategy>().MaxNumberOfRetries
            .Should().Be(3);

        framework.ResolvedSupervisorStrategy("child-2")
            .As<AllForOneStrategy>().WithinTimeRangeMilliseconds
            .Should().Be(500);

        framework.ResolvedSupervisorStrategy("child-2")
            .As<AllForOneStrategy>().Decider.Decide(new Exception())
            .Should().Be(Directive.Escalate);
    }

    public class ChildActor : ReceiveActor { } // Replaced by a TestProbe by the UnitTestFramework

    public class SutActor : ReceiveActor
    {
        public SutActor()
        {
            Thread.Sleep(500);

            Context.ActorOf(Context.DI().Props<ChildActor>(), "child-1");

            var child2SupervisorStrategy = new AllForOneStrategy( // SupervisorStrategy given to child-2
                    3,
                    500,
                    exception => Directive.Escalate);
            Context.ActorOf(Context.DI().Props<ChildActor>().WithSupervisorStrategy(child2SupervisorStrategy)), "child-2");
        }

        protected override SupervisorStrategy SupervisorStrategy() =>
            new OneForOneStrategy( // Default SupervisorStrategy this is passed to child-1
                1,
                1000,
                exception => Directive.Stop);
    }
}
```

The `ResolvedSupervisorStrategy` method returns the `SupervisorStrategy` that was used to create a child. It works for supervisor strategies that are passed to the child in their props and also when the actor under test has its own default `SupervisorStrategy()` method.

Supervisor strategies have a `Decider` property that you can invoke using the `Decide` method by passing in an exception. Using this you can assert whether a child will be restarted or stopped if it throws a certain type of exception:

``` csharp
framework.ResolvedSupervisorStrategy("child-2")
    .As<AllForOneStrategy>().Decider.Decide(new Exception())
    .Should().Be(Directive.Escalate);
```

### Asserting The Correct Messages Are Sent To The Parent

``` csharp
public class Example : TestKit
{
    [Fact]
    public void SutActor_ReceiveStringMessage_SendsUpperCaseStringMessageToParent()
    {
        //arrange
        UnitTestFramework<SutActor> framework = UnitTestFrameworkSettings
            .Empty
            .CreateFramework<SutActor>(this);

        //act
        framework.Sut.Tell("hello world");

        //assert
        framework.Parent.ExpectMsg("HELLO WORLD");
    }

    public class SutActor : ReceiveActor
    {
        public SutActor()
        {
            Receive<string>(s =>
            {
                Context.Parent.Tell(s.ToUpper());
            });
        }
    }
}
```

The `Parent` property returns the `TestProbe` instance that was used as the supervisor to the actor under test.

### Asserting Replies From The Parent Are Processed Correctly

``` csharp
public class Example : TestKit
{
    [Fact]
    public void SutActor_ReceiveSaveMessage_StoresModifiedSaveMessageFromParentInRepo()
    {
        //arrange
        Mock<IRepository> repoMock = new Mock<IRepository>();
        UnitTestFramework<SutActor> framework = UnitTestFrameworkSettings
            .Empty
            .RegisterParentHandler<SutActor.Save>(s => new ParentActor.ModifiedSave(s.Value.ToUpper()))
            .CreateFramework<SutActor>(this, Props.Create(() => new SutActor(repoMock.Object)));

        //act
        framework.Sut.Tell(new SutActor.Save("hello world"));

        //assert
        AwaitAssert(() => repoMock.Verify(repo => repo.Save("HELLO WORLD"), Times.Once));
    }

    public class ParentActor : ReceiveActor // Replaced by a TestProbe by the UnitTestFramework
    {
        public class ModifiedSave // Message that the ParentActor receives
        {
            public string Value { get; }

            public ModifiedSave(string value)
            {
                Value = value;
            }
        }
    }

    public interface IRepository// A dependency injected into SutActor
    {
        void Save(string value);
    }

    public class SutActor : ReceiveActor
    {
        public SutActor(IRepository repo)
        {
            Receive<Save>(s => Context.Parent.Tell(s));
            Receive<ParentActor.ModifiedSave>(s => repo.Save(s.Value));
        }

        public class Save // Message that the SutActor receives
        {
            public string Value { get; }

            public Save(string value)
            {
                Value = value;
            }
        }
    }
}
```

Above, the `RegisterParentHandler` method is used to register a handler against the parent actor:

``` csharp
RegisterParentHandler<SutActor.Save>(s => new ParentActor.ModifiedSave(s.Value.ToUpper()))
```

This example results in the following: The parent `TestProbe`, will receive `SutActor.Save` messages and reply with upper-cased `ParentActor.ModifiedSave` messages. You can then test that the actor under test handles messages from its parent correctly, without having to use the full implementation of the parent.

### Asserting That Exceptions Are Thrown

``` csharp
public class Example : TestKit
{
    [Fact]
    public void SutActor_ReceiveExceptionMessage_ThrowsSameException()
    {
        //arrange
        Exception message = new ArithmeticException();
        UnitTestFramework<SutActor> framework = UnitTestFrameworkSettings
            .Empty
            .CreateFramework<SutActor>(this);

        //act
        framework.TellMessageAndWaitForException(message);

        //assert
        framework.UnhandledExceptions.First().Should().BeSameAs(message);
    }

    public class SutActor : ReceiveActor
    {
        public SutActor()
        {
            Receive<Exception>(message => {
                Thread.Sleep(500);
                throw message;
            });
        }
    }

}
```

The `TellMessageAndWaitForException` method blocks the thread until an exception is thrown. Thrown exceptions are then stored in an `IEnumerable` for you to run assertions against. You can also block until more than one exception is thrown using the  `TellMessageAndWaitForExceptions` method.

### Adding A Delay To A Test That Honours TimeFactor

You can block the current thread by a certain period of time:

``` csharp
UnitTestFramework<SutActor> framework = UnitTestFrameworkSettings
    .Empty
    .CreateFramework<SutActor>(this);

// Sync
framework.Delay(TimeSpan.FromSeconds(1));

// Async
await framework.DelayAsync(TimeSpan.FromSeconds(1));
```

The main benefit of this feature is that it works like the rest of the Akka TestKit by multiplying the given duration by the configured timefactor. The example above blocks the thread by 3 seconds if the given TestKit instance has a `timefactor` of 3. Using this method allows you to modify all of your currently hard-coded delays by the `timefactor`.

### Specifying The Decider

You can specify whether the actor under test should be restarted or stopped when it throws a certain type of exception by providing a decider method when you call the `CreateFramework` method:

``` csharp
UnitTestFramework<SutActor> framework = UnitTestFrameworkSettings
    .Empty
    .CreateFramework<SutActor>(this, ex => Directive.Stop);
```

The `UnitTestFramework` restarts the actor under test by default.

``` csharp
var child = Context.ActorOf(Context.DI().Props<ChildActor>(), "child-1");
```

## Integration testing

The `BasicResolverSettings` class in the package allows you configure `Akka.DI`. This means you can test a series of concrete actors whilst also still being able to limit the scope of your tests to not include every actor in your hierarchy.

### Examples

Here's an example integration test using the resolver:

``` csharp
[Fact]
public void ParentActorReceivesMessage_SendsMessageToChild_ChildSendsMessageToGrandChild_GrandChildSavesMessageInRepo()
{
    //arrange
    Mock<IRepo> repoMock = new Mock<IRepo>();
    BasicResolverSettings
        .Empty
        .RegisterActor<ChildActor>()
        .RegisterActor(() => new GrandChildActor(repoMock.Object))
        .RegisterResolver(this);
    var sut = ActorOfAsTestActorRef<ParentActor>();

    //act
    sut.Tell("hello")

    //assert
    AwaitAssert(() =>
        repoMock.Verify(
            repo => repo.Save("hello"),
            Times.Once()));
}
```

To see some more examples on how to use the `BasicResolverSettings`. See the [examples](AkkaTestingHelpers.MediumTests/BasicResolverSettingsTests/Examples) folder.

## Upgrading To V2

I have removed the versions of this package from NuGet that were versioned by date (for example `2018.3.6.2`) and uploaded a new package with the version `2.0.0`. This is to honour semantic versioning going forward.

### Breaking Changes

There are 2 tiny breaking changes between version `2018.3.6.2` and version `2.0.0`. A quick find and replace should be all that is needed. Now proper versioning is in place upgrades should be smoother in the future.

#### RegisterHandler

The `RegisterHandler` method on the settings class is now called `RegisterChildHandler`.

``` csharp
// Old:
UnitTestFrameworkSettings.Empty.RegisterHandler<ExampleActor, int>(i => i * 2));

// New:
UnitTestFrameworkSettings.Empty.RegisterChildHandler<ExampleActor, int>(i => i * 2));
```

#### Supervisor

The `Supervisor` property has been renamed to `Parent`.

``` csharp
// Old:
framework.Supervisor.Expect("hello world");

// New:
framework.Parent.Expect("hello world");
```

## Installation

> Install-Package ConnelHooley.AkkaTestingHelpers

> dotnet add package ConnelHooley.AkkaTestingHelpers

> paket add ConnelHooley.AkkaTestingHelpers