public class JsonToken
{
    public enum TokenType
    {
        String,
        Number,
        True,
        False,
        Null,
        LeftCurlyBrace,
        RightCurlyBrace,
        LeftSquareBracket,
        RightSquareBracket,
        Colon,
        Comma
    }

    public TokenType Type { get; set; }
    public string Value { get; set; }

    public JsonToken(TokenType type, string value=null)
    {
        Type = type;
        Value = value;
    }
}