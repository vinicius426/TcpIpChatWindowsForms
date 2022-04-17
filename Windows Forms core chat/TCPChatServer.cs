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

            
            bool isUsername = text.ToLower().Contains("!username");
            bool isServer = text.ToLower().Contains("!server");
            bool isCommands = text.ToLower().Contains("!commands");
            bool isExit = text.ToLower().Contains("!exit");
            bool isWho = text.ToLower().Contains("!who");
            bool isAbout = text.ToLower().Contains("!about");
            bool isWhisper = text.ToLower().Contains("!whisper");
            bool isNewUsername = text.ToLower().Contains("!new_username");
            bool isMod = text.ToLower().Contains("!mod");
            bool isKick = text.ToLower().Contains("!kick");
            bool isMute = text.ToLower().Contains("!mute");

            void KeepSubscriptionOpen()
            {
                currentClientSocket.socket.BeginReceive(currentClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClientSocket);
            }

            if (isUsername == true && isServer == false && currentClientSocket.username == null)
            {
                //remove !new_username from string and check if username exists in List of clientSockets
                string trimmedText = text[(text.Split()[0].Length + 1)..];
                bool userExists = clientSockets.Any(item => item.username == trimmedText);
                
                if (userExists == true)
                {
                    SendToAll("Username " + trimmedText + " is in use. Client will be disconnected", null);
                    currentClientSocket.socket.Shutdown(SocketShutdown.Both);
                    currentClientSocket.socket.Close();
                    clientSockets.Remove(currentClientSocket);
                    AddToChat("Client disconnected");
                }
                else
                {
                    string newUsername = trimmedText;
                    currentClientSocket.username = newUsername;
                    SendToAll(currentClientSocket.username + " joined the server!", null);
                    KeepSubscriptionOpen();
                }
            }
            else if (isNewUsername == true && isServer == false)
            {
                //remove !new_setusername from string
                string trimmedText = text?[(text.Split()[0].Length + 1)..];
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
                    SendToAll("Username " + newSetUsername + " is not available. " + currentClientSocket.username + " will be disconnected", null);
                    currentClientSocket.socket.Shutdown(SocketShutdown.Both);
                    currentClientSocket.socket.Close();
                    clientSockets.Remove(currentClientSocket);
                    AddToChat(currentClientSocket.username + " disconnected");
                    return;
                }
                else
                {
                    currentClientSocket.username = newSetUsername;
                    SendToAll(oldUsername + " has changed username to " + newSetUsername, null);
                    KeepSubscriptionOpen();
                }
            }
            else
            {
                if (isCommands == true) // Client requested time
                {
                    ServerMessage("Commands are !commands !about !who !whisper !exit !kick !mute !username !new_username", currentClientSocket);
                    AddToChat("Commands sent to " + currentClientSocket.username);
                }
                else if (isExit == true) // Client wants to exit gracefully
                {
                    // Always Shutdown before closing
                    AddToChat(currentClientSocket.username + " disconnected");
                    currentClientSocket.socket.Close();
                    clientSockets.Remove(currentClientSocket);
                    
                    return;
                }
                else if (isWho == true)
                {
                    ServerMessage("Active Clients in the server: \n", currentClientSocket);
                    foreach (ClientSocket clientSocket in clientSockets)
                    {
                        ServerMessage(clientSocket.username + "\r\n", currentClientSocket);
                        AddToChat("Who sent to " + currentClientSocket.username);
                    };

                }
                else if (isAbout == true)
                {
                    ServerMessage("This project was created by Vinicius on April/222", currentClientSocket);
                    AddToChat("About sent to " + currentClientSocket.username);
                    KeepSubscriptionOpen();
                }
                else if (isWhisper == true)
                {
                    string trimmedText = text[(text.Split()[0].Length + 1)..];
                    string clearText = trimmedText[(trimmedText.Split()[0].Length + 1)..];
                    string getSendTo = Regex.Replace(trimmedText.Split()[0], @"[^0-9a-zA-Z\ ]+", "");
                    SendWhisper(clearText, currentClientSocket, getSendTo);
                    AddToChat(currentClientSocket.username + " whispers " + getSendTo);
                    KeepSubscriptionOpen();

                }
                else if (isMod == true)
                {
                    string trimmedText = text[(text.Split()[0].Length + 1)..];
                    AddToChat(trimmedText);
                    KeepSubscriptionOpen();
                }
                else if (isKick == true)
                {
                    string trimmedText = text[(text.Split()[0].Length + 1)..];
                    string getKickWho = Regex.Replace(trimmedText.Split()[0], @"[^0-9a-zA-Z\ ]+", "");
                    if (currentClientSocket.moderator == true)
                    {
                        var matchClient = clientSockets.Find(e => e.username == getKickWho);
                        string disconnectedClient = matchClient.username;
                        AddToChat(disconnectedClient + " disconnected");
                        SendToAll(disconnectedClient + " has been kicked from the server by " + currentClientSocket.username, currentClientSocket);
                        matchClient.socket.Close();
                        clientSockets.Remove(matchClient);
                        return;
                    }
                    else
                    {
                        AddToChat(currentClientSocket.username + " is tring to execute admin commands");
                        ServerMessage("You need Moderator rights to execute this command", currentClientSocket);
                    }
                }
                else if (isMute == true)
                {
                    string trimmedText = text[(text.Split()[0].Length + 1)..];
                    string getMuteWho = Regex.Replace(trimmedText.Split()[0], @"[^0-9a-zA-Z\ ]+", "");
                    if (currentClientSocket.moderator == true)
                    {
                        var matchClient = clientSockets.Find(e => e.username == getMuteWho);
                        string mutedClient = matchClient.username;
                        if(matchClient.muted == true)
                        {
                            matchClient.muted = false;
                            AddToChat(mutedClient + " unmuted by " + currentClientSocket);
                            SendToAll(mutedClient + " has been unmuted by " + currentClientSocket.username, currentClientSocket);
                            KeepSubscriptionOpen();
                        }
                        else
                        {
                            matchClient.muted = true;
                            AddToChat(mutedClient + " muted by " + currentClientSocket);
                            SendToAll(mutedClient + " has been muted by " + currentClientSocket.username, currentClientSocket);
                            KeepSubscriptionOpen();
                        }
                        return;
                    }
                    else
                    {
                        AddToChat(currentClientSocket.username + " is tring to execute admin commands");
                        ServerMessage("You need Moderator rights to execute this command", currentClientSocket);
                    }
                }
                else if (isUsername == true && currentClientSocket.username != null)
                {
                    ServerMessage("The command you are looking for is !new_username!", currentClientSocket);
                }
                else if (currentClientSocket.username == null)
                {
                    ServerMessage("You must choose an username before continue", null);
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

        public void SetModerator(string str)
        {
            var match = clientSockets.Find(e => e.username == str);
            if (match?.username != null)
            {
                if(match.moderator == true)
                {
                    string message = match.username + " is no longer a moderator";
                    match.moderator = false;
                    SendToAll(message, null);
                    AddToChat(message);
                }
                else
                {
                    match.moderator = true;
                    string message = match.username + " has been promoted to moderator!";
                    SendToAll(message, null);
                    AddToChat(message);
                }
            }
            else
            {
                AddToChat("Username " + str +  " not found");
            }
        }

        public void SendToAll(string str, ClientSocket from)
        {
            if (from?.muted == true)
            {
                byte[] data = Encoding.ASCII.GetBytes("You are muted");
                from.socket.Send(data);
            }
            else
            {
                AddToChat("...sent by " + from?.username);
                foreach (ClientSocket c in clientSockets)
                {
                    if (from?.socket != null && !from.socket.Equals(c) && from?.muted == false)
                    {
                        byte[] data = Encoding.ASCII.GetBytes(from?.username + ": " + str);
                        c.socket.Send(data);
                    }
                    else
                    {
                        byte[] data = Encoding.ASCII.GetBytes("SERVER: " + str + "\r\n");
                        c.socket.Send(data);
                    }
                }
            }
        }

        public void SendWhisper(string str, ClientSocket from, string to)
        {
            var match = clientSockets.Find(socket => socket.username == to);

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
    }
}
