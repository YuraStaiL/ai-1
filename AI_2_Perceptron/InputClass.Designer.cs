namespace AI_2_Perceptron
{
    partial class InputClass
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
            btnUploadMultiple = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            btnSegment = new Button();
            SuspendLayout();
            // 
            // btnUploadMultiple
            // 
            btnUploadMultiple.Font = new Font("Segoe UI", 15F);
            btnUploadMultiple.Location = new Point(1112, 944);
            btnUploadMultiple.Name = "btnUploadMultiple";
            btnUploadMultiple.Size = new Size(238, 61);
            btnUploadMultiple.TabIndex = 0;
            btnUploadMultiple.Text = "Upload multiple";
            btnUploadMultiple.UseVisualStyleBackColor = true;
            btnUploadMultiple.Click += btnUploadMultiple_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Location = new Point(180, 12);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1600, 900);
            flowLayoutPanel1.TabIndex = 1;
            flowLayoutPanel1.Paint += flowLayoutPanel1_Paint;
            // 
            // btnSegment
            // 
            btnSegment.Font = new Font("Segoe UI", 15F);
            btnSegment.Location = new Point(859, 946);
            btnSegment.Name = "btnSegment";
            btnSegment.Size = new Size(212, 59);
            btnSegment.TabIndex = 2;
            btnSegment.Text = "Segment";
            btnSegment.UseVisualStyleBackColor = true;
            btnSegment.Click += btnSegment_Click;
            // 
            // InputClass
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1904, 1041);
            Controls.Add(btnSegment);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(btnUploadMultiple);
            Name = "InputClass";
            Text = "InputClass";
            ResumeLayout(false);
        }

        #endregion

        private Button btnUploadMultiple;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnSegment;
    }
}