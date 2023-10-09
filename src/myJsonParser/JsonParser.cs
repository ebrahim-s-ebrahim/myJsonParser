using System;
using System.Collections.Generic;
using System.Text;

public class JsonParser
{
    private List<JsonToken> tokens;
    private int currentTokenIndex;

    public object ParseJson(string json)
    {
        // Lexing
        var lexer = new JsonLexer(json);
        var tokens = lexer.Tokenize();

        // parsing the tokens
        return Parse(tokens);
    }
    private object Parse(List<JsonToken> tokens)
    {
        this.tokens = tokens;
        this.currentTokenIndex = 0;

        return ParseValue();
    }

    private object ParseValue()
    {
        JsonToken currentToken = PeekToken();

        switch (currentToken.Type)
        {
            case JsonToken.TokenType.String:
                ConsumeToken();
                return currentToken.Value;
            case JsonToken.TokenType.Number:
                ConsumeToken();
                return int.Parse(currentToken.Value);
            case JsonToken.TokenType.True:
                ConsumeToken();
                return true;
            case JsonToken.TokenType.False:
                ConsumeToken();
                return false;
            case JsonToken.TokenType.Null:
                ConsumeToken();
                return null;
            case JsonToken.TokenType.LeftCurlyBrace:
                return ParseObject();
            case JsonToken.TokenType.LeftSquareBracket:
                return ParseArray();
            default:
                throw new InvalidOperationException($"Unexpected token: {currentToken.Type}");
        }
    }

    private Dictionary<string, object> ParseObject()
    {
        Dictionary<string, object> obj = new Dictionary<string, object>();
        HashSet<string> encounteredKeys = new HashSet<string>(); // To track encountered keys

        ConsumeToken(); // Consume the opening curly brace '{'

        while (true)
        {
            JsonToken currentToken = PeekToken();

            if (currentToken.Type == JsonToken.TokenType.RightCurlyBrace)
            {
                ConsumeToken(); // Consume the closing curly brace '}'
                return obj;
            }

            // Parse key-value pairs
            string key = ParseString();

            // Check for duplicate keys
            if (encounteredKeys.Contains(key))
            {
                throw new InvalidOperationException($"Duplicate key found: {key}");
            }

            encounteredKeys.Add(key);

            ConsumeToken(); // Consume the colon ':'
            object value = ParseValue();

            obj[key] = value;

            // Check for a comma ',' to determine if there are more key-value pairs
            if (PeekToken().Type == JsonToken.TokenType.Comma)
            {
                ConsumeToken(); // Consume the comma ','
            }
            else if (PeekToken().Type == JsonToken.TokenType.RightCurlyBrace)
            {
                ConsumeToken(); // Consume the closing curly brace '}'
                return obj;
            }
            else
            {
                throw new InvalidOperationException("Invalid object format");
            }
        }
    }

    private List<object> ParseArray()
    {
        List<object> array = new List<object>();
        ConsumeToken(); // Consume the opening square bracket '['

        while (true)
        {
            JsonToken currentToken = PeekToken();

            if (currentToken.Type == JsonToken.TokenType.RightSquareBracket)
            {
                ConsumeToken(); // Consume the closing square bracket ']'
                return array;
            }

            // Parse array elements
            object value = ParseValue();
            array.Add(value);

            // Check for a comma ',' to determine if there are more elements
            if (PeekToken().Type == JsonToken.TokenType.Comma)
            {
                ConsumeToken(); // Consume the comma ','
            }
            else if (PeekToken().Type == JsonToken.TokenType.RightSquareBracket)
            {
                ConsumeToken(); // Consume the closing square bracket ']'
                return array;
            }
            else
            {
                throw new InvalidOperationException("Invalid array format");
            }
        }
    }

    private string ParseString()
    {
        JsonToken currentToken = PeekToken();
        if (currentToken.Type == JsonToken.TokenType.String)
        {
            ConsumeToken();
            return currentToken.Value;
        }
        else
        {
            throw new InvalidOperationException("Expected a string");
        }
    }

    private JsonToken PeekToken()
    {
        if (currentTokenIndex < tokens.Count)
        {
            return tokens[currentTokenIndex];
        }
        else
        {
            throw new InvalidOperationException("No more tokens to parse");
        }
    }

    private void ConsumeToken()
    {
        if (currentTokenIndex < tokens.Count)
        {
            currentTokenIndex++;
        }
        else
        {
            throw new InvalidOperationException("No more tokens to consume");
        }
    }
}