namespace App4
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
            savePattern = new Button();
            findPattern = new Button();
            pictureBoxTraining = new PictureBox();
            pictureBoxTest = new PictureBox();
            logRichTextBox = new RichTextBox();
            textBoxSeparationDistance = new TextBox();
            label1 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTraining).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTest).BeginInit();
            SuspendLayout();
            // 
            // savePattern
            // 
            savePattern.Font = new Font("Segoe UI", 15F);
            savePattern.Location = new Point(178, 497);
            savePattern.Name = "savePattern";
            savePattern.Size = new Size(187, 66);
            savePattern.TabIndex = 0;
            savePattern.Text = "Save pattern";
            savePattern.UseVisualStyleBackColor = true;
            savePattern.Click += savePattern_Click;
            // 
            // findPattern
            // 
            findPattern.Font = new Font("Segoe UI", 15F);
            findPattern.Location = new Point(722, 497);
            findPattern.Name = "findPattern";
            findPattern.Size = new Size(187, 66);
            findPattern.TabIndex = 1;
            findPattern.Text = "Find pattern";
            findPattern.UseVisualStyleBackColor = true;
            findPattern.Click += findPattern_Click;
            // 
            // pictureBoxTraining
            // 
            pictureBoxTraining.Location = new Point(47, 28);
            pictureBoxTraining.Name = "pictureBoxTraining";
            pictureBoxTraining.Size = new Size(487, 454);
            pictureBoxTraining.TabIndex = 2;
            pictureBoxTraining.TabStop = false;
            // 
            // pictureBoxTest
            // 
            pictureBoxTest.Location = new Point(540, 28);
            pictureBoxTest.Name = "pictureBoxTest";
            pictureBoxTest.Size = new Size(487, 454);
            pictureBoxTest.TabIndex = 3;
            pictureBoxTest.TabStop = false;
            // 
            // logRichTextBox
            // 
            logRichTextBox.Font = new Font("Consolas", 15F, FontStyle.Regular, GraphicsUnit.Point, 204);
            logRichTextBox.Location = new Point(47, 581);
            logRichTextBox.Name = "logRichTextBox";
            logRichTextBox.Size = new Size(980, 159);
            logRichTextBox.TabIndex = 4;
            logRichTextBox.Text = "";
            // 
            // textBoxSeparationDistance
            // 
            textBoxSeparationDistance.Font = new Font("Segoe UI", 15F);
            textBoxSeparationDistance.Location = new Point(448, 513);
            textBoxSeparationDistance.Name = "textBoxSeparationDistance";
            textBoxSeparationDistance.Size = new Size(185, 34);
            textBoxSeparationDistance.TabIndex = 5;
            textBoxSeparationDistance.Text = "40";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15F);
            label1.Location = new Point(629, 519);
            label1.Name = "label1";
            label1.Size = new Size(33, 28);
            label1.TabIndex = 6;
            label1.Text = "px";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15F);
            label2.Location = new Point(448, 550);
            label2.Name = "label2";
            label2.Size = new Size(186, 28);
            label2.TabIndex = 7;
            label2.Text = "Separation Distance";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1072, 743);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBoxSeparationDistance);
            Controls.Add(logRichTextBox);
            Controls.Add(pictureBoxTest);
            Controls.Add(pictureBoxTraining);
            Controls.Add(findPattern);
            Controls.Add(savePattern);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxTraining).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTest).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button savePattern;
        private Button findPattern;
        private PictureBox pictureBoxTraining;
        private PictureBox pictureBoxTest;
        private RichTextBox logRichTextBox;
        private TextBox textBoxSeparationDistance;
        private Label label1;
        private Label label2;
    }
}
