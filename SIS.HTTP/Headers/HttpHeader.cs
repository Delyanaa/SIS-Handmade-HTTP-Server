using SIS.HTTP.Common;

namespace SIS.HTTP.Headers
{
    /*Store data about a HTTP Request / Response Header.*/
    public class HttpHeader
    {
        public const string Cookie = "Cookie";

        public HttpHeader(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(text:key, name: nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(text: value, name: nameof(value));
            this.Key = key;
            this.Value = value;
        }

        /*Key 
         * will be the header’s name, 
         * and the Value – its value.*/
        public string Key { get; }
        public string Value { get; }

        /*ToString()
         * brings a well-formatted web ready 
         * (it can be used in web communication without further formatting) 
         * string representation of the header.*/
        public override string ToString()
        {
            return $"{this.Key}: {this.Value}";
        }
    }
}
