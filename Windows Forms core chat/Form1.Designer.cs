﻿namespace Windows_Forms_Chat
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.MyPortTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.serverPortTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ServerIPTextBox = new System.Windows.Forms.TextBox();
            this.ChatTextBox = new System.Windows.Forms.TextBox();
            this.TypeTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.HostButton = new System.Windows.Forms.Button();
            this.JoinButton = new System.Windows.Forms.Button();
            this.SendButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.Divider = new System.Windows.Forms.Label();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            this.UsernameConfirmButton = new System.Windows.Forms.Button();
            this.GreetingsLabel = new System.Windows.Forms.Label();
            this.sendWhisperLabel = new System.Windows.Forms.Label();
            this.whisperTextBox = new System.Windows.Forms.TextBox();
            this.whisperButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "My Port";
            // 
            // MyPortTextBox
            // 
            this.MyPortTextBox.Location = new System.Drawing.Point(11, 28);
            this.MyPortTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MyPortTextBox.Name = "MyPortTextBox";
            this.MyPortTextBox.Size = new System.Drawing.Size(110, 23);
            this.MyPortTextBox.TabIndex = 1;
            this.MyPortTextBox.Text = "6666";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(220, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Server Port";
            // 
            // serverPortTextBox
            // 
            this.serverPortTextBox.Location = new System.Drawing.Point(220, 28);
            this.serverPortTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.serverPortTextBox.Name = "serverPortTextBox";
            this.serverPortTextBox.Size = new System.Drawing.Size(110, 23);
            this.serverPortTextBox.TabIndex = 3;
            this.serverPortTextBox.Text = "6666";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(370, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "server IP";
            // 
            // ServerIPTextBox
            // 
            this.ServerIPTextBox.Location = new System.Drawing.Point(370, 28);
            this.ServerIPTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ServerIPTextBox.Name = "ServerIPTextBox";
            this.ServerIPTextBox.Size = new System.Drawing.Size(140, 23);
            this.ServerIPTextBox.TabIndex = 5;
            this.ServerIPTextBox.Text = "127.0.0.1";
            // 
            // ChatTextBox
            // 
            this.ChatTextBox.Location = new System.Drawing.Point(12, 217);
            this.ChatTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ChatTextBox.Multiline = true;
            this.ChatTextBox.Name = "ChatTextBox";
            this.ChatTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ChatTextBox.Size = new System.Drawing.Size(530, 155);
            this.ChatTextBox.TabIndex = 6;
            this.ChatTextBox.Text = "\r\n";
            // 
            // TypeTextBox
            // 
            this.TypeTextBox.Enabled = false;
            this.TypeTextBox.Location = new System.Drawing.Point(54, 384);
            this.TypeTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TypeTextBox.Name = "TypeTextBox";
            this.TypeTextBox.Size = new System.Drawing.Size(392, 23);
            this.TypeTextBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 384);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Chat:";
            // 
            // HostButton
            // 
            this.HostButton.Location = new System.Drawing.Point(11, 68);
            this.HostButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.HostButton.Name = "HostButton";
            this.HostButton.Size = new System.Drawing.Size(82, 22);
            this.HostButton.TabIndex = 9;
            this.HostButton.Text = "Host Server";
            this.HostButton.UseVisualStyleBackColor = true;
            this.HostButton.Click += new System.EventHandler(this.HostButton_Click);
            // 
            // JoinButton
            // 
            this.JoinButton.Location = new System.Drawing.Point(220, 68);
            this.JoinButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.JoinButton.Name = "JoinButton";
            this.JoinButton.Size = new System.Drawing.Size(82, 22);
            this.JoinButton.TabIndex = 10;
            this.JoinButton.Text = "Join Server";
            this.JoinButton.UseVisualStyleBackColor = true;
            this.JoinButton.Click += new System.EventHandler(this.JoinButton_Click);
            // 
            // SendButton
            // 
            this.SendButton.Enabled = false;
            this.SendButton.Location = new System.Drawing.Point(460, 384);
            this.SendButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(82, 22);
            this.SendButton.TabIndex = 11;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(151, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "OR";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Violet;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(595, 92);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 74);
            this.button1.TabIndex = 13;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Violet;
            this.button2.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button2.Location = new System.Drawing.Point(682, 92);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(82, 74);
            this.button2.TabIndex = 13;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Violet;
            this.button3.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button3.Location = new System.Drawing.Point(770, 92);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(82, 74);
            this.button3.TabIndex = 13;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Violet;
            this.button4.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button4.Location = new System.Drawing.Point(595, 170);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(82, 74);
            this.button4.TabIndex = 13;
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Violet;
            this.button5.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button5.Location = new System.Drawing.Point(682, 170);
            this.button5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(82, 74);
            this.button5.TabIndex = 13;
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Violet;
            this.button6.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button6.Location = new System.Drawing.Point(770, 170);
            this.button6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(82, 74);
            this.button6.TabIndex = 13;
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Violet;
            this.button7.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button7.Location = new System.Drawing.Point(595, 248);
            this.button7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(82, 74);
            this.button7.TabIndex = 13;
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.Violet;
            this.button8.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button8.Location = new System.Drawing.Point(682, 248);
            this.button8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(82, 74);
            this.button8.TabIndex = 13;
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.Violet;
            this.button9.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button9.Location = new System.Drawing.Point(770, 248);
            this.button9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(82, 74);
            this.button9.TabIndex = 13;
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // Divider
            // 
            this.Divider.AutoSize = true;
            this.Divider.Location = new System.Drawing.Point(11, 92);
            this.Divider.Name = "Divider";
            this.Divider.Size = new System.Drawing.Size(497, 15);
            this.Divider.TabIndex = 14;
            this.Divider.Text = "_________________________________________________________________________________" +
    "_________________";
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Location = new System.Drawing.Point(11, 117);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(60, 15);
            this.UsernameLabel.TabIndex = 15;
            this.UsernameLabel.Text = "Username";
            this.UsernameLabel.UseMnemonic = false;
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.Enabled = false;
            this.UsernameTextBox.Location = new System.Drawing.Point(11, 135);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(110, 23);
            this.UsernameTextBox.TabIndex = 16;
            // 
            // UsernameConfirmButton
            // 
            this.UsernameConfirmButton.Enabled = false;
            this.UsernameConfirmButton.Location = new System.Drawing.Point(10, 170);
            this.UsernameConfirmButton.Name = "UsernameConfirmButton";
            this.UsernameConfirmButton.Size = new System.Drawing.Size(75, 23);
            this.UsernameConfirmButton.TabIndex = 17;
            this.UsernameConfirmButton.Text = "Confirm";
            this.UsernameConfirmButton.UseVisualStyleBackColor = true;
            this.UsernameConfirmButton.Click += new System.EventHandler(this.UsernameConfirmButton_Click);
            // 
            // GreetingsLabel
            // 
            this.GreetingsLabel.AutoSize = true;
            this.GreetingsLabel.Location = new System.Drawing.Point(595, 36);
            this.GreetingsLabel.Name = "GreetingsLabel";
            this.GreetingsLabel.Size = new System.Drawing.Size(0, 15);
            this.GreetingsLabel.TabIndex = 18;
            // 
            // sendWhisperLabel
            // 
            this.sendWhisperLabel.AutoSize = true;
            this.sendWhisperLabel.Location = new System.Drawing.Point(220, 117);
            this.sendWhisperLabel.Name = "sendWhisperLabel";
            this.sendWhisperLabel.Size = new System.Drawing.Size(50, 15);
            this.sendWhisperLabel.TabIndex = 19;
            this.sendWhisperLabel.Text = "Whisper";
            // 
            // whisperTextBox
            // 
            this.whisperTextBox.Enabled = false;
            this.whisperTextBox.Location = new System.Drawing.Point(220, 135);
            this.whisperTextBox.Name = "whisperTextBox";
            this.whisperTextBox.Size = new System.Drawing.Size(110, 23);
            this.whisperTextBox.TabIndex = 20;
            // 
            // whisperButton
            // 
            this.whisperButton.Enabled = false;
            this.whisperButton.Location = new System.Drawing.Point(220, 170);
            this.whisperButton.Name = "whisperButton";
            this.whisperButton.Size = new System.Drawing.Size(75, 23);
            this.whisperButton.TabIndex = 21;
            this.whisperButton.Text = "Select";
            this.whisperButton.UseVisualStyleBackColor = true;
            this.whisperButton.Click += new System.EventHandler(this.whisperButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 420);
            this.Controls.Add(this.whisperButton);
            this.Controls.Add(this.whisperTextBox);
            this.Controls.Add(this.sendWhisperLabel);
            this.Controls.Add(this.GreetingsLabel);
            this.Controls.Add(this.UsernameConfirmButton);
            this.Controls.Add(this.UsernameTextBox);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.Divider);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.JoinButton);
            this.Controls.Add(this.HostButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TypeTextBox);
            this.Controls.Add(this.ChatTextBox);
            this.Controls.Add(this.ServerIPTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.serverPortTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MyPortTextBox);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox MyPortTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox serverPortTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ServerIPTextBox;
        private System.Windows.Forms.TextBox ChatTextBox;
        private System.Windows.Forms.TextBox TypeTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button HostButton;
        private System.Windows.Forms.Button JoinButton;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Label Divider;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.TextBox UsernameTextBox;
        public System.Windows.Forms.Button UsernameConfirmButton;
        private System.Windows.Forms.Label GreetingsLabel;
        private System.Windows.Forms.Label sendWhisperLabel;
        private System.Windows.Forms.TextBox whisperTextBox;
        private System.Windows.Forms.Button whisperButton;
    }
}

