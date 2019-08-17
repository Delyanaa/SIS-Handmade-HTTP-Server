using SIS.HTTP.Enums;
using SIS.HTTP.Responses;
using SIS.HTTP.Headers;
using System.Text;

namespace SIS.WebServer.Results
{
    /* A simple HTML response,
     * with which we can return HTML pages or just simple messages.*/
    public class HtmlResult : HttpResponse
    {
        public HtmlResult(string content, HttpResponseStatusCode responseStatusCode) 
            : base(responseStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader("Content-Type", "text/html; charset=utf-8"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
