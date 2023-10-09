namespace myJsonParser.Test
{
    public class JsonLexerTest
    {
        // Tokenize returns a list of JsonTokens for a valid JSON input string
        [Fact]
        public void test_tokenize_valid_json_input()
        {
            // Arrange
            string input = "{\"name\":\"John\",\"age\":30,\"city\":\"New York\"}";
            JsonLexer lexer = new JsonLexer(input);

            // Act
            List<JsonToken> tokens = lexer.Tokenize();

            // Assert
            Assert.Equal(13, tokens.Count);
            Assert.Equal(JsonToken.TokenType.LeftCurlyBrace, tokens[0].Type);
            Assert.Equal("{", tokens[0].Value);
            Assert.Equal(JsonToken.TokenType.String, tokens[1].Type);
            Assert.Equal("name", tokens[1].Value);
            Assert.Equal(JsonToken.TokenType.Colon, tokens[2].Type);
            Assert.Equal(":", tokens[2].Value);
            Assert.Equal(JsonToken.TokenType.String, tokens[3].Type);
            Assert.Equal("John", tokens[3].Value);
            Assert.Equal(JsonToken.TokenType.Comma, tokens[4].Type);
            Assert.Equal(",", tokens[4].Value);
            Assert.Equal(JsonToken.TokenType.String, tokens[5].Type);
            Assert.Equal("age", tokens[5].Value);
            Assert.Equal(JsonToken.TokenType.Colon, tokens[6].Type);
            Assert.Equal(":", tokens[6].Value);
            Assert.Equal(JsonToken.TokenType.Number, tokens[7].Type);
            Assert.Equal("30", tokens[7].Value);
            Assert.Equal(JsonToken.TokenType.Comma, tokens[8].Type);
            Assert.Equal(",", tokens[8].Value);
            Assert.Equal(JsonToken.TokenType.String, tokens[9].Type);
            Assert.Equal("city", tokens[9].Value);
            Assert.Equal(JsonToken.TokenType.Colon, tokens[10].Type);
            Assert.Equal(":", tokens[10].Value);
            Assert.Equal(JsonToken.TokenType.String, tokens[11].Type);
            Assert.Equal("New York", tokens[11].Value);
            Assert.Equal(JsonToken.TokenType.RightCurlyBrace, tokens[12].Type);
            Assert.Equal("}", tokens[12].Value);
        }

        // Tokenize ignores whitespace characters and returns correct tokens
        [Fact]
        public void Tokenize_IgnoresWhitespaceCharacters_ReturnsCorrectTokens()
        {
            // Arrange
            string input = "  {  \"name\"  :  \"John\"  ,  \"age\"  :  30  }  ";
            JsonLexer lexer = new JsonLexer(input);

            // Act
            List<JsonToken> tokens = lexer.Tokenize();

            // Assert
            Assert.Equal(9, tokens.Count);
            Assert.Equal(JsonToken.TokenType.LeftCurlyBrace, tokens[0].Type);
            Assert.Equal("{", tokens[0].Value);
            Assert.Equal(JsonToken.TokenType.String, tokens[1].Type);
            Assert.Equal("name", tokens[1].Value);
            Assert.Equal(JsonToken.TokenType.Colon, tokens[2].Type);
            Assert.Equal(":", tokens[2].Value);
            Assert.Equal(JsonToken.TokenType.String, tokens[3].Type);
            Assert.Equal("John", tokens[3].Value);
            Assert.Equal(JsonToken.TokenType.Comma, tokens[4].Type);
            Assert.Equal(",", tokens[4].Value);
            Assert.Equal(JsonToken.TokenType.String, tokens[5].Type);
            Assert.Equal("age", tokens[5].Value);
            Assert.Equal(JsonToken.TokenType.Colon, tokens[6].Type);
            Assert.Equal(":", tokens[6].Value);
        }

        // Tokenize handles nested objects and arrays correctly
        [Fact]
        public void Tokenize_HandlesNestedObjectsAndArrays_ReturnsCorrectTokens()
        {
            // Arrange
            string input = "{\"name\":\"John\",\"age\":30,\"address\":{\"street\":\"123 Main St\",\"city\":\"New York\"},\"hobbies\":[\"reading\",\"gaming\"]}";
            JsonLexer lexer = new JsonLexer(input);

            // Act
            List<JsonToken> tokens = lexer.Tokenize();

            // Assert
            Assert.Equal(29, tokens.Count);
            Assert.Equal(JsonToken.TokenType.LeftCurlyBrace, tokens[0].Type);
            Assert.Equal(JsonToken.TokenType.String, tokens[1].Type);
            Assert.Equal(JsonToken.TokenType.Colon, tokens[2].Type);
            Assert.Equal(JsonToken.TokenType.String, tokens[3].Type);
            Assert.Equal(JsonToken.TokenType.Comma, tokens[4].Type);
            Assert.Equal(JsonToken.TokenType.String, tokens[5].Type);
            Assert.Equal(JsonToken.TokenType.Colon, tokens[6].Type);
            Assert.Equal(JsonToken.TokenType.Number, tokens[7].Type);
            Assert.Equal(JsonToken.TokenType.Comma, tokens[8].Type);
            Assert.Equal(JsonToken.TokenType.String, tokens[9].Type);
            Assert.Equal(JsonToken.TokenType.Colon, tokens[10].Type);
            Assert.Equal(JsonToken.TokenType.LeftCurlyBrace, tokens[11].Type);
            Assert.Equal(JsonToken.TokenType.String, tokens[12].Type);
            Assert.Equal(JsonToken.TokenType.Colon, tokens[13].Type);
            Assert.Equal(JsonToken.TokenType.String, tokens[14].Type);
            Assert.Equal(JsonToken.TokenType.Comma, tokens[15].Type);
            Assert.Equal(JsonToken.TokenType.String, tokens[16].Type);
            Assert.Equal(JsonToken.TokenType.Colon, tokens[17].Type);
            Assert.Equal(JsonToken.TokenType.String, tokens[18].Type);
            Assert.Equal(JsonToken.TokenType.RightCurlyBrace, tokens[19].Type);
            Assert.Equal(JsonToken.TokenType.Comma, tokens[20].Type);
            Assert.Equal(JsonToken.TokenType.String, tokens[21].Type);
            Assert.Equal(JsonToken.TokenType.Colon, tokens[22].Type);
            Assert.Equal(JsonToken.TokenType.LeftSquareBracket, tokens[23].Type);
            Assert.Equal(JsonToken.TokenType.String, tokens[24].Type);
            Assert.Equal(JsonToken.TokenType.Comma, tokens[25].Type);
            Assert.Equal(JsonToken.TokenType.String, tokens[26].Type);
            Assert.Equal(JsonToken.TokenType.RightSquareBracket, tokens[27].Type);
            Assert.Equal(JsonToken.TokenType.RightCurlyBrace, tokens[28].Type);
        }

        // Tokenize throws an exception for invalid JSON input
        [Fact]
        public void Tokenize_InvalidJsonInput_ThrowsException()
        {
            // Arrange
            string input = "{\"name\":\"John\",\"age\":30,\"city\":\"New York\"";
            JsonLexer lexer = new JsonLexer(input);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => lexer.Tokenize());
        }

        // Tokenize handles empty input correctly
        [Fact]
        public void Tokenize_EmptyInput_ReturnsEmptyList()
        {
            // Arrange
            string input = "";
            JsonLexer lexer = new JsonLexer(input);

            // Act
            List<JsonToken> tokens = lexer.Tokenize();

            // Assert
            Assert.Empty(tokens);
        }

        // Tokenize handles input with only whitespace characters correctly
        [Fact]
        public void Tokenize_WhitespaceInput_ReturnsEmptyList()
        {
            // Arrange
            string input = "   \t\n\r";
            JsonLexer lexer = new JsonLexer(input);

            // Act
            List<JsonToken> tokens = lexer.Tokenize();

            // Assert
            Assert.Empty(tokens);
        }
    }
}