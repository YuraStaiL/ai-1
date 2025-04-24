namespace AI_2_Perceptron
{
    partial class InputDialog
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
            btnOK = new Button();
            confirmationMsg = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Font = new Font("Segoe UI", 15F);
            btnOK.Location = new Point(48, 103);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(100, 38);
            btnOK.TabIndex = 0;
            btnOK.Text = "YES";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // confirmationMsg
            // 
            confirmationMsg.AutoSize = true;
            confirmationMsg.Font = new Font("Segoe UI", 26.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            confirmationMsg.Location = new Point(48, 28);
            confirmationMsg.Name = "confirmationMsg";
            confirmationMsg.Size = new Size(109, 47);
            confirmationMsg.TabIndex = 2;
            confirmationMsg.Text = "Is this";
            confirmationMsg.Click += classNameLabel_Click;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 15F);
            button1.Location = new Point(183, 103);
            button1.Name = "button1";
            button1.Size = new Size(100, 38);
            button1.TabIndex = 3;
            button1.Text = "NO";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // InputDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(366, 153);
            Controls.Add(button1);
            Controls.Add(confirmationMsg);
            Controls.Add(btnOK);
            Name = "InputDialog";
            Text = "Classify result";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOK;
        private Label confirmationMsg;
        private Button button1;
    }
}