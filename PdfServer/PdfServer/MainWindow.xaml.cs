using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace PdfServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Defines the thread variables so we can reference our server from
        // other methods inside this file.
        serverManage servM = new serverManage();
        Thread serverThread = null;

        public MainWindow()
        {
            // Makes the graphical window display. We leave just this in here,
            // if we add anything else, the order of running functions gets offset and
            // we lose all graphical capabilities.
            InitializeComponent();
        }

        private void Server_Start(object sender, RoutedEventArgs e)
        {
            // Starts a new thread (which is a a class of serverManage.cs) and then calls it's Start() method.
            serverThread = new Thread(servM.Start);

            // Sets a toggle-boolean so we can start/stop the server later.
            serverManage.online = true;
            serverThread.Start();
            Console.WriteLine("Starting server...");
            outputLog.Text = "Starting server";

            // Collect local ip address of server.
            string serverIPPublic = new System.Net.WebClient().DownloadString("https://api.ipify.org");
            serverIP.Content = "Server IP: "+ serverIPPublic;

            // Makes the box green for visual status that the server is running.
            ServerStatusIndicator.Value = 100;
        }

        private void Server_Stop(object sender, RoutedEventArgs e)
        {
            
            Console.WriteLine("Stopping server...");
            outputLog.Text = "Stopping server";
            try
            {
                ServerStatusIndicator.Value = 0;

                // We set the boolean online value to false so it stops looping infinitely, and then call it's Stop method.
                serverManage.online = false;
                serverManage.server.Stop();
                
                Console.WriteLine("Server stopped.");
                serverIP.Content = "Server IP:";
                outputLog.Text = "";
            }
            catch(Exception a)
            {
                Console.WriteLine("Abort failed.");
            }
        }
    }
}
