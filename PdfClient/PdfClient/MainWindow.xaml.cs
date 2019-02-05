using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace PdfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int PORT_NO = 5709;
        string greeting = "Hello Server";

        TcpClient client;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            string SERVER_IP = enteredIP.Text;
            outputLog.Text = "Sending " + greeting + " to '" + SERVER_IP + "'...\n";
            try
            {
                //---create a TCPClient object at the IP and port no.---
                client = new TcpClient(SERVER_IP, PORT_NO);
                NetworkStream nwStream = client.GetStream();
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(greeting);

                //---send the text---
                Console.WriteLine("Sending : " + greeting);
                outputLog.Text = "Sending "+greeting+" to '"+SERVER_IP + "'...\n";
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                //---read back the text---
                byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                outputLog.Text = outputLog.Text + "Received " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                
            }
            catch (Exception t)
            {
                Console.WriteLine("Server could not connect. "+t);
                outputLog.Text = "Failed to connect.";
            }
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.Close();
                outputLog.Text = "";
                enteredIP.Text = "";
            }
            catch(Exception c)
            {
                Console.WriteLine("Not connected. " + c);
                outputLog.Text = "Not connected.";
            }
        }
    }
}
