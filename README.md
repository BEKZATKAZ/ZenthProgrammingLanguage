# Zenth Programming Language
A minimal, JavaScript-inspired interpreted language, following the Onion Architecture with a little adjustments. Lightweight, easily customizable via **cfg/**, and good for small scripting tasks.

### Key Features
- Compilable/Executable code
- Extendable codebase with C# code integration
- Configurable keywords via **cfg/**

### Prerequisites
- .NET 9.0 or higher
- Windows 10 or higher

## Keyword Configuration
Customize keywords in **cfg/** with a simple key-value syntax, similar to **INI** or related to **JSON**, where both keys and values are strings.

### Syntax Example
```ini
"VARIABLE" = "var"
"FUNCTION" = "function"
"RETURN" = "return"
```

Then the values are assigned to the keys, in other words, to the actual C# properties via reflection. The language has not so many, but enough keywords to program things. Note that the code must define a **Program** class and a constructor to run the program.

## Coding
Below I showed class definition, object creation/deletion, class inheritance and polymorphism. So far, Zenth doesn't support **Garbage Collection**, and all objects must be managed manually.

### Example Code
Note that the following code uses the default syntax, which can be configured in **cfg/**.
```js
import zenth

class Person
{
    local var name
    local var age
    
    function construct(_name : auto, _age : number)
    {
        name = _name
        age = _age
    }

    function getInfo()
    {
        return "Name: " + name + ", Age: " + age
    }
}

class Employee : Person
{
    local var job
    
    function construct(_name : auto, _age : number, _job : auto)
    {
        base.construct(_name, _age)
        job = _job
    }

    override function getInfo()
    {
        return (base.getInfo() + ", Job: " + job)
    }
}

class Program
{
    function construct()
    {
        var person1 = new Person("Alex", 20)
        var person2 = new Employee("Robert", 36, "Senior Software Engineer")
        var person3 = person1

        var console = new Console()
        console.printLine(person1.getInfo())
        console.printLine(person2.getInfo())
        console.printLine(person3.getInfo())

        delete person1
        delete person2
    }
}
```
### Example Projects
I made 2 example projects in **examples/** to showcase what Zenth is capable of doing.
- Conway's Game Of Life
- Short C#/.NET Quiz

### Known Bugs
The language is not fully released, and has some uncritical bugs. Here are the key known bugs:

Getting values from non 1-dimentional arrays will only get the first dimention's value.
```js
// will not work
data[0][1] = value

// will work
var row = data[0]
row[1] = value
```

Expressions might not compile if both operands and operators are written together.
```js
var value = 10*-10+100 // will not work
var value = 10 * -10 + 100 // will work
```

Block statements will not compile if not written as in C#.
```js
// will not work
if (value >= 0) {
    return 0
}

// will work
if (value >= 0)
{
    return 0
}
```

## Extending
You may extend the codebase in **Zenth.Library**, by adding new classes that can later be used in this language. It provides a few custom attributes that must be applied, so the **Exporter** will know what class to intergrate.

### Attributes
- **PackageMember(name, path):** for classes, define the name and optionally the path.
- **MemberFunction(name):** for methods
- **MemberVariable(name):** for variables and properties

### Usage Example
```js
// Uses Vector2Struct from Zenth.Library
var vector = new vec2(0.1, 50)
var xy = vector.x + vector.y
var mag = vector.magnitude
```

Note that constructors don't require an attribute and can be freely used in Zenth. Also all values and method arguments must implement or be **IValue**, as my language does not work with regular classes or structs.

## How It Works
It's written in pure C# using .NET 9.0.

### A bit of backstory
This is my third custom programming language and I started this project a while ago, and it's the most optimized and clean version so far, good enough to finally publish. At the time, I was just learning the Onion Architecture and decided to challenge myself with this idea of creating a programming language. The project was buggy and messy at first, and I eventually dropped it. But a few days ago, I came back, fixed most of the bugs, almost completely changed the architecture, and added optimization with new features.

### Pseudo-Compiler
I wouldn't call it a real compiler. It just reads the code and wraps it into classes such as **IfStatement**, **ObjectCreation**, **MemberPath** and so on. It doesn't interpret character-by-character at runtime, since that would be too CPU-intensive.

### Architecture
As mentioned earlier, it uses the Onion Architecture, but with a little adjustments. I preferred using **ReadOnlySpan of chars** over **string**, as a bunch of text is managed by the compiler, and I don't want extra memory allocation. I used custom extensions for dictionaries for faster execution and alternate lookups for no need to convert a **ReadOnlySpan of chars** into **string**.

### Projects
- Zenth.Core: Independent project that defines all the coding wrappers and necessary interfaces
- Zenth.Infrastructure: Works with the core project, manages data, contexts and databases, handles functions and variables
- Zenth.Application: Surprisingly not many things here. It just compiles everything and works with the core project
- Zenth.Api: The main project that injects dependencies, applies configuration, and manages the console before compilation
- Zenth.Library: Provides a possibility to extend the codebase with C# code integration via pure reflection

## License
Licensed under the MIT License. See **LICENSE** for details.
