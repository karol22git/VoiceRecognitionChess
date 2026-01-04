namespace Chess
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
            components = new System.ComponentModel.Container();
            panel2 = new Panel();
            panel3 = new Panel();
            timer1 = new System.Windows.Forms.Timer(components);
            panel4 = new Panel();
            panel5 = new Panel();
            panel6 = new Panel();
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.BackColor = Color.Green;
            panel2.Location = new Point(800, 227);
            panel2.Name = "panel2";
            panel2.Size = new Size(191, 229);
            panel2.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(255, 128, 0);
            panel3.Location = new Point(767, 19);
            panel3.Name = "panel3";
            panel3.Size = new Size(241, 136);
            panel3.TabIndex = 2;
            panel3.Paint += panel3_Paint;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(192, 0, 0);
            panel4.Location = new Point(767, 545);
            panel4.Name = "panel4";
            panel4.Size = new Size(241, 135);
            panel4.TabIndex = 3;
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(192, 192, 255);
            panel5.Location = new Point(800, 161);
            panel5.Name = "panel5";
            panel5.Size = new Size(191, 60);
            panel5.TabIndex = 4;
            // 
            // panel6
            // 
            panel6.BackColor = Color.Purple;
            panel6.Location = new Point(800, 462);
            panel6.Name = "panel6";
            panel6.Size = new Size(191, 64);
            panel6.TabIndex = 5;
            // 
            // button1
            // 
            button1.Location = new Point(767, 708);
            button1.Name = "button1";
            button1.Size = new Size(119, 41);
            button1.TabIndex = 6;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(892, 708);
            button2.Name = "button2";
            button2.Size = new Size(114, 41);
            button2.TabIndex = 7;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1060, 736);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private System.Windows.Forms.Timer timer1;
        private Panel panel4;
        private Panel panel5;
        private Panel panel6;
        private Button button1;
        private Button button2;
    }
}
