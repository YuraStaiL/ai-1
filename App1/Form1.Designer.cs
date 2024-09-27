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
            sectorsNumber = new TextBox();
            label2 = new Label();
            button3 = new Button();
            logRichTextBox = new RichTextBox();
            button4 = new Button();
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
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(314, 314);
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
            txtThreshold.Text = "177";
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
            // sectorsNumber
            // 
            sectorsNumber.Font = new Font("Segoe UI", 20F);
            sectorsNumber.Location = new Point(862, 172);
            sectorsNumber.Name = "sectorsNumber";
            sectorsNumber.Size = new Size(179, 43);
            sectorsNumber.TabIndex = 6;
            sectorsNumber.Text = "6";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15F);
            label2.Location = new Point(862, 144);
            label2.Name = "label2";
            label2.Size = new Size(173, 28);
            label2.TabIndex = 7;
            label2.Text = "Number of sectors";
            // 
            // button3
            // 
            button3.Location = new Point(862, 101);
            button3.Name = "button3";
            button3.Size = new Size(179, 40);
            button3.TabIndex = 8;
            button3.Text = "Segment";
            button3.UseVisualStyleBackColor = true;
            button3.Click += buttonSegment_Click;
            // 
            // logRichTextBox
            // 
            logRichTextBox.Font = new Font("Segoe UI", 11F);
            logRichTextBox.Location = new Point(492, 293);
            logRichTextBox.Name = "logRichTextBox";
            logRichTextBox.Size = new Size(549, 254);
            logRichTextBox.TabIndex = 10;
            logRichTextBox.Text = "";
            // 
            // button4
            // 
            button4.Location = new Point(862, 236);
            button4.Name = "button4";
            button4.Size = new Size(179, 40);
            button4.TabIndex = 11;
            button4.Text = "Fill Segment";
            button4.UseVisualStyleBackColor = true;
            button4.Click += buttonFillSegment_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1088, 586);
            Controls.Add(button4);
            Controls.Add(logRichTextBox);
            Controls.Add(button3);
            Controls.Add(label2);
            Controls.Add(sectorsNumber);
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
        private TextBox sectorsNumber;
        private Label label2;
        private Button button3;
        private RichTextBox logRichTextBox;
        private Button button4;
    }
}
