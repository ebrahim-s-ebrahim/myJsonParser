using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myJsonParser.Test
{
    public class JsonParserClassTest
    {
        // Parse a simple JSON object with one key-value pair
        [Fact]
        public void test_parse_simple_object()
        {
            // Arrange
            string json = "{\"key\": \"value\"}";
            JsonParser parser = new JsonParser();

            // Act
            var result = parser.ParseJson(json);

            // Assert
            Assert.IsType<Dictionary<string, object>>(result);
            var obj = (Dictionary<string, object>)result;
            Assert.Single(obj);
            Assert.Equal("value", obj["key"]);
        }

        // Parse a simple JSON array with two elements
        [Fact]
        public void test_parse_simple_array()
        {
            // Arrange
            string json = "[3, 2]";
            JsonParser parser = new JsonParser();

            // Act
            var result = parser.ParseJson(json);

            // Assert
            Assert.IsType<List<object>>(result);
            var array = (List<object>)result;
            Assert.Equal(2, array.Count);
            Assert.Equal(3, array[0]);
            Assert.Equal(2, array[1]);
        }

        // Parse a JSON object with nested objects and arrays
        [Fact]
        public void test_parse_nested_object_and_array()
        {
            // Arrange
            string json = "{\"key\": [3, {\"nested_key\": \"nested_value\"}]}";
            JsonParser parser = new JsonParser();

            // Act
            var result = parser.ParseJson(json);

            // Assert
            Assert.IsType<Dictionary<string, object>>(result);
            var obj = (Dictionary<string, object>)result;
            Assert.Single(obj);
            Assert.IsType<List<object>>(obj["key"]);
            var array = (List<object>)obj["key"];
            Assert.Equal(2, array.Count);
            Assert.Equal(3, array[0]);
            Assert.IsType<Dictionary<string, object>>(array[1]);
            var nestedObj = (Dictionary<string, object>)array[1];
            Assert.Single(nestedObj);
            Assert.Equal("nested_value", nestedObj["nested_key"]);
        }

        // Parse an empty JSON object
        [Fact]
        public void test_parse_empty_object()
        {
            // Arrange
            string json = "{}";
            JsonParser parser = new JsonParser();

            // Act
            var result = parser.ParseJson(json);

            // Assert
            Assert.IsType<Dictionary<string, object>>(result);
            var obj = (Dictionary<string, object>)result;
            Assert.Empty(obj);
        }

        // Parse an empty JSON array
        [Fact]
        public void test_parse_empty_array()
        {
            // Arrange
            string json = "[]";
            JsonParser parser = new JsonParser();

            // Act
            var result = parser.ParseJson(json);

            // Assert
            Assert.IsType<List<object>>(result);
            var array = (List<object>)result;
            Assert.Empty(array);
        }

        // Parse a JSON object with duplicate keys
        [Fact]
        public void test_parse_duplicate_keys()
        {
            // Arrange
            string json = "{\"key\": \"value1\", \"key\": \"value2\"}";
            JsonParser parser = new JsonParser();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => parser.ParseJson(json));
        }
    }
}
