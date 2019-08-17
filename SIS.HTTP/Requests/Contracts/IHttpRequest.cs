using SIS.HTTP.Enums;
using SIS.HTTP.Headers.Contracts;
using System.Collections.Generic;

namespace SIS.HTTP.Requests.Contracts
{
    /*Describes the behavior of a Request object.*/
    public interface IHttpRequest
    {
        string Path { get; }
        string Url { get; }
        Dictionary<string, object> FormData { get; }
        Dictionary<string, object> QueryData { get; }
        IHttpHeaderCollection Headers { get; }
        HttpRequestMethod RequestMethod { get; }
    }
}
