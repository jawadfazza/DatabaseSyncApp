namespace DatabaseSyncApp
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            sourceComboBox = new ComboBox();
            destinationComboBox = new ComboBox();
            transferButton = new Button();
            addConnectionStringButton = new Button();
            label1 = new Label();
            label2 = new Label();
            sourceLabel = new Label();
            destinationLabel = new Label();
            progressBar = new ProgressBar();
            btnDeleteSource = new Button();
            btnRefreshSource = new Button();
            messageLabel = new Label();
            tableProgressBar = new ProgressBar();
            label3 = new Label();
            label4 = new Label();
            btnRefreshDestination = new Button();
            btnDeleteDestination = new Button();
            SuspendLayout();
            // 
            // sourceComboBox
            // 
            sourceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            sourceComboBox.DropDownWidth = 380;
            sourceComboBox.Font = new Font("Segoe UI", 12F);
            sourceComboBox.FormattingEnabled = true;
            sourceComboBox.Location = new Point(111, 29);
            sourceComboBox.Margin = new Padding(4, 3, 4, 3);
            sourceComboBox.Name = "sourceComboBox";
            sourceComboBox.Size = new Size(377, 29);
            sourceComboBox.TabIndex = 0;
            sourceComboBox.SelectedIndexChanged += sourceComboBox_SelectedIndexChanged;
            // 
            // destinationComboBox
            // 
            destinationComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            destinationComboBox.DropDownWidth = 380;
            destinationComboBox.Font = new Font("Segoe UI", 12F);
            destinationComboBox.FormattingEnabled = true;
            destinationComboBox.Location = new Point(111, 85);
            destinationComboBox.Margin = new Padding(4, 3, 4, 3);
            destinationComboBox.Name = "destinationComboBox";
            destinationComboBox.Size = new Size(377, 29);
            destinationComboBox.TabIndex = 1;
            destinationComboBox.SelectedIndexChanged += DestinationComboBox_SelectedIndexChanged;
            // 
            // transferButton
            // 
            transferButton.BackColor = SystemColors.MenuHighlight;
            transferButton.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            transferButton.ForeColor = SystemColors.ControlLightLight;
            transferButton.Location = new Point(111, 179);
            transferButton.Margin = new Padding(4, 3, 4, 3);
            transferButton.Name = "transferButton";
            transferButton.Size = new Size(377, 36);
            transferButton.TabIndex = 6;
            transferButton.Text = "Transfer";
            transferButton.UseVisualStyleBackColor = false;
            transferButton.Click += TransferButton_Click;
            // 
            // addConnectionStringButton
            // 
            addConnectionStringButton.BackColor = Color.Gold;
            addConnectionStringButton.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            addConnectionStringButton.Location = new Point(111, 137);
            addConnectionStringButton.Margin = new Padding(4, 3, 4, 3);
            addConnectionStringButton.Name = "addConnectionStringButton";
            addConnectionStringButton.Size = new Size(377, 36);
            addConnectionStringButton.TabIndex = 7;
            addConnectionStringButton.Text = "Confige New Connection";
            addConnectionStringButton.UseVisualStyleBackColor = false;
            addConnectionStringButton.Click += AddConnectionStringButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.Location = new Point(41, 32);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(66, 21);
            label1.TabIndex = 10;
            label1.Text = "Source:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label2.Location = new Point(4, 89);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(103, 21);
            label2.TabIndex = 11;
            label2.Text = "Destination:";
            // 
            // sourceLabel
            // 
            sourceLabel.AutoSize = true;
            sourceLabel.Location = new Point(114, 63);
            sourceLabel.Margin = new Padding(4, 0, 4, 0);
            sourceLabel.Name = "sourceLabel";
            sourceLabel.Size = new Size(10, 15);
            sourceLabel.TabIndex = 12;
            sourceLabel.Text = ".";
            // 
            // destinationLabel
            // 
            destinationLabel.AutoSize = true;
            destinationLabel.Location = new Point(115, 117);
            destinationLabel.Margin = new Padding(4, 0, 4, 0);
            destinationLabel.Name = "destinationLabel";
            destinationLabel.Size = new Size(10, 15);
            destinationLabel.TabIndex = 13;
            destinationLabel.Text = ".";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(111, 221);
            progressBar.Margin = new Padding(4, 3, 4, 3);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(377, 23);
            progressBar.TabIndex = 14;
            // 
            // btnDeleteSource
            // 
            btnDeleteSource.BackColor = SystemColors.HighlightText;
            btnDeleteSource.Font = new Font("Segoe UI", 12F);
            btnDeleteSource.Image = (Image)resources.GetObject("btnDeleteSource.Image");
            btnDeleteSource.Location = new Point(496, 28);
            btnDeleteSource.Margin = new Padding(4, 3, 4, 3);
            btnDeleteSource.Name = "btnDeleteSource";
            btnDeleteSource.Size = new Size(38, 30);
            btnDeleteSource.TabIndex = 15;
            btnDeleteSource.UseVisualStyleBackColor = false;
            btnDeleteSource.Click += BtnDeleteSource_Click;
            // 
            // btnRefreshSource
            // 
            btnRefreshSource.BackColor = SystemColors.HighlightText;
            btnRefreshSource.Font = new Font("Segoe UI", 12F);
            btnRefreshSource.Image = (Image)resources.GetObject("btnRefreshSource.Image");
            btnRefreshSource.Location = new Point(542, 29);
            btnRefreshSource.Margin = new Padding(4, 3, 4, 3);
            btnRefreshSource.Name = "btnRefreshSource";
            btnRefreshSource.Size = new Size(38, 29);
            btnRefreshSource.TabIndex = 18;
            btnRefreshSource.UseVisualStyleBackColor = false;
            btnRefreshSource.Click += BtnRefreshSource_Click;
            // 
            // messageLabel
            // 
            messageLabel.AutoSize = true;
            messageLabel.Location = new Point(111, 282);
            messageLabel.Margin = new Padding(4, 0, 4, 0);
            messageLabel.Name = "messageLabel";
            messageLabel.Size = new Size(42, 15);
            messageLabel.TabIndex = 20;
            messageLabel.Text = "Status:";
            // 
            // tableProgressBar
            // 
            tableProgressBar.Location = new Point(111, 250);
            tableProgressBar.Margin = new Padding(4, 3, 4, 3);
            tableProgressBar.Name = "tableProgressBar";
            tableProgressBar.Size = new Size(377, 23);
            tableProgressBar.TabIndex = 21;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(10, 229);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(93, 15);
            label3.TabIndex = 22;
            label3.Text = "Row Transaction";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(4, 258);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(97, 15);
            label4.TabIndex = 23;
            label4.Text = "Table Transaction";
            // 
            // btnRefreshDestination
            // 
            btnRefreshDestination.BackColor = SystemColors.HighlightText;
            btnRefreshDestination.Font = new Font("Segoe UI", 12F);
            btnRefreshDestination.Image = (Image)resources.GetObject("btnRefreshDestination.Image");
            btnRefreshDestination.Location = new Point(542, 85);
            btnRefreshDestination.Margin = new Padding(4, 3, 4, 3);
            btnRefreshDestination.Name = "btnRefreshDestination";
            btnRefreshDestination.Size = new Size(38, 29);
            btnRefreshDestination.TabIndex = 25;
            btnRefreshDestination.UseVisualStyleBackColor = false;
            btnRefreshDestination.Click += btnRefreshDestination_Click;
            // 
            // btnDeleteDestination
            // 
            btnDeleteDestination.BackColor = SystemColors.HighlightText;
            btnDeleteDestination.Font = new Font("Segoe UI", 12F);
            btnDeleteDestination.Image = (Image)resources.GetObject("btnDeleteDestination.Image");
            btnDeleteDestination.Location = new Point(496, 84);
            btnDeleteDestination.Margin = new Padding(4, 3, 4, 3);
            btnDeleteDestination.Name = "btnDeleteDestination";
            btnDeleteDestination.Size = new Size(38, 30);
            btnDeleteDestination.TabIndex = 24;
            btnDeleteDestination.UseVisualStyleBackColor = false;
            btnDeleteDestination.Click += btnDeleteDestination_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Info;
            ClientSize = new Size(599, 316);
            Controls.Add(btnRefreshDestination);
            Controls.Add(btnDeleteDestination);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(tableProgressBar);
            Controls.Add(messageLabel);
            Controls.Add(btnRefreshSource);
            Controls.Add(btnDeleteSource);
            Controls.Add(progressBar);
            Controls.Add(destinationLabel);
            Controls.Add(sourceLabel);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(addConnectionStringButton);
            Controls.Add(transferButton);
            Controls.Add(destinationComboBox);
            Controls.Add(sourceComboBox);
            Margin = new Padding(4, 3, 4, 3);
            Name = "Form1";
            Text = "Database Sync";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ComboBox sourceComboBox;
        private System.Windows.Forms.ComboBox destinationComboBox;
        private System.Windows.Forms.Button transferButton;
        private System.Windows.Forms.Button addConnectionStringButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label sourceLabel;
        private System.Windows.Forms.Label destinationLabel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnDeleteSource;
        private System.Windows.Forms.Button btnRefreshSource;
        private Label label3;
        private Label messageLabel;
        private ProgressBar tableProgressBar;
        private Label label4;
        private Button btnRefreshDestination;
        private Button btnDeleteDestination;
    }
}
