using SIS.HTTP.Common;
using SIS.HTTP.Headers.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace SIS.HTTP.Headers
{
    /* Implements the IHttpHeaderCollection interface.
     * The class is a Repository-like class. 
     * It holds a Dictionary collection of Headers and implements the
        interface’s methods*/
    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> httpHeaders;
        public HttpHeaderCollection()
        {
            this.httpHeaders = new Dictionary<string, HttpHeader>();
        }

        /* Adds
         * the header to the Dictionary collection with key – 
         * the key of the Header, and value – the Header.*/
        public void AddHeader(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, name: nameof(header));
            this.httpHeaders.Add(header.Key, header);
        }

        /*Why Dictionary?
         * Fast search using the Dictionary’s hashtable. 
         * Returns a boolean result depending on weather 
         * the collection contains a Header with the given key.*/
        public bool ContainsHeader(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(text:key , name: nameof(key));
            return this.httpHeaders.ContainsKey(key);
        }

        /* Retrieves 
         * from the collection and returns the Header with the given key, if present. 
         * If there is NO such Header, the method should return null.*/
        public HttpHeader GetHeader(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(text: key, name: nameof(key));
            return this.httpHeaders[key];
        }

        /* Returns 
         * all of the Headers’ string representations, 
         * separated by new line (“/r/n”). or
         * Environment.NewLine.*/
        public override string ToString() => string.Join(separator: "\r\n",
            this.httpHeaders.Values.Select(header => header.ToString()));
    }
}
