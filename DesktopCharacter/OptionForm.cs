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
        private Form1 form;

        public OptionForm()
        {
            InitializeComponent();
        }

        public void parentForm(Form1 parentForm)
        {
            this.form = parentForm;
            this.form.ignoreKeyAndMouse = true;
            string speed = this.form.playSpeed.ToString();
            textBox_motionspeed.Text = speed;
        }

        private void textBox_motionspeed_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float speed = float.Parse(textBox_motionspeed.Text);
                this.form.playSpeed = speed;
            } catch (Exception ex)
            {

            }
        }

        private void OptionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.form.ignoreKeyAndMouse = false;
        }
    }
}
