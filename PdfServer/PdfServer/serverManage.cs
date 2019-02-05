using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace PdfServer
{
    public class serverManage
    {
        // The TcpListener is made public so we can access it from our MainWindow,
        // this way we can issue commands like serverManage.Start() or serverManage.Stop()
        public static TcpListener server;
        StreamWriter sw;
        // Same goes for our boolean online value, otherwise the only way we can stop the thread is by
        // forcefully aborting it via serverThread.Abort(), and that's very messy and usually will crash.
        public static Boolean online = true;
        public void Start()
        {

            //Write a line of text
            sw = new StreamWriter("serverlog.txt");
            sw.WriteLine("Attempting to register TCPListener");
            Console.WriteLine("Attempting to register TCPListener");

            // Hooks up to the 5709 port using it's own address(localhost).
            server = new TcpListener(IPAddress.Any, 5709);

            try
            {
                // Starts up the server.
                server.Start();
            }
            catch(Exception b)
            {
                Console.WriteLine("Server must already be running, proceeding...");
            }
            Console.WriteLine("Server started and running.");
            sw.WriteLine("Server running.");
            sw.WriteLine("Listening...");
            while (online==true)
            {
                // Enables the server to listen and accept connections from a client.
                try
                {
                    // NOTE: The program will wait here until a client connects, once they do it'll proceed to run down the additional commands.
                    // This is where we need to apply multithreading so each client gets their own thread, and as a result, we can have more than
                    // one connection at a time.
                    TcpClient client = server.AcceptTcpClient();
                    sw.WriteLine("Connected to a Client...");

                    // Gets the stream from the client, which should be a handshake
                    // of who is connecting and what is going to be sent.
                    NetworkStream netstream = client.GetStream();
                    sw.WriteLine("Collecting greeting...");

                    while (client.Connected)
                    {
                        // Creates an empty byte array, basically we need to allocate the space of the incoming message, before we download the message.
                        // The car needs a garage before it can pull in!
                        byte[] msg = new byte[32];

                        try
                        {
                            // Do a bit of converting from byte -> String.
                            netstream.Read(msg, 0, msg.Length);
                            string received = Encoding.Default.GetString(msg).Trim();
                            received = received.Substring(0, 32);
                            Console.WriteLine("Received: " + received);
                            sw.WriteLine("Received '" + received + "'.");

                            byte[] response = new byte[100];
                            response = Encoding.Default.GetBytes("Hello Client");
                            netstream.Write(response, 0, response.Length);
                            sw.WriteLine("Sent '" + response + "'.");
                            
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Unknown failure during packet receive.");
                            sw.WriteLine("Client disconnected.");
                            sw.Close();
                        }
                    }
                }
                catch(SocketException a)
                {
                    Console.WriteLine("Service down");
                }
            }           
        }       
    }
}
