namespace RedRP
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Discory = new System.Windows.Forms.Button();
            this.textBoxInfo = new System.Windows.Forms.TextBox();
            this.PairingBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textBoxUserId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Discory
            // 
            this.Discory.Location = new System.Drawing.Point(10, 26);
            this.Discory.Name = "Discory";
            this.Discory.Size = new System.Drawing.Size(75, 21);
            this.Discory.TabIndex = 0;
            this.Discory.Text = "查找PS4";
            this.Discory.UseVisualStyleBackColor = true;
            this.Discory.Click += new System.EventHandler(this.Discory_Click);
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.Location = new System.Drawing.Point(-2, 48);
            this.textBoxInfo.Multiline = true;
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.Size = new System.Drawing.Size(262, 308);
            this.textBoxInfo.TabIndex = 1;
            // 
            // PairingBtn
            // 
            this.PairingBtn.Location = new System.Drawing.Point(91, 25);
            this.PairingBtn.Name = "PairingBtn";
            this.PairingBtn.Size = new System.Drawing.Size(75, 21);
            this.PairingBtn.TabIndex = 2;
            this.PairingBtn.Text = " 配对PS4";
            this.PairingBtn.UseVisualStyleBackColor = true;
            this.PairingBtn.Click += new System.EventHandler(this.PairingBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(27, 4);
            this.textBoxIP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(88, 21);
            this.textBoxIP.TabIndex = 4;
            this.textBoxIP.Text = "192.168.3.73";
            // 
            // textBoxUserId
            // 
            this.textBoxUserId.Location = new System.Drawing.Point(163, 3);
            this.textBoxUserId.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxUserId.Name = "textBoxUserId";
            this.textBoxUserId.Size = new System.Drawing.Size(98, 21);
            this.textBoxUserId.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(118, 7);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "User Id";
            // 
            // textBoxPin
            // 
            this.textBoxPin.Location = new System.Drawing.Point(198, 26);
            this.textBoxPin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxPin.Name = "textBoxPin";
            this.textBoxPin.Size = new System.Drawing.Size(62, 21);
            this.textBoxPin.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(171, 28);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "PIN";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 361);
            this.Controls.Add(this.textBoxPin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxUserId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PairingBtn);
            this.Controls.Add(this.textBoxInfo);
            this.Controls.Add(this.Discory);
            this.Name = "MainForm";
            this.Text = "Read RP 3.1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Discory;
        private System.Windows.Forms.TextBox textBoxInfo;
        private System.Windows.Forms.Button PairingBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.TextBox textBoxUserId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPin;
        private System.Windows.Forms.Label label3;
    }
}

