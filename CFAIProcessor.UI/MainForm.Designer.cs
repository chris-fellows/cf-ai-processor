namespace CFAIProcessor.UI
{
    partial class MainForm
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
            btnTest1 = new Button();
            btnTest2 = new Button();
            SuspendLayout();
            // 
            // btnTest1
            // 
            btnTest1.Location = new Point(114, 50);
            btnTest1.Name = "btnTest1";
            btnTest1.Size = new Size(75, 23);
            btnTest1.TabIndex = 0;
            btnTest1.Text = "Test 1";
            btnTest1.UseVisualStyleBackColor = true;
            btnTest1.Click += btnTest1_Click;
            // 
            // btnTest2
            // 
            btnTest2.Location = new Point(229, 50);
            btnTest2.Name = "btnTest2";
            btnTest2.Size = new Size(75, 23);
            btnTest2.TabIndex = 1;
            btnTest2.Text = "Test 2";
            btnTest2.UseVisualStyleBackColor = true;
            btnTest2.Click += btnTest2_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(881, 494);
            Controls.Add(btnTest2);
            Controls.Add(btnTest1);
            Name = "MainForm";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button btnTest1;
        private Button btnTest2;
    }
}
