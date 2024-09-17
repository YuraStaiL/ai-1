namespace App1
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
            button1 = new Button();
            pictureBox1 = new PictureBox();
            btnConvert = new Button();
            txtThreshold = new TextBox();
            label1 = new Label();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(677, 44);
            button1.Name = "button1";
            button1.Size = new Size(179, 40);
            button1.TabIndex = 0;
            button1.Text = "Upload";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnLoadImage_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(60, 44);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(529, 503);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.Paint += pictureBox1_Paint;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            // 
            // btnConvert
            // 
            btnConvert.Location = new Point(677, 101);
            btnConvert.Name = "btnConvert";
            btnConvert.Size = new Size(179, 40);
            btnConvert.TabIndex = 2;
            btnConvert.Text = "Convert to Grayscale and B/W";
            btnConvert.UseVisualStyleBackColor = true;
            btnConvert.Click += btnConvert_Click;
            // 
            // txtThreshold
            // 
            txtThreshold.Font = new Font("Segoe UI", 20F);
            txtThreshold.Location = new Point(677, 172);
            txtThreshold.Name = "txtThreshold";
            txtThreshold.Size = new Size(179, 43);
            txtThreshold.TabIndex = 3;
            txtThreshold.Text = "123";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15F);
            label1.Location = new Point(677, 144);
            label1.Name = "label1";
            label1.Size = new Size(141, 28);
            label1.TabIndex = 4;
            label1.Text = "Threshold B/W";
            // 
            // button2
            // 
            button2.Location = new Point(862, 44);
            button2.Name = "button2";
            button2.Size = new Size(179, 40);
            button2.TabIndex = 5;
            button2.Text = "Restore img";
            button2.UseVisualStyleBackColor = true;
            button2.Click += btnRestoreOriginalImg_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1421, 785);
            Controls.Add(button2);
            Controls.Add(label1);
            Controls.Add(txtThreshold);
            Controls.Add(btnConvert);
            Controls.Add(pictureBox1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Lab 1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private PictureBox pictureBox1;
        private Button btnConvert;
        private TextBox txtThreshold;
        private Label label1;
        private Button button2;
    }
}
