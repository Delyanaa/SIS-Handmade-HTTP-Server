using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using System;


namespace SIS.WebServer.Routing.Contracts
{
    /*This class holds a colossal collection of nested dictionaries, 
     * which will be used for routing*/
    public interface IServerRoutingTable
    {
        void Add(HttpRequestMethod method, string path, Func<IHttpRequest, IHttpResponse> func);
        bool Contains(HttpRequestMethod method, string path);
        Func<IHttpRequest, IHttpResponse> Get(HttpRequestMethod method, string path);
    }
}
