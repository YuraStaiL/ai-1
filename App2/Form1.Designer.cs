﻿namespace App2
{
    partial class Hopfield
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
            AddClass = new Button();
            binarizeTextBox = new TextBox();
            label3 = new Label();
            dataGridView = new DataGridView();
            dataGridView2 = new DataGridView();
            trainButton = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(677, 23);
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
            btnConvert.Location = new Point(677, 80);
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
            txtThreshold.Location = new Point(677, 151);
            txtThreshold.Name = "txtThreshold";
            txtThreshold.Size = new Size(179, 43);
            txtThreshold.TabIndex = 3;
            txtThreshold.Text = "177";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15F);
            label1.Location = new Point(677, 123);
            label1.Name = "label1";
            label1.Size = new Size(141, 28);
            label1.TabIndex = 4;
            label1.Text = "Threshold B/W";
            // 
            // button2
            // 
            button2.Location = new Point(862, 23);
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
            sectorsNumber.Location = new Point(862, 151);
            sectorsNumber.Name = "sectorsNumber";
            sectorsNumber.Size = new Size(179, 43);
            sectorsNumber.TabIndex = 6;
            sectorsNumber.Text = "6";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15F);
            label2.Location = new Point(862, 123);
            label2.Name = "label2";
            label2.Size = new Size(173, 28);
            label2.TabIndex = 7;
            label2.Text = "Number of sectors";
            // 
            // button3
            // 
            button3.Location = new Point(862, 80);
            button3.Name = "button3";
            button3.Size = new Size(179, 40);
            button3.TabIndex = 8;
            button3.Text = "Find Class";
            button3.UseVisualStyleBackColor = true;
            button3.Click += buttonSegment_Click;
            // 
            // logRichTextBox
            // 
            logRichTextBox.Font = new Font("Segoe UI", 11F);
            logRichTextBox.Location = new Point(732, 293);
            logRichTextBox.Name = "logRichTextBox";
            logRichTextBox.Size = new Size(735, 416);
            logRichTextBox.TabIndex = 10;
            logRichTextBox.Text = "";
            // 
            // AddClass
            // 
            AddClass.Location = new Point(677, 234);
            AddClass.Name = "AddClass";
            AddClass.Size = new Size(169, 44);
            AddClass.TabIndex = 12;
            AddClass.Text = "Add class";
            AddClass.UseVisualStyleBackColor = true;
            AddClass.Click += AddClass_Click;
            // 
            // binarizeTextBox
            // 
            binarizeTextBox.Font = new Font("Segoe UI", 20F);
            binarizeTextBox.Location = new Point(862, 234);
            binarizeTextBox.Name = "binarizeTextBox";
            binarizeTextBox.Size = new Size(179, 43);
            binarizeTextBox.TabIndex = 13;
            binarizeTextBox.Text = "0,5";
            binarizeTextBox.TextChanged += binarizeTextBox_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 15F);
            label3.Location = new Point(862, 203);
            label3.Name = "label3";
            label3.Size = new Size(80, 28);
            label3.TabIndex = 14;
            label3.Text = "Binarize";
            label3.Click += label3_Click;
            // 
            // dataGridView
            // 
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(12, 414);
            dataGridView.Name = "dataGridView";
            dataGridView.Size = new Size(692, 231);
            dataGridView.TabIndex = 15;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(1047, 265);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.Size = new Size(32, 12);
            dataGridView2.TabIndex = 16;
            // 
            // trainButton
            // 
            trainButton.Location = new Point(1047, 80);
            trainButton.Name = "trainButton";
            trainButton.Size = new Size(141, 40);
            trainButton.TabIndex = 17;
            trainButton.Text = "Train";
            trainButton.UseVisualStyleBackColor = true;
            trainButton.Click += trainButton_Click;
            // 
            // Hopfield
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1471, 721);
            Controls.Add(trainButton);
            Controls.Add(dataGridView);
            Controls.Add(label3);
            Controls.Add(binarizeTextBox);
            Controls.Add(AddClass);
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
            Controls.Add(dataGridView2);
            Name = "Hopfield";
            Text = "Hopfield Network";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
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
        private Button AddClass;
        private TextBox binarizeTextBox;
        private Label label3;
        private DataGridView dataGridView;
        private DataGridView dataGridView2;
        private Button trainButton;
    }
}
