

namespace DatabaseSyncApp
{
    partial class ConnectionStringConfigForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            serverLabel = new Label();
            serverTextBox = new TextBox();
            databaseLabel = new Label();
            databaseTextBox = new TextBox();
            usernameLabel = new Label();
            usernameTextBox = new TextBox();
            passwordLabel = new Label();
            passwordTextBox = new TextBox();
            saveButton = new Button();
            nameTextBox = new TextBox();
            label1 = new Label();
            testButton = new Button();
            SuspendLayout();
            // 
            // serverLabel
            // 
            serverLabel.AutoSize = true;
            serverLabel.Location = new Point(15, 82);
            serverLabel.Margin = new Padding(4, 0, 4, 0);
            serverLabel.Name = "serverLabel";
            serverLabel.Size = new Size(42, 15);
            serverLabel.TabIndex = 0;
            serverLabel.Text = "Server:";
            // 
            // serverTextBox
            // 
            serverTextBox.Location = new Point(112, 79);
            serverTextBox.Margin = new Padding(4, 3, 4, 3);
            serverTextBox.Name = "serverTextBox";
            serverTextBox.Size = new Size(233, 23);
            serverTextBox.TabIndex = 1;
            // 
            // databaseLabel
            // 
            databaseLabel.AutoSize = true;
            databaseLabel.Location = new Point(15, 117);
            databaseLabel.Margin = new Padding(4, 0, 4, 0);
            databaseLabel.Name = "databaseLabel";
            databaseLabel.Size = new Size(58, 15);
            databaseLabel.TabIndex = 2;
            databaseLabel.Text = "Database:";
            // 
            // databaseTextBox
            // 
            databaseTextBox.Location = new Point(112, 113);
            databaseTextBox.Margin = new Padding(4, 3, 4, 3);
            databaseTextBox.Name = "databaseTextBox";
            databaseTextBox.Size = new Size(233, 23);
            databaseTextBox.TabIndex = 3;
            // 
            // usernameLabel
            // 
            usernameLabel.AutoSize = true;
            usernameLabel.Location = new Point(15, 152);
            usernameLabel.Margin = new Padding(4, 0, 4, 0);
            usernameLabel.Name = "usernameLabel";
            usernameLabel.Size = new Size(63, 15);
            usernameLabel.TabIndex = 4;
            usernameLabel.Text = "Username:";
            // 
            // usernameTextBox
            // 
            usernameTextBox.Location = new Point(112, 148);
            usernameTextBox.Margin = new Padding(4, 3, 4, 3);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(233, 23);
            usernameTextBox.TabIndex = 5;
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(15, 186);
            passwordLabel.Margin = new Padding(4, 0, 4, 0);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(60, 15);
            passwordLabel.TabIndex = 6;
            passwordLabel.Text = "Password:";
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new Point(112, 183);
            passwordTextBox.Margin = new Padding(4, 3, 4, 3);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.Size = new Size(233, 23);
            passwordTextBox.TabIndex = 7;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(258, 221);
            saveButton.Margin = new Padding(4, 3, 4, 3);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(88, 27);
            saveButton.TabIndex = 8;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += SaveButton_Click;
            // 
            // nameTextBox
            // 
            nameTextBox.Location = new Point(113, 28);
            nameTextBox.Margin = new Padding(4, 3, 4, 3);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(233, 23);
            nameTextBox.TabIndex = 10;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 31);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(42, 15);
            label1.TabIndex = 9;
            label1.Text = "Name:";
            // 
            // testButton
            // 
            testButton.Location = new Point(113, 221);
            testButton.Margin = new Padding(4, 3, 4, 3);
            testButton.Name = "testButton";
            testButton.Size = new Size(116, 27);
            testButton.TabIndex = 11;
            testButton.Text = "Test Connection";
            testButton.UseVisualStyleBackColor = true;
            testButton.Click += testButton_Click;
            // 
            // ConnectionStringConfigForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(358, 289);
            Controls.Add(testButton);
            Controls.Add(nameTextBox);
            Controls.Add(label1);
            Controls.Add(saveButton);
            Controls.Add(passwordTextBox);
            Controls.Add(passwordLabel);
            Controls.Add(usernameTextBox);
            Controls.Add(usernameLabel);
            Controls.Add(databaseTextBox);
            Controls.Add(databaseLabel);
            Controls.Add(serverTextBox);
            Controls.Add(serverLabel);
            Margin = new Padding(4, 3, 4, 3);
            Name = "ConnectionStringConfigForm";
            Text = "Connection String Configuration";
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.TextBox serverTextBox;
        private System.Windows.Forms.Label databaseLabel;
        private System.Windows.Forms.TextBox databaseTextBox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Button saveButton;
        private TextBox nameTextBox;
        private Label label1;
        private Button testButton;
    }
}
