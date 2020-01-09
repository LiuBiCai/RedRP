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
            this.SuspendLayout();
            // 
            // Discory
            // 
            this.Discory.Location = new System.Drawing.Point(364, 12);
            this.Discory.Name = "Discory";
            this.Discory.Size = new System.Drawing.Size(75, 23);
            this.Discory.TabIndex = 0;
            this.Discory.Text = "查找PS4";
            this.Discory.UseVisualStyleBackColor = true;
            this.Discory.Click += new System.EventHandler(this.Discory_Click);
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.Location = new System.Drawing.Point(12, 15);
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.Size = new System.Drawing.Size(346, 20);
            this.textBoxInfo.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 48);
            this.Controls.Add(this.textBoxInfo);
            this.Controls.Add(this.Discory);
            this.Name = "MainForm";
            this.Text = "Read RP 1.0";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Discory;
        private System.Windows.Forms.TextBox textBoxInfo;
    }
}

