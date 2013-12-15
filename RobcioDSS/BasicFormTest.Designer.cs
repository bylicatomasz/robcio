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
            this.buttonForward = new System.Windows.Forms.Button();
            this.buttonLeft = new System.Windows.Forms.Button();
            this.buttonRight = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.test = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ClawOpenButton
            // 
            this.ClawOpenButton.Location = new System.Drawing.Point(12, 12);
            this.ClawOpenButton.Name = "ClawOpenButton";
            this.ClawOpenButton.Size = new System.Drawing.Size(75, 23);
            this.ClawOpenButton.TabIndex = 0;
            this.ClawOpenButton.Text = "ClawOpen";
            this.ClawOpenButton.UseVisualStyleBackColor = true;
            this.ClawOpenButton.Click += new System.EventHandler(this.ClawOpenButton_Click);
            // 
            // ClawCloseButton
            // 
            this.ClawCloseButton.Location = new System.Drawing.Point(93, 12);
            this.ClawCloseButton.Name = "ClawCloseButton";
            this.ClawCloseButton.Size = new System.Drawing.Size(75, 23);
            this.ClawCloseButton.TabIndex = 1;
            this.ClawCloseButton.Text = "ClawClose";
            this.ClawCloseButton.UseVisualStyleBackColor = true;
            this.ClawCloseButton.Click += new System.EventHandler(this.ClawCloseButton_Click);
            // 
            // ClawBrawoButton
            // 
            this.ClawBrawoButton.Location = new System.Drawing.Point(12, 41);
            this.ClawBrawoButton.Name = "ClawBrawoButton";
            this.ClawBrawoButton.Size = new System.Drawing.Size(75, 23);
            this.ClawBrawoButton.TabIndex = 2;
            this.ClawBrawoButton.Text = "ClawBrawo!";
            this.ClawBrawoButton.UseVisualStyleBackColor = true;
            this.ClawBrawoButton.Click += new System.EventHandler(this.ClawBrawoButton_Click);
            // 
            // ClearTaskButton
            // 
            this.ClearTaskButton.Location = new System.Drawing.Point(93, 41);
            this.ClearTaskButton.Name = "ClearTaskButton";
            this.ClearTaskButton.Size = new System.Drawing.Size(75, 23);
            this.ClearTaskButton.TabIndex = 3;
            this.ClearTaskButton.Text = "ClearTask";
            this.ClearTaskButton.UseVisualStyleBackColor = true;
            this.ClearTaskButton.Click += new System.EventHandler(this.ClearTaskButton_Click);
            // 
            // buttonForward
            // 
            this.buttonForward.Location = new System.Drawing.Point(108, 117);
            this.buttonForward.Name = "buttonForward";
            this.buttonForward.Size = new System.Drawing.Size(75, 23);
            this.buttonForward.TabIndex = 4;
            this.buttonForward.Text = "Forward";
            this.buttonForward.UseVisualStyleBackColor = true;
            this.buttonForward.Click += new System.EventHandler(this.buttonForward_Click);
            // 
            // buttonLeft
            // 
            this.buttonLeft.Location = new System.Drawing.Point(27, 147);
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(75, 23);
            this.buttonLeft.TabIndex = 5;
            this.buttonLeft.Text = "Left";
            this.buttonLeft.UseVisualStyleBackColor = true;
            this.buttonLeft.Click += new System.EventHandler(this.buttonLeft_Click);
            // 
            // buttonRight
            // 
            this.buttonRight.Location = new System.Drawing.Point(189, 147);
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(75, 23);
            this.buttonRight.TabIndex = 6;
            this.buttonRight.Text = "Right";
            this.buttonRight.UseVisualStyleBackColor = true;
            this.buttonRight.Click += new System.EventHandler(this.buttonRight_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(108, 175);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 7;
            this.buttonBack.Text = "Back";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(108, 146);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 8;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // test
            // 
            this.test.Location = new System.Drawing.Point(189, 41);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(75, 23);
            this.test.TabIndex = 9;
            this.test.Text = "test";
            this.test.UseVisualStyleBackColor = true;
            this.test.Click += new System.EventHandler(this.test_Click);
            // 
            // BasicFormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.test);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonRight);
            this.Controls.Add(this.buttonLeft);
            this.Controls.Add(this.buttonForward);
            this.Controls.Add(this.ClearTaskButton);
            this.Controls.Add(this.ClawBrawoButton);
            this.Controls.Add(this.ClawCloseButton);
            this.Controls.Add(this.ClawOpenButton);
            this.Location = new System.Drawing.Point(0, 100);
            this.Name = "BasicFormTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "BasicFormTest";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ClawOpenButton;
        private System.Windows.Forms.Button ClawCloseButton;
        private System.Windows.Forms.Button ClawBrawoButton;
        private System.Windows.Forms.Button ClearTaskButton;
        private System.Windows.Forms.Button buttonForward;
        private System.Windows.Forms.Button buttonLeft;
        private System.Windows.Forms.Button buttonRight;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button test;
    }
}