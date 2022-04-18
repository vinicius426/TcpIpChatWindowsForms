using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;


//reference: https://github.com/AbleOpus/NetworkingSamples/blob/master/MultiClient/Program.cs
namespace Windows_Forms_Chat
{
    public class TCPChatClient : TCPChatBase
    {
        //public static TCPChatClient tcpChatClient;
        public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public ClientSocket clientSocket = new ClientSocket();


        public int serverPort;
        public string serverIP;


        public static TCPChatClient CreateInstance(int port, int serverPort, string serverIP, TextBox chatTextBox)
        {
            TCPChatClient tcp = null;
            //if port values are valid and ip worth attempting to join
            if (port > 0 && port < 65535 && 
                serverPort > 0 && serverPort < 65535 && 
                serverIP.Length > 0 &&
                chatTextBox != null)
            {
                tcp = new TCPChatClient
                {
                    port = port,
                    serverPort = serverPort,
                    serverIP = serverIP,
                    chatTextBox = chatTextBox
                };
                tcp.clientSocket.socket = tcp.socket;

            }

            return tcp;
        }

        public void ConnectToServer()
        {
            int attempts = 0;

            while (!socket.Connected)
            {
                try
                {
                    attempts++;
                    SetChat("Connection attempt " + attempts);
                    // Change IPAddress.Loopback to a remote IP to connect to a remote host.
                    socket.Connect(serverIP, serverPort);
                }
                catch (SocketException)
                {
                    chatTextBox.Text = "";
                }
            }
           
            //Console.Clear();
            AddToChat("Connected");
            //keep open thread for receiving data
            clientSocket.socket.BeginReceive(clientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, clientSocket);
        }

        public void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }


        public void ReceiveCallback(IAsyncResult AR)
        {
            ClientSocket currentClientSocket = (ClientSocket)AR.AsyncState;

            int received;

            try
            {
                received = currentClientSocket.socket.EndReceive(AR);
            }
            catch (SocketException)
            {
                AddToChat("Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                currentClientSocket.socket.Close();
                return;
            }
            //read bytes from packet
            byte[] recBuf = new byte[received];
            Array.Copy(currentClientSocket.buffer, recBuf, received);
            //convert to string so we can work with it
            string text = Encoding.ASCII.GetString(recBuf);
            Console.WriteLine("Received Text: " + text);

            //text is from server but could have been broadcast from the other clients

            bool isKick = text.ToLower() == "!kick";
            bool isUsername = text.ToLower().Contains("!username");
            bool isKickServer = text.Contains("SERVER: !kick");
            bool isModServer = text.Contains("SERVER: !mod");

            if(isModServer || isKickServer || isKick || isUsername)
            {
                if (isKick == true)
                {
                    string trimmedText = RemoveCommand(text);
                    if (currentClientSocket.moderator == true)
                    {
                        Close();
                        return;
                    }
                }
                else if (isKickServer)
                {
                    if (GetTarget(text) == clientSocket.username)
                    {
                        Application.Exit();
                        return;
                    }
                }
                else if (isUsername)
                {
                    string removeCommand = RemoveCommand(text);
                    clientSocket.username = removeCommand;
                }
                else if (isModServer == true)
                {
                    if (ValidCommand(text))
                    {
                        if (GetTarget(text) == clientSocket.username)
                        {
                            if (clientSocket.moderator == true)
                            {
                                clientSocket.moderator = false;
                                SendString("!change_mod");
                            }
                            else
                            {
                                clientSocket.moderator = true;
                                SendString("!change_mod");
                            }
                        }
                        else
                        {
                            AddToChat("User not found!");
                        }
                    }
                    else
                    {
                        ServerMessage("Command invalid, please type !commands to see available commands", currentClientSocket);
                    }
                }
            }
            else
            {
                AddToChat(text);
            }
            
            //we just received a message from this socket, better keep an ear out with another thread for the next one
            currentClientSocket.socket.BeginReceive(currentClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClientSocket);
        }
        public void Close()
        {
            socket.Close();
        }
        public string RemoveCommand(string str)
        {
            string text = str?[(str.Split()[0].Length + 1)..];
            return text;
        }

        private static bool ValidCommand(string text)
        {
            string[] newstring = text.Split();
            if (newstring.Length > 1 && newstring[1] != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ServerMessage(string str, ClientSocket from)
        {
            if (from?.socket == null || from?.username == null)
            {
                byte[] dataTo = Encoding.ASCII.GetBytes(str);
                from?.socket.Send(dataTo);
            }
            else
            {
                byte[] dataTo = Encoding.ASCII.GetBytes(str);
                from.socket.Send(dataTo);
            }
        }

        public string GetTarget(string str)
        {

            string removeCommand = RemoveCommand(str);
            string trimmedText = RemoveCommand(removeCommand);
            string target = Regex.Replace(trimmedText.Split()[0], @"[^0-9a-zA-Z\ ]+", "");
            return target;
        }
    }

}
