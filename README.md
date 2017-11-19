# AkkaTestingHelpers
This NuGet package offers helper classes for both unit testing and integration testing with Akka.NET.

> Install-Package ConnelHooley.AkkaTestingHelpers

> dotnet add package ConnelHooley.AkkaTestingHelpers

> paket add ConnelHooley.AkkaTestingHelpers

For a detailed explanation as to why I created the package, along with some explained examples, see my [blog post](http://connelhooley.uk/blog/2017/09/30/introducing-akka-testing-helpers-di).

## Unit testing
The `UnitTestFramework` class in the package allows you to test an Actor in full isolation. The framework creates the actor to be tested (referred to by the framework as SUT: System Under Test) with a `TestProbe` as its parent. It also replaces its children with `TestProbe` objects.

It can be used to test the following scenarios:
- That an actor sends the correct messages to its children
- That an actor sends the correct messages to its parent
- That an actor processes replies from its children correctly
- What names an actor gives to its children
- What types an actor creates as its children
- What supervisor strategies an actor creates its children with

The framework replaces children with `TestProbe` objects by using `Akka.DI`. This means you must use IOC in your solution for this to work correctly. If you're not creating your child actors like below then this framework will not be able to replace the children with `TestProbe` objects:

``` csharp
var child = Context.ActorOf(Context.DI().Props<ChildActor>(), "child-1");
```

### Examples
Here's an example unit test using the framework:

``` csharp
[Fact]
public void ParentActor_ReceivesString_SendsChildUpperCaseValue()
{
    //arrange
    var framework = UnitTestFrameworkSettings
        .Empty
        .CreateFramework<ParentActor>(this, 1);

    //act
    framework.Sut.Tell("hello world");

    //assert
    framework
        .ResolvedTestProbe("child-actor-name")
        .ExpectMsg("HELLO WORLD");
}
```

To see some more examples on how to use the `UnitTestFramework`. See the [examples](AkkaTestingHelpers.MediumTests/UnitTestFrameworkTests/Examples) folder. These example are explained in my [blog post](http://connelhooley.uk/blog/2017/09/30/introducing-akka-testing-helpers-di).

### Usage guide
#### Initiating the unit test framework
To create an instance of the `UnitTestFramework` you must first create an `UnitTestFrameworkSettings` object. This is done using the `Empty` property of the settings object:

``` csharp
var settings = UnitTestFrameworkSettings.Empty;
```

The settings object allows you to register message handlers against it. A message handler is a method that is ran when a particular type of child actor receives a particular type of message. The return value of the method is then sent back to the actor that sent the message.

The following example registers a handler that is invoked whenever any children of the type `ExampleActor` receive a message of the type `int`. The handler doubles the `int` and sends it back to the actor who sent it the original message.

``` csharp
var settings = UnitTestFrameworkSettings
    .Empty
    .RegisterHandler<ExampleActor, int>(i => i * 2));
```

You can then create the framework object from the settings object by using the `CreateFramework` method. When creating the framework you must specify the type of actor you wish to test along with a `TestKit` instance. If the actor you wish to test (the SUT actor) does not have a default constructor you must give a `Props` object to create the actor with. If the SUT actor creates children in its constructor you must specify how many children it creates. The `CreateFramework` method only returns once all the children have been created. This will be explained later.

The example below creates a framework with `ParentActor` as the SUT actor and waits for the `ParentActor`'s constructor to create 2 child actors. Note that `this` in the example is an instance of `TestKit`.

``` csharp
var framework = UnitTestFrameworkSettings
    .Empty
    .RegisterHandler<ExampleActor, int>(i => i * 2))
    .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor(), 2));
```

#### Using the unit test framework
> Note: some of these examples use the `FluentAssertions` NuGet package.

Once you have created a framework you can send messages to your SUT actor by using the `Sut` property. The following example sends a message of the type `string` to the sut actor.
``` csharp
framework.Sut.Tell("hello world");
```

If the SUT actor sends messages to its parent/supervisor you can test this like so:
``` csharp
framework.Sut.Tell("hello world");
framework.Supervisor.Expect("hello world");
```

If the SUT actor creates a child actor of the type `ExampleActor` with the name `"child-1"` then you can test this like so:
``` csharp
framework.ResolvedType("child-1").Should().Be<ExampleActor>()
```

> Note: this is the reason the `CreateFramework` waits for children to be resolved before returning. If it did not then the child may not had been resolved when we did our assertion.

If the SUT actor sends `int` messages to a child actor with the name `"child-2"` then you can test this like so:
``` csharp
framework.Sut.Tell(5);
framework.ResolvedTestProbe("child-2").ExpectMsg(5);
```

If the SUT actor creates a child with a `SupervisorStrategy` of the type `OneForOneStrategy` with a retry limit of 5, then you can test this like so:
``` csharp
framework.Sut.Tell(5);
framework.ResolvedSupervisorStrategy("child-1")
    .As<OneForOneStrategy>().MaxNumberOfRetries
    .Should().Be(5);
```

> If a child actor is created using a `Props` object that specifies a `SupervisorStrategy` then that will be returned (E.g. `Context.DI.Props<ChildActor>().WithSupervisorStrategy(new AllForOneStrategy(exception => Directive.Escalate))`). If it is not, the private SupervisorStrategy property of the SUT is returned (E.g. `Context.DI.Props<ChildActor>()`)

If the SUT actor creates `2` new children when it receives a `string` message, you can wait for those children to be created like so:

``` csharp
framework.TellMessageAndWaitForChildren("hello", 2);
```

This means you can then go on use the `ResolvedType`, `ResolvedTestProbe` and `ResolvedSupervisorStrategy` methods safely knowing the new actors have been created.

## Integration testing
The `BasicResolverSettings` class in the package allows you configure `Akka.DI`.  This means you can test a series of concrete actors whilst also still being able to limit the scope of your tests to not include every actor in your hierarchy.

###Examples
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

### Usage guide
#### Initiating the resolver
To register the resolver you must first configure one using the `BasicResolverSettings` object. This is done using the `Empty` property of the settings object:

``` csharp
var settings = BasicResolverSettings.Empty;
```

The settings object allows you to register actor factories against it. An actor factory is a method that creates an Actor. When registering an actor factory you must specify which actor type you want to assign the factory against. The type of actor you register the factory against, and the type of actor the factory returns, do not have to be the same.

The example below registers a factory that returns a `StubExampleActor` whenever an actor asks `Akka.DI` for an instance of `ExampleActor`:

``` csharp
BasicResolverSettings
    .Empty
    .RegisterActor<ExampleActor>(() => new StubExampleActor());
```

You can then register the resolver from the settings object by using the `RegisterResolver` method. When registering the resolver you must pass in a `TestKit` instance to register the resolver against.

The example below registers a resolver. Note that `this` in the example is an instance of `TestKit`.

``` csharp
BasicResolverSettings
    .Empty
    .RegisterActor<ExampleActor>(() => new StubExampleActor())
    .RegisterResolver(this);
```

Once the resolver is registered any calls to `Context.DI().Props<ExampleActor>()` will be intercepted by the factory that was registered and an `StubExampleActor` will be constructed.