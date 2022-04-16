using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Linq;
using System.Text.RegularExpressions;

//https://github.com/AbleOpus/NetworkingSamples/blob/master/MultiServer/Program.cs
namespace Windows_Forms_Chat
{
    public class TCPChatServer : TCPChatBase
    {
        public Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //connected clients
        public List<ClientSocket> clientSockets = new List<ClientSocket>();

        public static TCPChatServer createInstance(int port, TextBox chatTextBox)
        {
            TCPChatServer tcp = null;
            //setup if port within range and valid chat box given
            if (port > 0 && port < 65535 && chatTextBox != null)
            {
                tcp = new TCPChatServer();
                tcp.port = port;
                tcp.chatTextBox = chatTextBox;

            }

            //return empty if user not enter useful details
            return tcp;
        }

        public void SetupServer()
            {
                AddToChat("Setting up server...");
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                serverSocket.Listen(0);
                //kick off thread to read connecting clients, when one connects, it'll call out AcceptCallback function
                serverSocket.BeginAccept(AcceptCallback, this);
            // chatTextBox.Text += "Server setup complete \n";
            //  chatTextBox.Text += "\n\n" + user + " has joined the channel!";
            AddToChat("Server setup complete");
        }

        public void CloseAllSockets()
        {
            foreach (ClientSocket clientSocket in clientSockets)
            {
                clientSocket.socket.Shutdown(SocketShutdown.Both);
                clientSocket.socket.Close();
            };
            clientSockets.Clear();
            serverSocket.Close();
        }

