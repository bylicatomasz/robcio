namespace RobcioDSS
{
    partial class BasicFormTest
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
            this.ClawOpenButton = new System.Windows.Forms.Button();
            this.ClawCloseButton = new System.Windows.Forms.Button();
            this.ClawBrawoButton = new System.Windows.Forms.Button();
            this.ClearTaskButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ClawOpenButton
            // 
            this.ClawOpenButton.Location = new System.Drawing.Point(72, 55);
            this.ClawOpenButton.Name = "ClawOpenButton";
            this.ClawOpenButton.Size = new System.Drawing.Size(75, 23);
            this.ClawOpenButton.TabIndex = 0;
            this.ClawOpenButton.Text = "ClawOpen";
            this.ClawOpenButton.UseVisualStyleBackColor = true;
            this.ClawOpenButton.Click += new System.EventHandler(this.ClawOpenButton_Click);
            // 
            // ClawCloseButton
            // 
            this.ClawCloseButton.Location = new System.Drawing.Point(170, 55);
            this.ClawCloseButton.Name = "ClawCloseButton";
            this.ClawCloseButton.Size = new System.Drawing.Size(75, 23);
            this.ClawCloseButton.TabIndex = 1;
            this.ClawCloseButton.Text = "ClawClose";
            this.ClawCloseButton.UseVisualStyleBackColor = true;
            this.ClawCloseButton.Click += new System.EventHandler(this.ClawCloseButton_Click);
            // 
            // ClawBrawoButton
            // 
            this.ClawBrawoButton.Location = new System.Drawing.Point(72, 138);
            this.ClawBrawoButton.Name = "ClawBrawoButton";
            this.ClawBrawoButton.Size = new System.Drawing.Size(75, 23);
            this.ClawBrawoButton.TabIndex = 2;
            this.ClawBrawoButton.Text = "ClawBrawo!";
            this.ClawBrawoButton.UseVisualStyleBackColor = true;
            this.ClawBrawoButton.Click += new System.EventHandler(this.ClawBrawoButton_Click);
            // 
            // ClearTaskButton
            // 
            this.ClearTaskButton.Location = new System.Drawing.Point(185, 137);
            this.ClearTaskButton.Name = "ClearTaskButton";
            this.ClearTaskButton.Size = new System.Drawing.Size(75, 23);
            this.ClearTaskButton.TabIndex = 3;
            this.ClearTaskButton.Text = "ClearTask";
            this.ClearTaskButton.UseVisualStyleBackColor = true;
            this.ClearTaskButton.Click += new System.EventHandler(this.ClearTaskButton_Click);
            // 
            // BasicFormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.ClearTaskButton);
            this.Controls.Add(this.ClawBrawoButton);
            this.Controls.Add(this.ClawCloseButton);
            this.Controls.Add(this.ClawOpenButton);
            this.Name = "BasicFormTest";
            this.Text = "BasicFormTest";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ClawOpenButton;
        private System.Windows.Forms.Button ClawCloseButton;
        private System.Windows.Forms.Button ClawBrawoButton;
        private System.Windows.Forms.Button ClearTaskButton;
    }
}