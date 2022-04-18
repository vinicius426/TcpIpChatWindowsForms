using System;
using System.Windows.Forms;


//https://www.youtube.com/watch?v=xgLRe7QV6QI&ab_channel=HazardEditHazardEdit
namespace Windows_Forms_Chat
{
    public partial class Form1 : Form
    {
        TicTacToe ticTacToe = new TicTacToe();
        TCPChatServer server = null;
        TCPChatClient client = null;
        string username = null;
        string newUsername = null;

        public Form1()
        {
            InitializeComponent();
        }

        public bool CanHostOrJoin()
        {
            if (server == null && client == null)
                return true;
            else
                return false;
        }

        private void HostButton_Click(object sender, EventArgs e)
        {
            if (CanHostOrJoin())
            {
                try
                {
                    int port = int.Parse(MyPortTextBox.Text);
                    server = TCPChatServer.createInstance(port, ChatTextBox);
                    //oh no, errors
                    if (server == null)
                        throw new Exception("System: Incorrect port value!\n");//thrown exceptions should exit the try and land in next catch

                    server.SetupServer();
                    HostButton.Enabled = false;
                    JoinButton.Enabled = false;
                    TypeTextBox.Enabled = true;
                    SendButton.Enabled = true;
                    SetModeratorButton.Enabled = true;
                    SetModeratorTextBox.Enabled = true;
                }
                catch (Exception ex)
                {
                    ChatTextBox.Text += "System: Error: " + ex + "\n";
                    ChatTextBox.AppendText(Environment.NewLine);
                }
            }

        }

        private void JoinButton_Click(object sender, EventArgs e)
        {
            if (username != null)
            {
                if (CanHostOrJoin())
                {
                    try
                    {
                        int port = int.Parse(MyPortTextBox.Text);
                        int serverPort = int.Parse(serverPortTextBox.Text);
                        client = TCPChatClient.CreateInstance(port, serverPort, ServerIPTextBox.Text, ChatTextBox);

                        if (client == null)
                            throw new Exception("System: Incorrect port value!\n");//thrown exceptions should exit the try and land in next catch

                        client.ConnectToServer();
                        client.SendString("!username " + username);
                        warnLabel.Text = "";
                        HostButton.Enabled = false;
                        JoinButton.Enabled = false;
                        whisperTextBox.Enabled = true;
                        WhisperButton.Enabled = true;

                    }
                    catch (Exception ex)
                    {
                        client = null;
                        ChatTextBox.Text += "System: Error: " + ex + "\n";
                        ChatTextBox.AppendText(Environment.NewLine);
                    }
                }
            }
            else
            {
                warnLabel.Text = "Please, choose an username before continue.";
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            bool isUsername = TypeTextBox.Text.ToLower().Contains("!username");
            bool isNewUsername = TypeTextBox.Text.ToLower().Contains("!new_username");

            if (client != null)
            {
                if (isUsername == true)
                {
                    string trimmedText = TypeTextBox.Text[(TypeTextBox.Text.Split()[0].Length + 1)..];
                    username = trimmedText;
                }
                else if (isNewUsername == true)
                {
                    string trimmedText = TypeTextBox.Text[(TypeTextBox.Text.Split()[0].Length + 1)..];
                    newUsername = trimmedText;
                    TypeTextBox.Clear();
                    client.SendString("!new_username " + username + " " + newUsername);
                    username = newUsername;
                }
                client.SendString(TypeTextBox.Text);
            }
            else if (server != null)
            {
                server.SendToAll(TypeTextBox.Text, null);
            }
            else if (isUsername)
            {
                string trimmedText = TypeTextBox.Text[(TypeTextBox.Text.Split()[0].Length + 1)..];
                username = trimmedText;
            }

            TypeTextBox.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //On form loaded
            ticTacToe.buttons.Add(button1);
            ticTacToe.buttons.Add(button2);
            ticTacToe.buttons.Add(button3);
            ticTacToe.buttons.Add(button4);
            ticTacToe.buttons.Add(button5);
            ticTacToe.buttons.Add(button6);
            ticTacToe.buttons.Add(button7);
            ticTacToe.buttons.Add(button8);
            ticTacToe.buttons.Add(button9);
        }

        private void AttemptMove(int i)
        {
            if (ticTacToe.myTurn)
            {
                bool validMove = ticTacToe.SetTile(i, ticTacToe.playerTileType);
                if (validMove)
                {
                    //tell server about it
                    //ticTacToe.myTurn = false;//call this too when ready with server
                }
                //example, do something similar from server
                GameState gs = ticTacToe.GetGameState();
                if (gs == GameState.crossWins)
                {
                    ChatTextBox.AppendText("X wins!");
                    ChatTextBox.AppendText(Environment.NewLine);
                    ticTacToe.ResetBoard();
                }
                if (gs == GameState.naughtWins)
                {
                    ChatTextBox.AppendText(") wins!");
                    ChatTextBox.AppendText(Environment.NewLine);
                    ticTacToe.ResetBoard();
                }
                if (gs == GameState.draw)
                {
                    ChatTextBox.AppendText("Draw!");
                    ChatTextBox.AppendText(Environment.NewLine);
                    ticTacToe.ResetBoard();
                }
            }
        }

        private void UsernameConfirmButton_Click(object sender, EventArgs e)
        {
            if (client != null && server == null)
            {
                if (UsernameTextBox != null && username == null)
                {
                    username = UsernameTextBox.Text;
                    UsernameTextBox.Clear();
                    client.SendString("!username " + username);
                    TypeTextBox.Enabled = true;
                    SendButton.Enabled = true;
                    UsernameLabel.Text = "Change username";
                }
                else if (UsernameTextBox != null && username != null)
                {
                    newUsername = UsernameTextBox.Text;
                    UsernameTextBox.Clear();
                    client.SendString("!new_username " + username + " " + newUsername);
                    username = newUsername;
                }
            }
            else
            {
                username = UsernameTextBox.Text;
                UsernameTextBox.Clear();
            }
        }

        private void whisperButton_Click(object sender, EventArgs e)
        {
            if (whisperTextBox != null)
            {
                TypeTextBox.Clear();
                TypeTextBox.Text = "!whisper " + whisperTextBox.Text + ": ";
                whisperTextBox.Clear();
            }
        }

        private void SetModeratorButton_Click(object sender, EventArgs e)
        {
            string userTobeSet = SetModeratorTextBox.Text;
            server.SetModerator(userTobeSet);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AttemptMove(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AttemptMove(1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AttemptMove(2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AttemptMove(3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AttemptMove(4);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AttemptMove(5);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AttemptMove(6);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AttemptMove(7);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AttemptMove(8);
        }
    }
}
