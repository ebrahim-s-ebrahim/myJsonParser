# myJsonParser

Simple.Json.Parser is a simple JSON parser library for .NET.

## Features

- Parse JSON into dictionaries and lists

- Handles nested objects and arrays

- Validates JSON syntax

- Lightweight and easy to use

## Getting Started

Install the latest version of the JsonParser package from NuGet:

```

dotnet add package Simple.Json.Parser

```

Parse a JSON string:

```csharp

var json = @"{'name': 'John', 'age': 30}";

var parser = new JsonParser();

var data = parser.ParseJson(json);

Console.WriteLine(data["name"]); // prints "John"

```

## Usage

The main entry point is the JsonParser class.

- To parse JSON, call ParseJson() with the JSON string.

- This returns a Dictionary<string, object> for parsed JSON objects, and List<object> for JSON arrays.

- Primitive values like strings, numbers, and booleans are represented as usual .NET types.

The parser will throw exceptions for invalid JSON syntax. Use a try/catch block to handle errors gracefully:

```csharp

try {

var data = parser.ParseJson(json);

} catch (JsonParseException ex) {

// invalid JSON

}

```
