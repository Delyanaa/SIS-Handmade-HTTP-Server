using SIS.WebServer.Routing.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SIS.WebServer
{
    /*The Server class is the main wrapper class for the TCP connection. 
     * It uses a TcpListener to capture Client connections and then passes 
     * them to the ConnectionHandler, which processes them.*/
    public class Server
    {
        private const string LocalhostIpAddress = "127.0.0.1";
        private readonly int port;
        private readonly TcpListener listener;
        private readonly IServerRoutingTable serverRoutingTable;
        private bool isRunning;

        /*Constructor 
         * initialize the Listener 
         * and the RoutingTable*/
        public Server(int port, IServerRoutingTable serverRoutingTable)
        {
            this.port = port;
            this.listener = new TcpListener(IPAddress.Parse(LocalhostIpAddress), port);

            this.serverRoutingTable = serverRoutingTable;
        }

        /* The main processing of the client connection*/
        /* Instantiate 
         * a new ConnectionHandler for each client connection, 
         * and then we pass the client to the ConnectionHandler, 
         * along with the routing table, 
         * so that the Request can be processed.*/
        private async Task Listen(Socket client)
        {
            var connectionHandler = new ConnectionHandler(client, this.serverRoutingTable);
            connectionHandler.ProcessRequest();
        }

        /*Start 
         * the listening process. The listening process should be
         * asynchronous to ensure concurrent client functionality.*/
        public void Run()
        {
            this.listener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server started at http://{LocalhostIpAddress}:{this.port}");

            while (this.isRunning)
            {
                Console.WriteLine("Waiting for client...");

                /*Accepts the pending connection request*/
                var client = this.listener.AcceptSocket();
                this.Listen(client);
            }
        }
    }
}
