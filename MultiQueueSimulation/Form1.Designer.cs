namespace MultiQueueSimulation
{
    partial class Form1
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
            this.path_box = new System.Windows.Forms.TextBox();
            this.path_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // path_box
            // 
            this.path_box.Location = new System.Drawing.Point(126, 172);
            this.path_box.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.path_box.Name = "path_box";
            this.path_box.Size = new System.Drawing.Size(360, 20);
            this.path_box.TabIndex = 0;
            this.path_box.TextChanged += new System.EventHandler(this.path_box_TextChanged);
            // 
            // path_button
            // 
            this.path_button.Location = new System.Drawing.Point(254, 214);
            this.path_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.path_button.Name = "path_button";
            this.path_button.Size = new System.Drawing.Size(102, 43);
            this.path_button.TabIndex = 1;
            this.path_button.Text = "Open The File";
            this.path_button.UseVisualStyleBackColor = true;
            this.path_button.Click += new System.EventHandler(this.path_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 372);
            this.Controls.Add(this.path_button);
            this.Controls.Add(this.path_box);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox path_box;
        private System.Windows.Forms.Button path_button;
    }
}

