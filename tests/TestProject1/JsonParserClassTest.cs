using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myJsonParserTest
{
    public class JsonParserClassTest
    {
        // Parsing a simple JSON object with one key-value pair
        [Fact]
        public void test_parse_simple_object()
        {
            // Arrange
            List<JsonToken> tokens = new List<JsonToken>
            {
                new JsonToken(JsonToken.TokenType.LeftCurlyBrace),
                new JsonToken(JsonToken.TokenType.String, "key"),
                new JsonToken(JsonToken.TokenType.Colon),
                new JsonToken(JsonToken.TokenType.String, "value"),
                new JsonToken(JsonToken.TokenType.RightCurlyBrace)
            };
            JsonParser parser = new JsonParser(tokens);

            // Act
            var result = parser.Parse();

            // Assert
            Assert.IsType<Dictionary<string, object>>(result);
            var obj = (Dictionary<string, object>)result;
            Assert.Single(obj);
            Assert.Equal("value", obj["key"]);
        }

        // Parsing a simple JSON array with one element
        [Fact]
        public void test_parse_simple_array()
        {
            // Arrange
            List<JsonToken> tokens = new List<JsonToken>
            {
                new JsonToken(JsonToken.TokenType.LeftSquareBracket),
                new JsonToken(JsonToken.TokenType.String, "element"),
                new JsonToken(JsonToken.TokenType.RightSquareBracket)
            };
            JsonParser parser = new JsonParser(tokens);

            // Act
            var result = parser.Parse();

            // Assert
            Assert.IsType<List<object>>(result);
            var array = (List<object>)result;
            Assert.Single(array);
            Assert.Equal("element", array[0]);
        }

        // Parsing a JSON object with nested objects and arrays
        [Fact]
        public void test_parse_nested_object()
        {
            // Arrange
            List<JsonToken> tokens = new List<JsonToken>
            {
                new JsonToken(JsonToken.TokenType.LeftCurlyBrace),
                new JsonToken(JsonToken.TokenType.String, "key1"),
                new JsonToken(JsonToken.TokenType.Colon),
                new JsonToken(JsonToken.TokenType.LeftCurlyBrace),
                new JsonToken(JsonToken.TokenType.String, "key2"),
                new JsonToken(JsonToken.TokenType.Colon),
                new JsonToken(JsonToken.TokenType.LeftSquareBracket),
                new JsonToken(JsonToken.TokenType.String, "element1"),
                new JsonToken(JsonToken.TokenType.Comma),
                new JsonToken(JsonToken.TokenType.String, "element2"),
                new JsonToken(JsonToken.TokenType.RightSquareBracket),
                new JsonToken(JsonToken.TokenType.RightCurlyBrace),
                new JsonToken(JsonToken.TokenType.RightCurlyBrace)
            };
            JsonParser parser = new JsonParser(tokens);

            // Act
            var result = parser.Parse();

            // Assert
            Assert.IsType<Dictionary<string, object>>(result);
            var obj = (Dictionary<string, object>)result;
            Assert.Single(obj);
            Assert.IsType<Dictionary<string, object>>(obj["key1"]);
            var nestedObj = (Dictionary<string, object>)obj["key1"];
            Assert.Single(nestedObj);
            Assert.IsType<List<object>>(nestedObj["key2"]);
            var array = (List<object>)nestedObj["key2"];
            Assert.Equal(2, array.Count);
            Assert.Equal("element1", array[0]);
            Assert.Equal("element2", array[1]);
        }

        // Parsing an empty JSON object
        [Fact]
        public void test_parse_empty_object()
        {
            // Arrange
            List<JsonToken> tokens = new List<JsonToken>
            {
                new JsonToken(JsonToken.TokenType.LeftCurlyBrace),
                new JsonToken(JsonToken.TokenType.RightCurlyBrace)
            };
            JsonParser parser = new JsonParser(tokens);

            // Act
            var result = parser.Parse();

            // Assert
            Assert.IsType<Dictionary<string, object>>(result);
            var obj = (Dictionary<string, object>)result;
            Assert.Empty(obj);
        }

        // Parsing an empty JSON array
        [Fact]
        public void test_parse_empty_array()
        {
            // Arrange
            List<JsonToken> tokens = new List<JsonToken>
            {
                new JsonToken(JsonToken.TokenType.LeftSquareBracket),
                new JsonToken(JsonToken.TokenType.RightSquareBracket)
            };
            JsonParser parser = new JsonParser(tokens);

            // Act
            var result = parser.Parse();

            // Assert
            Assert.IsType<List<object>>(result);
            var array = (List<object>)result;
            Assert.Empty(array);
        }

        // Parsing a JSON object with duplicate keys
        [Fact]
        public void test_parse_duplicate_keys()
        {
            // Arrange
            List<JsonToken> tokens = new List<JsonToken>
            {
                new JsonToken(JsonToken.TokenType.LeftCurlyBrace),
                new JsonToken(JsonToken.TokenType.String, "key"),
                new JsonToken(JsonToken.TokenType.Colon),
                new JsonToken(JsonToken.TokenType.String, "value1"),
                new JsonToken(JsonToken.TokenType.Comma),
                new JsonToken(JsonToken.TokenType.String, "key"),
                new JsonToken(JsonToken.TokenType.Colon),
                new JsonToken(JsonToken.TokenType.String, "value2"),
                new JsonToken(JsonToken.TokenType.RightCurlyBrace)
            };
            JsonParser parser = new JsonParser(tokens);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => parser.Parse());
        }
    }
}
