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
            trainPicture = new PictureBox();
            testPicture = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)trainPicture).BeginInit();
            ((System.ComponentModel.ISupportInitialize)testPicture).BeginInit();
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
            // trainPicture
            // 
            trainPicture.Location = new Point(47, 28);
            trainPicture.Name = "trainPicture";
            trainPicture.Size = new Size(487, 454);
            trainPicture.TabIndex = 2;
            trainPicture.TabStop = false;
            // 
            // testPicture
            // 
            testPicture.Location = new Point(540, 28);
            testPicture.Name = "testPicture";
            testPicture.Size = new Size(487, 454);
            testPicture.TabIndex = 3;
            testPicture.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1072, 575);
            Controls.Add(testPicture);
            Controls.Add(trainPicture);
            Controls.Add(findPattern);
            Controls.Add(savePattern);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)trainPicture).EndInit();
            ((System.ComponentModel.ISupportInitialize)testPicture).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button savePattern;
        private Button findPattern;
        private PictureBox trainPicture;
        private PictureBox testPicture;
    }
}
