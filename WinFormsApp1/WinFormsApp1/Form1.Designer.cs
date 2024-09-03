namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button sendButton;

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
            button1 = new Button();
            label1 = new Label();
            sendButton = new Button();
            textBox1 = new TextBox();
            stopButton = new Button();
            label2 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 12F);
            button1.Location = new Point(583, 29);
            button1.Name = "button1";
            button1.Size = new Size(186, 54);
            button1.TabIndex = 0;
            button1.Text = "choose file";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F);
            label1.Location = new Point(29, 55);
            label1.Name = "label1";
            label1.Size = new Size(0, 20);
            label1.TabIndex = 1;
            // 
            // sendButton
            // 
            sendButton.BackColor = SystemColors.ActiveCaption;
            sendButton.Location = new Point(232, 177);
            sendButton.Name = "sendButton";
            sendButton.Size = new Size(98, 41);
            sendButton.TabIndex = 1;
            sendButton.Text = "Send";
            sendButton.UseVisualStyleBackColor = false;
            sendButton.Click += sendButton_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(30, 291);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(740, 27);
            textBox1.TabIndex = 2;
            // 
            // stopButton
            // 
            stopButton.BackColor = Color.DarkRed;
            stopButton.Location = new Point(427, 177);
            stopButton.Name = "stopButton";
            stopButton.Size = new Size(103, 41);
            stopButton.TabIndex = 3;
            stopButton.Text = "Stop";
            stopButton.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(674, 198);
            label2.Name = "label2";
            label2.Size = new Size(0, 20);
            label2.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(798, 332);
            Controls.Add(label2);
            Controls.Add(stopButton);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(sendButton);
            Name = "Form1";
            Text = "intraNET";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private TextBox textBox1;
        private Button stopButton;
        private Label label2;
    }
}
