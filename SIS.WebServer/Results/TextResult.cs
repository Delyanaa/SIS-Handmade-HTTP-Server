using SIS.HTTP.Enums;
using SIS.HTTP.Responses;
using SIS.HTTP.Headers;
using System.Text;

namespace SIS.WebServer.Results
{
    /*Designed to hold text contents, this is a simple plain text response. 
     * It should have a Content-Type header – text/plain.*/
    public class TextResult : HttpResponse
    {
        public TextResult(string content, HttpResponseStatusCode responseStatusCode,
            string contentType = "text/plain; charset=utf-8") : base(responseStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader("Content-Type", contentType));
            this.Content = Encoding.UTF8.GetBytes(content);
        }

        public TextResult(byte[] content, HttpResponseStatusCode responseStatusCode,
          string contentType = "text/plain; charset=utf-8") : base(responseStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader("Content-Type", contentType));
            this.Content = content;
        }
    }
}
