using SIS.WebServer;
using SIS.WebServer.Routing.Contracts;
using SIS.WebServer.Routing;
using SIS.HTTP.Enums;
using Demo.App.Controllers;

namespace Demo_App
{
    class Program
    {
        static void Main(string[] args)
        {
            

            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            
            serverRoutingTable.Add(HttpRequestMethod.Get, "/", httpRequest
                => new HomeController().Home(httpRequest));

            Server server = new Server(12345, serverRoutingTable);
            server.Run();
        }
    }
}
