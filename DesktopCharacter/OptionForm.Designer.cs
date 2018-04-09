namespace DesktopCharacter
{
    partial class OptionForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.comboBox_screen = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown_scale = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_motionspeed = new System.Windows.Forms.NumericUpDown();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_motionspeed)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(401, 399);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.numericUpDown_motionspeed);
            this.tabPage1.Controls.Add(this.numericUpDown_scale);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.comboBox_screen);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(393, 373);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "AppSetting";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // comboBox_screen
            // 
            this.comboBox_screen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_screen.FormattingEnabled = true;
            this.comboBox_screen.Location = new System.Drawing.Point(84, 27);
            this.comboBox_screen.Name = "comboBox_screen";
            this.comboBox_screen.Size = new System.Drawing.Size(121, 20);
            this.comboBox_screen.TabIndex = 1;
            this.comboBox_screen.SelectedIndexChanged += new System.EventHandler(this.comboBox_screen_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Screen";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "MotionSpeed";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(393, 373);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Model&Motions";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Scale";
            // 
            // numericUpDown_scale
            // 
            this.numericUpDown_scale.Location = new System.Drawing.Point(84, 121);
            this.numericUpDown_scale.Name = "numericUpDown_scale";
            this.numericUpDown_scale.Size = new System.Drawing.Size(120, 19);
            this.numericUpDown_scale.TabIndex = 5;
            this.numericUpDown_scale.ValueChanged += new System.EventHandler(this.numericUpDown_scale_ValueChanged);
            // 
            // numericUpDown_motionspeed
            // 
            this.numericUpDown_motionspeed.Location = new System.Drawing.Point(84, 77);
            this.numericUpDown_motionspeed.Name = "numericUpDown_motionspeed";
            this.numericUpDown_motionspeed.Size = new System.Drawing.Size(120, 19);
            this.numericUpDown_motionspeed.TabIndex = 3;
            this.numericUpDown_motionspeed.ValueChanged += new System.EventHandler(this.numericUpDown_motionspeed_ValueChanged);
            // 
            // OptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 399);
            this.Controls.Add(this.tabControl1);
            this.Name = "OptionForm";
            this.Text = "OptionForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OptionForm_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_motionspeed)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_screen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown_scale;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDown_motionspeed;
    }
}