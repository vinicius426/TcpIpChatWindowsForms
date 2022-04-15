using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Linq;

//https://github.com/AbleOpus/NetworkingSamples/blob/master/MultiServer/Program.cs
namespace Windows_Forms_Chat
{
    public class TCPChatServer : TCPChatBase
    {
        string[] userArray = { };

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

            List<string> list = new List<string>(userArray.ToList());


            if (isUsername == true)
            {
                //remove !new_username from string
                string trimmedText = text[(text.Split()[0].Length + 1)..];
                var userExist = Array.Exists(userArray, x => x == trimmedText);
                
                if (userExist == true)
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
                    list.Add(newUsername);
                    userArray = list.ToArray();
                    currentClientSocket.username = newUsername;
                    SendToAll("Username " + newUsername + " has joined the room", currentClientSocket);
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
                }

                userArray = userArray.Where(val => val != oldUsername).ToArray();

                //remove old username from string to be able to add to new username to array
                string newSetUsername = trimmedText[(trimmedText.Split()[0].Length + 1)..];
                var verifyUserExist = Array.Exists(userArray, x => x == newSetUsername);
                if(verifyUserExist == true)
                {
                    SendToAll("Username " + newSetUsername + " is not available. Client will be disconnected", currentClientSocket);
                    currentClientSocket.socket.Shutdown(SocketShutdown.Both);
                    currentClientSocket.socket.Close();
                    clientSockets.Remove(currentClientSocket);
                    AddToChat("Client disconnected");
                    return;
                }
                list.Add(newSetUsername);
                userArray = list.ToArray() ;
                currentClientSocket.username = newSetUsername;
                SendToAll(oldUsername + " has changed username to "+ newSetUsername, currentClientSocket);
                KeepSubscriptionOpen();
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
                    foreach (Object obj in clientSockets)
                    {
                        SendToAll(, currentClientSocket);
                        KeepSubscriptionOpen();
                    }

                }
                else if (isAbout == true)
                {
                    SendToAll("This project was created by Vinicius on April/222", currentClientSocket);
                    KeepSubscriptionOpen();
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
                    byte[] data = Encoding.ASCII.GetBytes(str);
                    c.socket.Send(data);
                }
            }
        }

        
    }
}
