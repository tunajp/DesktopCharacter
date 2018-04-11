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

            this.initDataGridView();
        }

        private void initDataGridView()
        {
            dataGridView_model.RowHeadersVisible = false;
            dataGridView_model.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_model.ColumnCount = 6;
            dataGridView_model.Columns[0].HeaderText = "Enable";
            dataGridView_model.Columns[1].HeaderText = "Name";
            dataGridView_model.Columns[2].HeaderText = "Download";
            dataGridView_model.Columns[3].HeaderText = "ModelFile";
            dataGridView_model.Columns[4].HeaderText = "Reference";
            dataGridView_model.Columns[5].HeaderText = "MotionName";

            dataGridView_motion.RowHeadersVisible = false;
            dataGridView_motion.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_motion.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_motion.ColumnCount = 4;
            dataGridView_motion.Columns[0].HeaderText = "Motion";
            dataGridView_motion.Columns[1].HeaderText = "Download";
            dataGridView_motion.Columns[2].HeaderText = "MotionFile";
            dataGridView_motion.Columns[3].HeaderText = "Reference";
        }

        public void parentForm(MainForm parentForm)
        {
            this.form = parentForm;
            this.form.ignoreKeyAndMouse = true;
            numericUpDown_motionspeed.Value = (decimal)this.form.playSpeed;
            numericUpDown_scale.Value = (decimal)this.form.scale;

            string[] languages = { "Default", "ja-JP" };
            int i = 0;
            foreach (string lang in languages)
            {
                this.comboBox_language.Items.Add(lang);
                if (this.form.currentLanguage == lang)
                {
                    this.comboBox_language.SelectedIndex = i;
                }
                i++;
            }
        }

        private void comboBox_language_SelectedIndexChanged(object sender, EventArgs e)
        {
            string lang = this.comboBox_language.Text;
            if (this.form.currentLanguage == lang)
            {
                return;
            }
            string org_lang = lang;
            if (lang == "Default")
            {
                lang = "en-US";
            }
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(lang);
            this.form.currentLanguage = org_lang;

            this.Close();
            OptionForm optionForm = new OptionForm();
            optionForm.parentForm(this.form);
            optionForm.Show();

            this.form.icon.Dispose();
            this.form.TaskTray();

            // TODO: save to registry
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
