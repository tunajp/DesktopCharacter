using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopCharacter
{
    public partial class OptionForm : Form
    {
        private MainForm form;

        public OptionForm()
        {
            InitializeComponent();

            this.comboBox_screen.DisplayMember = "DeviceName";
            this.comboBox_screen.DataSource = Screen.AllScreens;
            this.numericUpDown_motionspeed.DecimalPlaces = 3;
            this.numericUpDown_scale.DecimalPlaces = 3;
        }

        public void parentForm(MainForm parentForm)
        {
            this.form = parentForm;
            this.form.ignoreKeyAndMouse = true;
            numericUpDown_motionspeed.Value = (decimal)this.form.playSpeed;
            numericUpDown_scale.Value = (decimal)this.form.scale;
        }

        private void comboBox_screen_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.form.screen = (Screen)this.comboBox_screen.SelectedItem;
            // 画面サイズをフォームのサイズに適用する
            this.form.ClientSize = new Size(this.form.screen.Bounds.Width, this.form.screen.Bounds.Height);
            this.form.Location = this.form.screen.Bounds.Location;
        }

        private void numericUpDown_scale_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                float scale = (float)numericUpDown_scale.Value;
                this.form.scale = scale;
                this.form.setScale();
            }
            catch (Exception ex)
            {

            }
        }

        private void numericUpDown_motionspeed_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                float speed = (float)numericUpDown_motionspeed.Value;
                this.form.playSpeed = speed;
            }
            catch (Exception ex)
            {

            }
        }

        private void OptionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.form.ignoreKeyAndMouse = false;
        }
    }
}
