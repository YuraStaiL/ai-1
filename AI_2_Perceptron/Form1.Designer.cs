namespace AI_2_Perceptron
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
            pictureBox1 = new PictureBox();
            txtThreshold = new TextBox();
            label1 = new Label();
            logRichTextBox = new RichTextBox();
            Сlassify = new Button();
            TrainPers = new Button();
            epochCnt = new TextBox();
            label3 = new Label();
            secondClassName = new TextBox();
            label4 = new Label();
            firstClassName = new TextBox();
            label5 = new Label();
            learningRateValue = new TextBox();
            label6 = new Label();
            sectorsNumber = new TextBox();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(314, 314);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            pictureBox1.Paint += pictureBox1_Paint;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            // 
            // txtThreshold
            // 
            txtThreshold.Font = new Font("Segoe UI", 20F);
            txtThreshold.Location = new Point(353, 140);
            txtThreshold.Name = "txtThreshold";
            txtThreshold.Size = new Size(179, 43);
            txtThreshold.TabIndex = 3;
            txtThreshold.Text = "177";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15F);
            label1.Location = new Point(353, 112);
            label1.Name = "label1";
            label1.Size = new Size(141, 28);
            label1.TabIndex = 4;
            label1.Text = "Threshold B/W";
            // 
            // logRichTextBox
            // 
            logRichTextBox.Font = new Font("Segoe UI", 11F);
            logRichTextBox.Location = new Point(12, 349);
            logRichTextBox.Name = "logRichTextBox";
            logRichTextBox.Size = new Size(1034, 252);
            logRichTextBox.TabIndex = 10;
            logRichTextBox.Text = "";
            // 
            // Сlassify
            // 
            Сlassify.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 204);
            Сlassify.Location = new Point(538, 204);
            Сlassify.Name = "Сlassify";
            Сlassify.Size = new Size(179, 40);
            Сlassify.TabIndex = 11;
            Сlassify.Text = "Сlassify";
            Сlassify.UseVisualStyleBackColor = true;
            Сlassify.Click += buttonFillSegment_Click;
            // 
            // TrainPers
            // 
            TrainPers.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 204);
            TrainPers.Location = new Point(353, 202);
            TrainPers.Name = "TrainPers";
            TrainPers.Size = new Size(169, 44);
            TrainPers.TabIndex = 12;
            TrainPers.Text = "Train";
            TrainPers.UseVisualStyleBackColor = true;
            TrainPers.Click += AddClass_Click;
            // 
            // epochCnt
            // 
            epochCnt.Font = new Font("Segoe UI", 20F);
            epochCnt.Location = new Point(723, 140);
            epochCnt.Name = "epochCnt";
            epochCnt.Size = new Size(179, 43);
            epochCnt.TabIndex = 14;
            epochCnt.Text = "1";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 15F);
            label3.Location = new Point(723, 109);
            label3.Name = "label3";
            label3.Size = new Size(74, 28);
            label3.TabIndex = 15;
            label3.Text = "Epochs";
            label3.Click += label3_Click;
            // 
            // secondClassName
            // 
            secondClassName.Font = new Font("Segoe UI", 20F);
            secondClassName.Location = new Point(538, 283);
            secondClassName.Name = "secondClassName";
            secondClassName.Size = new Size(179, 43);
            secondClassName.TabIndex = 16;
            secondClassName.Text = "letter K";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 15F);
            label4.Location = new Point(353, 252);
            label4.Name = "label4";
            label4.Size = new Size(121, 28);
            label4.TabIndex = 17;
            label4.Text = "class 1 name";
            // 
            // firstClassName
            // 
            firstClassName.Font = new Font("Segoe UI", 20F);
            firstClassName.Location = new Point(353, 283);
            firstClassName.Name = "firstClassName";
            firstClassName.Size = new Size(179, 43);
            firstClassName.TabIndex = 18;
            firstClassName.Text = "letter A";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 15F);
            label5.Location = new Point(538, 252);
            label5.Name = "label5";
            label5.Size = new Size(121, 28);
            label5.TabIndex = 19;
            label5.Text = "class 2 name";
            // 
            // learningRateValue
            // 
            learningRateValue.Font = new Font("Segoe UI", 20F);
            learningRateValue.Location = new Point(723, 283);
            learningRateValue.Name = "learningRateValue";
            learningRateValue.Size = new Size(179, 43);
            learningRateValue.TabIndex = 20;
            learningRateValue.Text = "0,33";
            learningRateValue.TextChanged += learningRateValue_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 15F);
            label6.Location = new Point(723, 252);
            label6.Name = "label6";
            label6.Size = new Size(126, 28);
            label6.TabIndex = 21;
            label6.Text = "Learning rate";
            // 
            // sectorsNumber
            // 
            sectorsNumber.Font = new Font("Segoe UI", 20F);
            sectorsNumber.Location = new Point(538, 140);
            sectorsNumber.Name = "sectorsNumber";
            sectorsNumber.Size = new Size(179, 43);
            sectorsNumber.TabIndex = 22;
            sectorsNumber.Text = "6";
            sectorsNumber.TextChanged += sectorNumbers_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15F);
            label2.Location = new Point(538, 109);
            label2.Name = "label2";
            label2.Size = new Size(76, 28);
            label2.TabIndex = 23;
            label2.Text = "Sectors";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1049, 628);
            Controls.Add(label2);
            Controls.Add(sectorsNumber);
            Controls.Add(label6);
            Controls.Add(learningRateValue);
            Controls.Add(label5);
            Controls.Add(firstClassName);
            Controls.Add(label4);
            Controls.Add(secondClassName);
            Controls.Add(label3);
            Controls.Add(epochCnt);
            Controls.Add(TrainPers);
            Controls.Add(Сlassify);
            Controls.Add(logRichTextBox);
            Controls.Add(label1);
            Controls.Add(txtThreshold);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Perceptron";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PictureBox pictureBox1;
        private TextBox txtThreshold;
        private Label label1;
        private RichTextBox logRichTextBox;
        private Button Сlassify;
        private Button TrainPers;
        private TextBox epochCnt;
        private Label label3;
        private TextBox secondClassName;
        private Label label4;
        private TextBox firstClassName;
        private Label label5;
        private TextBox learningRateValue;
        private Label label6;
        private TextBox sectorsNumber;
        private Label label2;
    }
}
