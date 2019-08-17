﻿using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Extensions;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using SIS.HTTP.Responses.Contracts;
using System.Text;

namespace SIS.HTTP.Responses
{
    /*Implements the IHttpResponse interface. */
    public class HttpResponse : IHttpResponse
    {
        /* A HttpResponse 
         * is instantiated with an object 
         * with NULL or default values.*/
        public HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
            this.Content = new byte[0]; 
        }

        public HttpResponse(HttpResponseStatusCode statusCode) : this()
        {
            CoreValidator.ThrowIfNull(statusCode, name: nameof(statusCode));
            this.StatusCode = statusCode;
        }
       
        public HttpResponseStatusCode StatusCode { get; set; }
        public IHttpHeaderCollection Headers { get;  }
        public byte[] Content { get; set; }

        public void AddHeader(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, name: nameof(header));
            this.Headers.AddHeader(header);
        }

        /* Converts 
         * the result from the ToString() method to a byte[] array, 
         * and concatenates to it  the Content bytes,
         * thus forming the full Response in byte format*/
        public byte[] GetBytes()
        {
            byte[] httpResponseBytesWithoutBody = Encoding.ASCII.GetBytes(s: this.ToString());
            byte[] httpResponseBytesWithBody = new byte[httpResponseBytesWithoutBody.Length + this.Content.Length];


            for (int i = 0; i < httpResponseBytesWithoutBody.Length; i++)
                httpResponseBytesWithBody[i] = httpResponseBytesWithoutBody[i];
            

            for (int i = 0; i < httpResponseBytesWithBody.Length - httpResponseBytesWithoutBody.Length; i++)
                httpResponseBytesWithBody[i + httpResponseBytesWithoutBody.Length] = this.Content[i];
            

            return httpResponseBytesWithBody;
        }

        /* Forms 
         * the Response line – the line holding the protocol, 
         * the status code and the status, and the Response Headers along with the <CRLF> line. 
         * These properties are concatenated in a string and returned.*/
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result
                .Append($"{GlobalConstants.HttpOneProtocolFragment} {this.StatusCode.GetStatusLine()}").Append(GlobalConstants.HttpNewLine)
                .Append(this.Headers).Append(GlobalConstants.HttpNewLine);
                

            result.Append(GlobalConstants.HttpNewLine);
            return result.ToString();
        }
    }
}