        public void AcceptCallback(IAsyncResult AR)
        {
            Socket joiningSocket;

            try
            {
                joiningSocket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }

            ClientSocket newClientSocket = new ClientSocket();
            newClientSocket.socket = joiningSocket;

            clientSockets.Add(newClientSocket);
            //start a thread to listen out for this new joining socket. Therefore there is a thread open for each client
            joiningSocket.BeginReceive(newClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, newClientSocket);
            AddToChat("Client connected, waiting for request...");

            //we finished this accept thread, better kick off another so more people can join
            serverSocket.BeginAccept(AcceptCallback, null);
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
                clientSockets.Remove(currentClientSocket);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(currentClientSocket.buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);

            AddToChat(text);

            
            var isUsername = text.ToLower().Contains("!new_username");
            var isCommands = text.ToLower().Contains("!commands");
            var isExit = text.ToLower().Contains("!exit");
            var isWho = text.ToLower().Contains("!who");
            var isAbout = text.ToLower().Contains("!about");
            var isWhisper = text.ToLower().Contains("!whisper");
            var isSetUsername = text.ToLower().Contains("!set_username");

            void KeepSubscriptionOpen()
            {
                currentClientSocket.socket.BeginReceive(currentClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClientSocket);
            }

            if (isUsername == true)
            {
                //remove !new_username from string and check if username exists in List of clientSockets
                string trimmedText = text[(text.Split()[0].Length + 1)..];
                bool userExists = clientSockets.Any(item => item.username == trimmedText);
                
                if (userExists == true)
                {
                    SendToAll("Username " + trimmedText + " is in use. Client will be disconnected", currentClientSocket);
                    currentClientSocket.socket.Shutdown(SocketShutdown.Both);
                    currentClientSocket.socket.Close();
                    clientSockets.Remove(currentClientSocket);
                    AddToChat("Client disconnected");
                }
                else
                {
                    string newUsername = trimmedText;
                    currentClientSocket.username = newUsername;
                    SendToAll("joined the server!", currentClientSocket);
                    KeepSubscriptionOpen();
                }
            }
            else if (isSetUsername == true)
            {
                //remove !set_setusername from string
                string trimmedText = text[(text.Split()[0].Length + 1)..];
                string oldUsername = null;
                trimmedText = trimmedText.Trim();

                //remove new username from string to be able to remove old from array
                if (trimmedText.Contains(" "))
                {
                    oldUsername = trimmedText.Substring(0, trimmedText.LastIndexOf(' ')).TrimEnd();
                };

                //remove old username from string to be able to change to new username
                string newSetUsername = trimmedText[(trimmedText.Split()[0].Length + 1)..];
                bool verifyNewUserExists = clientSockets.Any(item => item.username == newSetUsername);
                if (verifyNewUserExists == true)
                {
                    SendToAll("Username " + newSetUsername + " is not available. " + currentClientSocket.username + " will be disconnected", currentClientSocket);
                    currentClientSocket.socket.Shutdown(SocketShutdown.Both);
                    currentClientSocket.socket.Close();
                    clientSockets.Remove(currentClientSocket);
                    AddToChat(currentClientSocket.username + " disconnected");
                    return;
                }
                else
                {
                    currentClientSocket.username = newSetUsername;
                    SendToAll(oldUsername + " has changed username to " + newSetUsername, currentClientSocket);
                    KeepSubscriptionOpen();
                }
            }
            else
            {
                if (isCommands == true) // Client requested time
                {
                    byte[] data = Encoding.ASCII.GetBytes("Commands are !commands !about !who !whisper !exit");
                    currentClientSocket.socket.Send(data);
                    AddToChat("Commands sent to client");
                }
                else if (isExit == true) // Client wants to exit gracefully
                {
                    // Always Shutdown before closing
                    currentClientSocket.socket.Shutdown(SocketShutdown.Both);
                    currentClientSocket.socket.Close();
                    clientSockets.Remove(currentClientSocket);
                    AddToChat("Client disconnected");
                    return;
                }
                else if (isWho == true)
                {
                    SendToAll("Who is active on the server: \n", currentClientSocket);
                    foreach (ClientSocket clientSocket in clientSockets)
                    {
                        SendToAll(clientSocket.username, currentClientSocket);
                    };

                }
                else if (isAbout == true)
                {
                    SendToAll("This project was created by Vinicius on April/222", currentClientSocket);
                    KeepSubscriptionOpen();
                }
                else if (isWhisper == true)
                {
                    string trimmedText = text[(text.Split()[0].Length + 1)..];
                    string clearText = trimmedText[(trimmedText.Split()[0].Length + 1)..];
                    string getSendTo = Regex.Replace(trimmedText.Split()[0], @"[^0-9a-zA-Z\ ]+", "");
                    SendWhisper(clearText, currentClientSocket, getSendTo);
                    AddToChat(currentClientSocket.username + " whispers " + getSendTo);
                     
                }
                else
                {
                    //normal message broadcast out to all clients
                    SendToAll(text, currentClientSocket);
                }
                //we just received a message from this socket, better keep an ear out with another thread for the next one
                KeepSubscriptionOpen();
            }
        }

        public void SendToAll(string str, ClientSocket from)
        {
            foreach(ClientSocket c in clientSockets)
            {
                if(from == null || !from.socket.Equals(c))
                {
                    byte[] data = Encoding.ASCII.GetBytes(from.username + ": " +  str);
                    c.socket.Send(data);
                }
            }
        }

        public void SendWhisper(string str, ClientSocket from, string to)
        {
            var match = clientSockets.Find(e => e.username == to);


            if (match?.socket != null && from.username != to)
            {
                byte[] dataTo = Encoding.ASCII.GetBytes(from.username + " whispers You: " + str);
                match.socket.Send(dataTo);
                byte[] dataFrom = Encoding.ASCII.GetBytes(" You whisper " + match.username + ": " + str);
                from.socket.Send(dataFrom);
            }
            else if (from.username == to)
            {
                byte[] dataFrom = Encoding.ASCII.GetBytes("Are you trying to whisper yourself?...Weird...");
                from.socket.Send(dataFrom);
            }
            else
            { 
                byte[] dataNotFound = Encoding.ASCII.GetBytes("Username " + to + " not connected to the server.");
                from.socket.Send(dataNotFound);
                
            }
        }
    }
}
