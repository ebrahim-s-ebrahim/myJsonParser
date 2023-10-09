using System.Text;

public class JsonLexer
{
    private string input;
    private int position;
    private int braceDepth = 0;
    private int bracketDepth = 0;

    public JsonLexer(string input)
    {
        this.input = input;
        this.position = 0;
    }

    public List<JsonToken> Tokenize()
    {
        List<JsonToken> tokens = new List<JsonToken>();

        while (position < input.Length)
        {
            char currentChar = input[position];

            switch (currentChar)
            {
                case '"':
                    tokens.Add(ScanString());
                    break;
                case '0' or '1' or '2' or '3' or '4' or '5' or '6' or '7' or '8' or '9' or '-' or '.' or 'e':
                    tokens.Add(ScanNumber());
                    break;
                case 't':
                    tokens.Add(ScanTrue());
                    break;
                case 'f':
                    tokens.Add(ScanFalse());
                    break;
                case 'n':
                    tokens.Add(ScanNull());
                    break;
                case '{':
                    tokens.Add(new JsonToken(JsonToken.TokenType.LeftCurlyBrace, "{"));
                    position++;
                    braceDepth++;
                    break;
                case '}':
                    tokens.Add(new JsonToken(JsonToken.TokenType.RightCurlyBrace, "}"));
                    position++;
                    braceDepth--;
                    break;
                case '[':
                    tokens.Add(new JsonToken(JsonToken.TokenType.LeftSquareBracket, "["));
                    position++;
                    bracketDepth++;
                    break;
                case ']':
                    tokens.Add(new JsonToken(JsonToken.TokenType.RightSquareBracket, "]"));
                    position++;
                    bracketDepth--;
                    break;
                case ':':
                    tokens.Add(new JsonToken(JsonToken.TokenType.Colon, ":"));
                    position++;
                    break;
                case ',':
                    tokens.Add(new JsonToken(JsonToken.TokenType.Comma, ","));
                    position++;
                    break;
                case ' ' or '\t' or '\n' or '\r':
                    // Ignore whitespace
                    position++;
                    break;
                default:
                    throw new InvalidOperationException($"Invalid character: {currentChar}");
            }
        }
        if (braceDepth > 0)
        {
            throw new InvalidOperationException("Unmatched opening curly brace");
        }
        if (bracketDepth > 0)
        {
            throw new InvalidOperationException("Unmatched opening square bracket");
        }
        return tokens;
    }

    private JsonToken ScanString()
    {
        StringBuilder value = new StringBuilder();
        position++; // Skip the opening double quote

        while (position < input.Length)
        {
            char currentChar = input[position];

            if (currentChar == '"')
            {
                position++; // Skip the closing double quote
                return new JsonToken(JsonToken.TokenType.String, value.ToString());
            }
            else if (currentChar == '\\')
            {
                // Handle escape sequences if needed
                if (position + 1 < input.Length)
                {
                    value.Append(input[position]);
                    value.Append(input[position + 1]);
                    position += 2;
                }
                else
                {
                    throw new InvalidOperationException("Invalid escape sequence");
                }
            }
            else
            {
                value.Append(currentChar);
                position++;
            }
        }

        throw new InvalidOperationException("Unterminated string");
    }

    private JsonToken ScanNumber()
    {
        int start = position;
        while (position < input.Length && "e0123456789-.".Contains(input[position]))
        {
            position++;
        }

        string number = input.Substring(start, position - start);
        return new JsonToken(JsonToken.TokenType.Number, number);
    }

    private JsonToken ScanTrue()
    {
        if (input.Length - position >= 4 && input.Substring(position, 4) == "true")
        {
            position += 4;
            return new JsonToken(JsonToken.TokenType.True, "true");
        }

        throw new InvalidOperationException("Invalid token for 'true'");
    }

    private JsonToken ScanFalse()
    {
        if (input.Length - position >= 5 && input.Substring(position, 5) == "false")
        {
            position += 5;
            return new JsonToken(JsonToken.TokenType.False, "false");
        }

        throw new InvalidOperationException("Invalid token for 'false'");
    }

    private JsonToken ScanNull()
    {
        if (input.Length - position >= 4 && input.Substring(position, 4) == "null")
        {
            position += 4;
            return new JsonToken(JsonToken.TokenType.Null, "null");
        }

        throw new InvalidOperationException("Invalid token for 'null'");
    }
}
