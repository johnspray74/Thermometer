<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#about-the-project">About the project</a></li>
    <ul>
        <li><a href="#The-application"> The application</a></li>
        <li><a href="#How-it-works">How it works</a></li>
    </ul>
    <li><a href="#To-run-the-example-application">To run the example application</a></li>
    <li><a href="#Built-with">Built with</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <ul>
        <li><a href="#Future-work">Future work</a></li>
    </ul>
    <li><a href="#Authors">Authors</a></li>
    <li><a href="#license">License</a></li>
  </ol>
</details>


# About the project

The purpose of this project is example code for Chapter Two of the ALA website ([Abstraction Layered Architecture](AbstractionLayeredArchitecture.md)).

It's not about what the application itself does, it's about how all the code mechanics of an application conforming to this architecture work.

### The application

The example is in the form a fairly minimal Thermometer console application using one programming paradigm (dataflow) and a handful of domain abstractions.

It uses the console for output, and a simulated ADC (analog to digital converter) hardware as it's data source.


<!---
![Application diagram](Application/Application-diagram.png)
--->

### How it works

Some knowledge of ALA itself is needed to understand the architecture of the code.
A brief explanation of ALA can be read here ([Abstraction Layered Architecture.md](AbstractionLayeredArchitecture.md)).
An in-depth explanation of ALA and theory can be found in the web site <http://www.abstractionlayeredarchitecture.com>.

Regardless of the theory behind the organisation of the code into folders, the code itself is really quite simple.

The *Application* folder contains Application.cs that instantiates domain abstractions (like lego building blocks) and wires them together to create a Thermometer. No other code knows its part of a thermometer. 

The constructor in Application.cs instantiates classes in the *DomainAbstractions* folder. It configures them with any details from according to the requirements of the thermometer via constructor parameters or properties. Then it wires them together using interfaces in the *ProgrammingParadigms* folder. When a pair of objects is wired, one object has a field of the type of the interface and the other implements the interface. On both instances these are called ports.

The wiring is done by a *WireTo* operator, which is an extension method in the *Foundation* folder. WireTo uses reflection to find matching ports. It then assigns the second object, casted as the interface, to the field in the first object. This is like dependency injection without using constructor parameters or setters. The dependency injection is directed by the Application.cs. We say *like* dependeny injection, because they are not really dependencies. The domain abstractions do not have dependencies on each other in any way, not even an abstract base class or an API like interface that can have multiple implementations. They only depend on their own ports. The actual dependency is on an abstract interface which we think of as at the abstraction level of a programming paradigm.

Once all the instances of domain abstractions are wired according to the diagram, they can communicate at run-time because they have ordinary fields with references to each other. For example IDataflow<T> is used everywhere that a piece of data needs to be pushed from one instance to another. Other programming paradigms are usually used, but in this example we only use one to keep things minimal.
  
Peruse the code to see how the dataflow programming paradigm interface works, and how some of the domain abstractions work using it for ports. You will find the domain abstractions are like independent programs to read and understand because they depend only on the programming paradigms. 
   
  
## To run the example application

1. Clone this repository or download as a zip.
2. Open the solution in Visual Studio 2022 or later (this example uses arithmetic in generic types which is at the time of writing  only supported in .NET 6 Preview 7.)
3. When the application runs, you will see temperature data being displayed which comes from the simulated real device.

## Built with

C#, Visual Studio 2022 (and .NET 6 Preview 7)


## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the project using the button at top right of the main Github page or (<https://github.com/johnspray74/ALAExample/fork>)
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -am 'Add AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request


### Future work

Swift, Java, Python and Rust versions needed.


We would love help to further develop explore ALA.


## Authors

John Spray

### Contact

John Spray - johnspray274@gmail.com



## License

This project is licensed under the terms of the MIT license. See [License.txt](License.txt)

[![GitHub license](https://img.shields.io/github/license/johnspray74/ALAExample)](https://github.com/johnspray74/ALAExample/blob/master/License.txt)

## Acknowledgments


