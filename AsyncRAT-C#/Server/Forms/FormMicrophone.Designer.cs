namespace Server.Forms
{
    partial class FormMicrophone
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
            this.components = new System.ComponentModel.Container();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.StartListen = new System.Windows.Forms.Button();
            this.StopListen = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.RefreshMics = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 38);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(362, 145);
            this.listBox1.TabIndex = 0;
            // 
            // StartListen
            // 
            this.StartListen.Location = new System.Drawing.Point(12, 189);
            this.StartListen.Name = "StartListen";
            this.StartListen.Size = new System.Drawing.Size(97, 27);
            this.StartListen.TabIndex = 1;
            this.StartListen.Text = "Start";
            this.StartListen.UseVisualStyleBackColor = true;
            this.StartListen.Click += new System.EventHandler(this.StartListen_Click);
            // 
            // StopListen
            // 
            this.StopListen.Enabled = false;
            this.StopListen.Location = new System.Drawing.Point(12, 222);
            this.StopListen.Name = "StopListen";
            this.StopListen.Size = new System.Drawing.Size(97, 27);
            this.StopListen.TabIndex = 2;
            this.StopListen.Text = "Stop";
            this.StopListen.UseVisualStyleBackColor = true;
            this.StopListen.Visible = false;
            this.StopListen.Click += new System.EventHandler(this.StopListen_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(115, 204);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(112, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(245, 204);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(91, 21);
            this.comboBox2.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(115, 189);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Rate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 189);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Audio Format";
            // 
            // RefreshMics
            // 
            this.RefreshMics.Location = new System.Drawing.Point(12, 5);
            this.RefreshMics.Name = "RefreshMics";
            this.RefreshMics.Size = new System.Drawing.Size(69, 27);
            this.RefreshMics.TabIndex = 7;
            this.RefreshMics.Text = "Refresh";
            this.RefreshMics.UseVisualStyleBackColor = true;
            this.RefreshMics.Click += new System.EventHandler(this.RefreshMics_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormMicrophone
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 261);
            this.Controls.Add(this.RefreshMics);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.StopListen);
            this.Controls.Add(this.StartListen);
            this.Controls.Add(this.listBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMicrophone";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Microphone";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMicrophone_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button StartListen;
        private System.Windows.Forms.Button StopListen;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button RefreshMics;
        public System.Windows.Forms.ListBox listBox1;
        public System.Windows.Forms.Timer timer1;
    }
}