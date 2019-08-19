using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Requests;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.HTTP.Sessions;
using SIS.WebServer.Results;
using SIS.WebServer.Routing.Contracts;
using SIS.HTTP.Cookies;

namespace SIS.WebServer
{
    /*The ConnectionHandler class is the client connection 
     * processor. It receives the connection, extracts the 
     * request string data from it, processes it using the 
     * routing table, and then sends back the Response in a
     * byte format, throughout the TCP link.*/
    public class ConnectionHandler
    {
        private readonly Socket client;
        private readonly IServerRoutingTable serverRoutingTable;

        /* Constructor
         * just initialize the socket 
         * (the wrapper object for a client connection) and
         * the routing table*/
        public ConnectionHandler(Socket client, IServerRoutingTable serverRoutingTable)
        {
            CoreValidator.ThrowIfNull(client, name: nameof(client));
            CoreValidator.ThrowIfNull(serverRoutingTable, name: nameof(serverRoutingTable));

            this.client = client;
            this.serverRoutingTable = serverRoutingTable;
        }

        /* asynchronous 
         * method which contains the main functionality of the class. 
         * It uses the other methods to read the request, handle it, 
         * generate a response, send it to the client, and finally, 
         * close the connection.*/
        public async Task ProcessRequestAsync()
        {
            //IHttpResponse httpResponse = null;
            try
            {
                IHttpRequest httpRequest = await this.ReadRequestAsync();

                if (httpRequest != null)
                {
                    Console.WriteLine($"Processing {httpRequest.RequestMethod}{httpRequest.Path}...");
                    var sessionId = this.SetRequestSession(httpRequest);

                    var httpResponse = this.HandleRequest(httpRequest);
                    this.SetResponseSession(httpResponse, sessionId);
                    await this.PrepareResponse(httpResponse);
                }
            }
            catch (BadRequestException badRequestException)
            {
                await this.PrepareResponse(new TextResult(badRequestException.ToString(), HttpResponseStatusCode.BadRequest));
            }
            catch (Exception exception)
            {
                await this.PrepareResponse(new TextResult(exception.ToString(), HttpResponseStatusCode.InternalServerError));
            }
            /*Disables both receiving and sending on socket*/
            this.client.Shutdown(how: SocketShutdown.Both);
        }

        /* Reads 
         * the byte data from the client connection, extracts
         * the request string data from it, and then maps it
         * to a HttpRequest object.*/
        public async Task<IHttpRequest> ReadRequestAsync()
        {
            StringBuilder result = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                int numberOfBytesRead = await this.client.ReceiveAsync(data, SocketFlags.None);
                if (numberOfBytesRead == 0)  break;

                string bytesAsString = Encoding.UTF8.GetString(data.Array, index: 0, count: numberOfBytesRead);
                    result.Append(bytesAsString);
                if (numberOfBytesRead < 1023) break;
            }

            if (result.Length == 0) return null;

            return new HttpRequest(result.ToString());
        }

        /* Checks 
         * if the routing table has a handler for the given Request,
         * using the Request’s Method and Path.
         *  •	If there is no such handler a “Not Found” Response is returned. 
            •	If there is a handler, its function is invoked, and its resulting
                Response – returned. */
        public IHttpResponse HandleRequest(IHttpRequest httpRequest)
        {
            if(!this.serverRoutingTable.Contains(httpRequest.RequestMethod, httpRequest.Path))
            {
                return new TextResult(content: $"Route with method {httpRequest.RequestMethod} and path \"{httpRequest.Path}\" not found", HttpResponseStatusCode.NotFound);
            }
            return this.serverRoutingTable.Get(httpRequest.RequestMethod, httpRequest.Path)
                .Invoke(httpRequest);
        }

        /*Extracts
         * the byte data from the Response, 
         * and sends it to the client*/
        private async Task PrepareResponse(IHttpResponse httpResponse)
        {
            byte[] byteSegments = httpResponse.GetBytes();
            this.client.Send(byteSegments, SocketFlags.None);
        }

        private string SetRequestSession(IHttpRequest httpRequest)
        {
            string sessionId = null;
            //user already has session 
            if (httpRequest.Cookies.ContainsCookie(HttpSessionStorage.SessionCookieKey))
            {
                var cookie = httpRequest.Cookies.GetCookie(HttpSessionStorage.SessionCookieKey);
                sessionId = cookie.Value;
            }
            else // no generated session 
            {
                sessionId = Guid.NewGuid().ToString();
            }
            httpRequest.Session = HttpSessionStorage.GetSessions(sessionId);

            return httpRequest.Session.Id;
        }

        private void SetResponseSession(IHttpResponse httpResponse, string sessionId)
        {
            //if we have a session
            if (sessionId != null)
            {
                httpResponse.AddCookie(new HttpCookie(HttpSessionStorage.SessionCookieKey, sessionId));
            }
        }
    }
}
