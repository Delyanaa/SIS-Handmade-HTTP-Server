using System;
using System.Collections.Generic;
using System.Linq;
using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Cookies.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Sessions;
using SIS.HTTP.Sessions.Contracts;

namespace SIS.HTTP.Requests
{
    public class HttpRequest : IHttpRequest
    {
        /* HttpRequest 
         * holds its Path, Url, RequestMethod, Headers, Data etc. 
         * Those things come from the requestString,
         * which is passed to its constructor.*/
        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public IHttpCookieCollection Cookies { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public IHttpSession Session { get; set; }

        /* Checks 
         * if the split requestLine holds exactly 3 elements,
         * and if the 3rd element is equal to “HTTP/1.1”. 
         * Returns a boolean result.
         * 
         * requestLine example: POST/home/index HTTP 1.1/ */
        private bool IsValidRequestLine(string[] requestLineParams)
        {
            if (requestLineParams.Length != 3
                || requestLineParams[2] != GlobalConstants.HttpOneProtocolFragment)
            {
                return false;
            }
            return true;
        }

        /* Used 
         * in the ParseQueryParameters() method. 
         * It checks if the Query string is NOT NULL or empty and 
         * if there is at least 1 or more queryParameters.*/
        private bool IsValidRequestQueryString(string queryString, string[] queryParameters)
        {
            CoreValidator.ThrowIfNullOrEmpty(queryString, nameof(queryString));

            return true; //TODO: REGEX QUERY STRING
        }

        private bool HasQueryString()
        {
            return this.Url.Split('?').Length > 1;
        }

        private IEnumerable<string> ParsePlainRequestHeaders(string[] requestLines)
        {
            for (int i = 1; i < requestLines.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(requestLines[i]))
                {
                    yield return requestLines[i];
                }
            }
        }

        /* Sets 
         * the Request’s Method, by parsing the 1st element 
         * from the split requestLine*/
        private void ParseRequestMethod(string[] requestLineParams)
        {
            bool parseResult = HttpRequestMethod.TryParse(requestLineParams[0], true,
                out HttpRequestMethod method);

            if (!parseResult)
            {
                throw new BadRequestException(
                    string.Format(GlobalConstants.UnsupportedHTTPMethodExceptionMessage,
                        requestLineParams[0]));
            }

            this.RequestMethod = method;
        }

        /* Sets 
         * the Request’s Url to the 2nd element 
         * from the split requestLine.*/
        private void ParseRequestUrl(string[] requestLineParams)
        {
            this.Url = requestLineParams[1];
        }

        /* Sets 
         * the Request’s Path, by splitting the Request’s Url 
         * and taking only the path from it.*/
        private void ParseRequestPath()
        {
            this.Path = this.Url.Split('?')[0];
        }

        /* Skips 
         * the first line (the request line), 
         * traverses the request lines until it reaches an empty line (the <CRLF> line). 
         * Each line represents a header, which is split and parsed. 
         * Then the string data is mapped to an HttpHeader object, and the object itself 
         * is added to the Headers property of the Request.
         * Throws a BadRequestException if there is no “Host” Header present after the parsing. */
        private void ParseRequestHeaders(string[] plainHeaders)
        {
            plainHeaders.Select(plainHeader => plainHeader.Split(new[] { ": " },
                 StringSplitOptions.RemoveEmptyEntries))
                .ToList()
                .ForEach(headerKeyValuePair => this.Headers.AddHeader(new HttpHeader
                            (headerKeyValuePair[0], headerKeyValuePair[1])));
        }

        private void ParseCookies()
        {
            if (this.Headers.ContainsHeader(HttpHeader.Cookie))
            {
                string value = this.Headers.GetHeader(HttpHeader.Cookie).Value;
                string[] unparsedCookies = value.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries);

                foreach(string unparsedCookie in unparsedCookies)
                {
                    string[] cookieKeyValuePair = unparsedCookie.Split(new[] { '=' }, 2);

                    HttpCookie httpCookie = new HttpCookie(cookieKeyValuePair[0],
                        cookieKeyValuePair[1], false);

                    this.Cookies.AddCookie(httpCookie);
                }
            }
        }

        //URL: users/profile?name="pesho"&id="asd"#fragment 

        /* Extracts 
         * the Query string, by splitting the Request’s Url and taking only the query from it.
         * Then splits the Query string into different parameters, and maps each of them into the 
         * Query Data Dictionary.
         * Validates the Query string and parameters by calling the IsValidrequestQueryString() method.
         * Does nothing if the Request’s Url contains NO Query string.
         * Throws a BadRequestException if the Query string is invalid.*/
        private void ParseRequestQueryParameters()
        {
            if (this.HasQueryString())
            {
                this.Url.Split('?', '#')[1]
                    .Split('&')
                    .Select(plainQueryParameter => plainQueryParameter.Split('='))
                    .ToList()
                    .ForEach(queryParameterKeyValuePair =>
                        this.QueryData.Add(queryParameterKeyValuePair[0], queryParameterKeyValuePair[1]));
            }
        }

        /* Splits
         * the Request’s Body into different parameters, and   
         * maps each of them into the Form Data Dictionary.
         * Does nothing if the Request contains NO Body.*/
        private void ParseRequestFormDataParameters(string requestBody)
        {
            if (!string.IsNullOrEmpty(requestBody))
            {
                //TODO: Parse Multiple Parameters By Name
                requestBody
                    .Split('&')
                    .Select(plainQueryParameter => plainQueryParameter.Split('='))
                    .ToList()
                    .ForEach(queryParameterKeyValuePair =>
                        this.FormData.Add(queryParameterKeyValuePair[0], queryParameterKeyValuePair[1]));
            }
        }

        /* Invokes 
         * the ParseQueryParameters() and the ParseFormDataParameters() methods. 
         * Just a wrapping method.
         * You should be able to parse even complex requests with no problems.*/
        private void ParseRequestParameters(string requestBody)
        {
            this.ParseRequestQueryParameters();
            this.ParseRequestFormDataParameters(requestBody); //TODO: Split
        }

        private void ParseRequest(string requestString)
        {
            string[] splitRequestString = requestString
                .Split(new[] { GlobalConstants.HttpNewLine }, StringSplitOptions.None);

            string[] requestLineParams = splitRequestString[0]
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLineParams))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLineParams);
            this.ParseRequestUrl(requestLineParams);
            this.ParseRequestPath();

            this.ParseRequestHeaders(this.ParsePlainRequestHeaders(splitRequestString).ToArray());
            this.ParseCookies();

            this.ParseRequestParameters(splitRequestString[splitRequestString.Length - 1]);
        }
    }
}
