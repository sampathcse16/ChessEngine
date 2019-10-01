namespace ChessEngine
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
            this.validMovesHighlighter = new System.Windows.Forms.Button();
            this.snapShot = new System.Windows.Forms.Button();
            this.snapShotTextBox = new System.Windows.Forms.RichTextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.whiteRadioButton = new System.Windows.Forms.RadioButton();
            this.blackRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // validMovesHighlighter
            // 
            this.validMovesHighlighter.Location = new System.Drawing.Point(619, 174);
            this.validMovesHighlighter.Name = "validMovesHighlighter";
            this.validMovesHighlighter.Size = new System.Drawing.Size(128, 23);
            this.validMovesHighlighter.TabIndex = 0;
            this.validMovesHighlighter.Text = "Highlight Valid Moves";
            this.validMovesHighlighter.UseVisualStyleBackColor = true;
            this.validMovesHighlighter.Click += new System.EventHandler(this.validMovesHighlighter_Click);
            // 
            // snapShot
            // 
            this.snapShot.Location = new System.Drawing.Point(619, 203);
            this.snapShot.Name = "snapShot";
            this.snapShot.Size = new System.Drawing.Size(128, 23);
            this.snapShot.TabIndex = 1;
            this.snapShot.Text = "Snap Shot";
            this.snapShot.UseVisualStyleBackColor = true;
            this.snapShot.Click += new System.EventHandler(this.snapShot_Click);
            // 
            // snapShotTextBox
            // 
            this.snapShotTextBox.Location = new System.Drawing.Point(571, 252);
            this.snapShotTextBox.Name = "snapShotTextBox";
            this.snapShotTextBox.Size = new System.Drawing.Size(217, 186);
            this.snapShotTextBox.TabIndex = 3;
            this.snapShotTextBox.Text = "";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 22);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(116, 17);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "White As Computer";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 45);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(115, 17);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.Text = "Black As Computer";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(6, 68);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(135, 17);
            this.radioButton3.TabIndex = 3;
            this.radioButton3.Text = "Computer VS Computer";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(637, 145);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(568, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Current Move:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Location = new System.Drawing.Point(571, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 90);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // whiteRadioButton
            // 
            this.whiteRadioButton.AutoSize = true;
            this.whiteRadioButton.Checked = true;
            this.whiteRadioButton.Location = new System.Drawing.Point(649, 118);
            this.whiteRadioButton.Name = "whiteRadioButton";
            this.whiteRadioButton.Size = new System.Drawing.Size(53, 17);
            this.whiteRadioButton.TabIndex = 7;
            this.whiteRadioButton.TabStop = true;
            this.whiteRadioButton.Text = "White";
            this.whiteRadioButton.UseVisualStyleBackColor = true;
            // 
            // blackRadioButton
            // 
            this.blackRadioButton.AutoSize = true;
            this.blackRadioButton.Location = new System.Drawing.Point(708, 116);
            this.blackRadioButton.Name = "blackRadioButton";
            this.blackRadioButton.Size = new System.Drawing.Size(52, 17);
            this.blackRadioButton.TabIndex = 8;
            this.blackRadioButton.Text = "Black";
            this.blackRadioButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.blackRadioButton);
            this.Controls.Add(this.whiteRadioButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.snapShotTextBox);
            this.Controls.Add(this.snapShot);
            this.Controls.Add(this.validMovesHighlighter);
            this.Name = "Form1";
            this.Text = "Chess";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button validMovesHighlighter;
        private System.Windows.Forms.Button snapShot;
        private System.Windows.Forms.RichTextBox snapShotTextBox;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton whiteRadioButton;
        private System.Windows.Forms.RadioButton blackRadioButton;
    }
}

