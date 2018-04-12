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
            dataGridView_model.ColumnCount = 3;
            DataGridViewCheckBoxColumn enableColumn = new DataGridViewCheckBoxColumn();
            {
                enableColumn.HeaderText = "Enable";
                //column.Name = ColumnName.OutOfOffice.ToString();
                enableColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                enableColumn.FlatStyle = FlatStyle.Standard;
                //column.ThreeState = true;
                enableColumn.CellTemplate = new DataGridViewCheckBoxCell();
                enableColumn.CellTemplate.Style.BackColor = Color.Beige;
            }
            DataGridViewButtonColumn downloadColumn = new DataGridViewButtonColumn();
            {
                downloadColumn.HeaderText = "download";
                downloadColumn.UseColumnTextForButtonValue = true;
                downloadColumn.Name = "downloadButton";
                downloadColumn.DefaultCellStyle.NullValue = "Download";
                downloadColumn.Text = "Download";
            }
            DataGridViewButtonColumn referenceColumn = new DataGridViewButtonColumn();
            {
                referenceColumn.HeaderText = "reference";
                referenceColumn.UseColumnTextForButtonValue = true;
                referenceColumn.Name = "referenceButton";
                referenceColumn.DefaultCellStyle.NullValue = "Reference";
                referenceColumn.Text = "Reference";
            }
            DataGridViewComboBoxColumn motionColumn = new DataGridViewComboBoxColumn();
            {
                motionColumn.HeaderText = "motion";
            }
            dataGridView_model.Columns.Insert(1, enableColumn);
            dataGridView_model.Columns.Insert(3, downloadColumn);
            dataGridView_model.Columns.Insert(5, referenceColumn);
            dataGridView_model.Columns.Insert(6, motionColumn);
            dataGridView_model.Columns[0].HeaderText = "Id";
            dataGridView_model.Columns[0].Visible = false;
            //dataGridView_model.Columns[1].HeaderText = "Enable";
            dataGridView_model.Columns[2].HeaderText = "Name";
            //dataGridView_model.Columns[3].HeaderText = "Download";
            dataGridView_model.Columns[4].HeaderText = "ModelFile";
            //dataGridView_model.Columns[5].HeaderText = "Reference";
            //dataGridView_model.Columns[6].HeaderText = "MotionName";

            motionColumn.Items.Add("モーション1");
            motionColumn.Items.Add("モーション2");

            dataGridView_motion.RowHeadersVisible = false;
            dataGridView_motion.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_motion.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_motion.ColumnCount = 3;
            DataGridViewButtonColumn downloadColumn2 = new DataGridViewButtonColumn();
            {
                downloadColumn2.HeaderText = "download";
                downloadColumn2.UseColumnTextForButtonValue = true;
                downloadColumn2.Name = "downloadButton";
                downloadColumn2.DefaultCellStyle.NullValue = "Download";
                downloadColumn2.Text = "Download";
            }
            DataGridViewButtonColumn referenceColumn2 = new DataGridViewButtonColumn();
            {
                referenceColumn2.HeaderText = "reference";
                referenceColumn2.UseColumnTextForButtonValue = true;
                referenceColumn2.Name = "referenceButton";
                referenceColumn2.DefaultCellStyle.NullValue = "Reference";
                referenceColumn2.Text = "Reference";
            }
            dataGridView_motion.Columns.Insert(2, downloadColumn2);
            dataGridView_motion.Columns.Insert(4, referenceColumn2);
            dataGridView_motion.Columns[0].HeaderText = "Id";
            dataGridView_motion.Columns[0].Visible = false;
            dataGridView_motion.Columns[1].HeaderText = "Motion";
            //dataGridView_motion.Columns[2].HeaderText = "Download";
            dataGridView_motion.Columns[3].HeaderText = "MotionFile";
            //dataGridView_motion.Columns[4].HeaderText = "Reference";


            ContextMenuStrip menu = new ContextMenuStrip();

            ToolStripMenuItem menuItem0 = new ToolStripMenuItem();
            menuItem0.Text = "Add";// Properties.Resources.Option;
            //menuItem0.Click += new EventHandler(Option_Click);
            menu.Items.Add(menuItem0);

            ToolStripMenuItem menuItem1 = new ToolStripMenuItem();
            menuItem1.Text = "Delete";// Properties.Resources.Option;
            //menuItem1.Click += new EventHandler(Option_Click);
            menu.Items.Add(menuItem1);

            dataGridView_model.ContextMenuStrip = menu;
            dataGridView_motion.ContextMenuStrip = menu;
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

            getData();
        }

        private void getData()
        {
            // model dara
#if false
            List<Data.Model> models = this.form.modelData.getAllModels();
#else
            // testdata
            List<Data.Model> models = new List<Data.Model>();
            Data.Model m = new Data.Model();
            m.Id = 1;
            m.Name = "model name1";
            m.FileName = @"D:\Projects\data\hoge.pmd";
            m.MotionId = 1;
            models.Add(m);
            Data.Model m2 = new Data.Model();
            m2.Id = 2;
            m2.Name = "model name2";
            m2.FileName = @"D:\Projects\data\hogehoge.pmd";
            m2.MotionId = 0;
            models.Add(m2);
#endif

            // datagridview
            foreach (Data.Model model in models)
            {
                DataGridViewRow item = new DataGridViewRow();
                item.CreateCells(dataGridView_model);
                item.Cells[0].Value = model.Id;
                item.Cells[1].Value = true; //
                item.Cells[2].Value = model.Name;
                item.Cells[4].Value = model.FileName;
                item.Cells[6].Value = "モーション2";// model.MotionId -> motion name;
                dataGridView_model.Rows.Add(item);
            }

            // motion dara
#if false
            List<Data.Motion> motions = this.form.motionData.getAllMotions();
#else
            // testdata
            List<Data.Motion> motions = new List<Data.Motion>();
            Data.Motion m3 = new Data.Motion();
            m3.Id = 1;
            m3.Name = "motion name1";
            m3.FileName = @"D:\Projects\data\hoge.pmd";
            m.MotionId = 1;
            motions.Add(m3);
#endif

            // datagridview
            foreach (Data.Motion motion in motions)
            {
                DataGridViewRow item = new DataGridViewRow();
                item.CreateCells(dataGridView_motion);
                item.Cells[0].Value = motion.Id;
                item.Cells[1].Value = motion.Name; //
                item.Cells[3].Value = motion.FileName;
                dataGridView_motion.Rows.Add(item);
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

            // save to database
            this.form.optionData.setLanguage(org_lang);

            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(lang);
            this.form.currentLanguage = org_lang;

            this.Close();
            OptionForm optionForm = new OptionForm();
            optionForm.parentForm(this.form);
            optionForm.Show();

            this.form.icon.Dispose();
            this.form.TaskTray();
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
