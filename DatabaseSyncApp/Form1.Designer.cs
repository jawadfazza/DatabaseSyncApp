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
            this.sourceComboBox = new System.Windows.Forms.ComboBox();
            this.destinationComboBox = new System.Windows.Forms.ComboBox();
            this.transferButton = new System.Windows.Forms.Button();
            this.addConnectionStringButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sourceLabel = new System.Windows.Forms.Label();
            this.destinationLabel = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // sourceComboBox
            // 
            this.sourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceComboBox.FormattingEnabled = true;
            this.sourceComboBox.Location = new System.Drawing.Point(103, 29);
            this.sourceComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sourceComboBox.Name = "sourceComboBox";
            this.sourceComboBox.Size = new System.Drawing.Size(377, 23);
            this.sourceComboBox.TabIndex = 0;
            this.sourceComboBox.SelectedIndexChanged += new System.EventHandler(this.sourceComboBox_SelectedIndexChanged);
            // 
            // destinationComboBox
            // 
            this.destinationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.destinationComboBox.FormattingEnabled = true;
            this.destinationComboBox.Location = new System.Drawing.Point(103, 78);
            this.destinationComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.destinationComboBox.Name = "destinationComboBox";
            this.destinationComboBox.Size = new System.Drawing.Size(377, 23);
            this.destinationComboBox.TabIndex = 1;
            this.destinationComboBox.SelectedIndexChanged += new System.EventHandler(this.DestinationComboBox_SelectedIndexChanged);
            // 
            // transferButton
            // 
            this.transferButton.Location = new System.Drawing.Point(103, 172);
            this.transferButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.transferButton.Name = "transferButton";
            this.transferButton.Size = new System.Drawing.Size(377, 27);
            this.transferButton.TabIndex = 6;
            this.transferButton.Text = "Transfer";
            this.transferButton.UseVisualStyleBackColor = true;
            this.transferButton.Click += new System.EventHandler(this.TransferButton_Click);
            // 
            // addConnectionStringButton
            // 
            this.addConnectionStringButton.Location = new System.Drawing.Point(103, 130);
            this.addConnectionStringButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.addConnectionStringButton.Name = "addConnectionStringButton";
            this.addConnectionStringButton.Size = new System.Drawing.Size(377, 27);
            this.addConnectionStringButton.TabIndex = 7;
            this.addConnectionStringButton.Text = "Configure New Connection";
            this.addConnectionStringButton.UseVisualStyleBackColor = true;
            this.addConnectionStringButton.Click += new System.EventHandler(this.AddConnectionStringButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "Source:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 78);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "Destination:";
            // 
            // sourceLabel
            // 
            this.sourceLabel.AutoSize = true;
            this.sourceLabel.Location = new System.Drawing.Point(106, 57);
            this.sourceLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.sourceLabel.Name = "sourceLabel";
            this.sourceLabel.Size = new System.Drawing.Size(10, 15);
            this.sourceLabel.TabIndex = 12;
            this.sourceLabel.Text = ".";
            // 
            // destinationLabel
            // 
            this.destinationLabel.AutoSize = true;
            this.destinationLabel.Location = new System.Drawing.Point(107, 106);
            this.destinationLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.destinationLabel.Name = "destinationLabel";
            this.destinationLabel.Size = new System.Drawing.Size(10, 15);
            this.destinationLabel.TabIndex = 13;
            this.destinationLabel.Text = ".";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(103, 214);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(377, 23);
            this.progressBar.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 249);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.destinationLabel);
            this.Controls.Add(this.sourceLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addConnectionStringButton);
            this.Controls.Add(this.transferButton);
            this.Controls.Add(this.destinationComboBox);
            this.Controls.Add(this.sourceComboBox);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "Database Sync";
            this.ResumeLayout(false);
            this.PerformLayout();
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
    }
}
