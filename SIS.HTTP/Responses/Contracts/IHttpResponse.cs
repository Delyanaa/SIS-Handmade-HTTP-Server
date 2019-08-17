using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;

namespace SIS.HTTP.Responses.Contracts
{
    /*Describe the behavior of a Response object*/
    public interface IHttpResponse
    {
        HttpResponseStatusCode StatusCode { get; set; }
        IHttpHeaderCollection Headers { get; }
        byte[] Content { get; set; }
        void AddHeader(HttpHeader header);
        byte[] GetBytes();
    }
}
