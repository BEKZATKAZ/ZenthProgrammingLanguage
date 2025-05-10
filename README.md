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
### Example Code
I showed class definition, object creation/deletion, class inheritance and polymorphism. Note that Zenth doesn't support **Garbage Collection** so far, and all the objects need to be managed manually.
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
I made 2 example projects in **examples/** to showcase what the language can do.
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

Note that constructors don't require an attribute and can be freely used in this language. Also all values or method arguments must implement or be **IValue**, as my language does not work with regular classes or structs such as **bool**, **float**, **string**, so on.

## How it's Made
### A bit of backstory
This is my third custom programming language and I started this project a while ago, and it's the most optimized and clean version so far, good enough to finally publish. It's written in pure C# using .NET 9.0 and follows the Onion Architecture with a little adjustments. At the time, I was just learning the Onion Architecture and decided to challenge myself with this idea of creating a programming language. The project was buggy and messy at first, and I eventually dropped it. But a few days ago, I came back, fixed most of the bugs, almost completely changed the architecture, and added optimization with new features.

### Pseudo-Compiler
I wouldn't call it a real compiler. It just reads the code and wraps it into classes such as **IfStatement**, **ObjectCreation**, **MemberPath** and so on. It doesn't interpret character-by-character at runtime, since that would be too CPU-intensive.

## License
Licensed under the MIT License. See **LICENSE** for details.
