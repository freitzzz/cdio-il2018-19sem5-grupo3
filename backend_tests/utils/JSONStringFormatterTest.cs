using backend.utils;
using Xunit;
using static backend.utils.JSONStringFormatter;

namespace backend_tests.utils
{
    public class JSONStringFormatterTest
    {
        

        [Fact]
        public void ensureFormatMessageToJSONWithErrorMessageTypeWorks(){

            string message = "This is a message";

            string expected = string.Format("{{\n  \"error\": \"{0}\"\n}}", message);

            string result = JSONStringFormatter.formatMessageToJson(MessageTypes.ERROR_MSG, message);

            Assert.Equal(expected, result);
        }
    }
}