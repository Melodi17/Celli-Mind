namespace Celli_Mind
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
            this.Emotion_Label = new System.Windows.Forms.Label();
            this.Action_Button = new System.Windows.Forms.Button();
            this.Output_Box = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // Emotion_Label
            // 
            this.Emotion_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Emotion_Label.Font = new System.Drawing.Font("Consolas", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Emotion_Label.Location = new System.Drawing.Point(0, 0);
            this.Emotion_Label.Name = "Emotion_Label";
            this.Emotion_Label.Size = new System.Drawing.Size(369, 37);
            this.Emotion_Label.TabIndex = 0;
            this.Emotion_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Emotion_Label.Visible = false;
            // 
            // Action_Button
            // 
            this.Action_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Action_Button.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Action_Button.FlatAppearance.BorderSize = 0;
            this.Action_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.Action_Button.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.WindowFrame;
            this.Action_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Action_Button.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Action_Button.ForeColor = System.Drawing.Color.White;
            this.Action_Button.Location = new System.Drawing.Point(264, 109);
            this.Action_Button.Name = "Action_Button";
            this.Action_Button.Size = new System.Drawing.Size(94, 29);
            this.Action_Button.TabIndex = 1;
            this.Action_Button.Text = "Quit";
            this.Action_Button.UseVisualStyleBackColor = true;
            this.Action_Button.Visible = false;
            // 
            // Output_Box
            // 
            this.Output_Box.BackColor = System.Drawing.Color.Black;
            this.Output_Box.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Output_Box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Output_Box.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Output_Box.ForeColor = System.Drawing.Color.White;
            this.Output_Box.Location = new System.Drawing.Point(0, 37);
            this.Output_Box.Name = "Output_Box";
            this.Output_Box.ReadOnly = true;
            this.Output_Box.Size = new System.Drawing.Size(370, 526);
            this.Output_Box.TabIndex = 2;
            this.Output_Box.Text = "";
            this.Output_Box.Click += new System.EventHandler(this.Output_Box_Click);
            this.Output_Box.Validated += new System.EventHandler(this.Output_Box_Validated);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(370, 150);
            this.Controls.Add(this.Action_Button);
            this.Controls.Add(this.Output_Box);
            this.Controls.Add(this.Emotion_Label);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "Form1";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Click += new System.EventHandler(this.Form1_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private Label Emotion_Label;
        private Button Action_Button;
        private RichTextBox Output_Box;
    }
}