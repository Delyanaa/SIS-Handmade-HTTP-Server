using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;


namespace SIS.WebServer.Results
{
    /*Designed to hold NO CONTENT, and its only purpouse is to redirect the client. 
     * This Response has a location though. Its status is predefined also. 
     * It has status – SeeOther.*/
    public class RedirectResult: HttpResponse
    {
        public RedirectResult(string location): base(HttpResponseStatusCode.SeeOther)
        {
            this.Headers.AddHeader(new HttpHeader($"Location", location)); 
        }
    }
}
