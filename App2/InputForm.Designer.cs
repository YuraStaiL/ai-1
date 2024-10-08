namespace App2
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
            classNameTextBox = new TextBox();
            classNameLabel = new Label();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Font = new Font("Segoe UI", 15F);
            btnOK.Location = new Point(120, 103);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(100, 38);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // classNameTextBox
            // 
            classNameTextBox.Font = new Font("Segoe UI", 17F);
            classNameTextBox.Location = new Point(48, 59);
            classNameTextBox.Name = "classNameTextBox";
            classNameTextBox.Size = new Size(249, 38);
            classNameTextBox.TabIndex = 1;
            // 
            // classNameLabel
            // 
            classNameLabel.AutoSize = true;
            classNameLabel.Font = new Font("Segoe UI", 18F);
            classNameLabel.Location = new Point(106, 24);
            classNameLabel.Name = "classNameLabel";
            classNameLabel.Size = new Size(134, 32);
            classNameLabel.TabIndex = 2;
            classNameLabel.Text = "Class name";
            // 
            // InputForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(351, 153);
            Controls.Add(classNameLabel);
            Controls.Add(classNameTextBox);
            Controls.Add(btnOK);
            Name = "InputForm";
            Text = "InputForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOK;
        private TextBox classNameTextBox;
        private Label classNameLabel;
    }
}