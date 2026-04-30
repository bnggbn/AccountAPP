using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace AccountAPP
{
    internal partial class ScheduleForm : Form
    {
        private readonly AccountDB _db;

        public ScheduleForm(AccountDB db)
        {
            InitializeComponent();
            _db = db;
        }

        private void ScheduleForm_Load(object sender, EventArgs e)
        {
            // combobox 頻率選項
            comboBox_freq.Items.AddRange(new[] { "每天", "每月", "每年" });
            comboBox_freq.SelectedIndex = 1;

            ResetTypeCombobox();
            RefreshGrid();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_name.Text) ||
                string.IsNullOrEmpty(comboBox_type.Text) ||
                string.IsNullOrEmpty(textBox_amount.Text))
            {
                MessageBox.Show("請填寫完整資訊");
                return;
            }

            if (!int.TryParse(textBox_amount.Text, out int amount))
            {
                MessageBox.Show("金額請輸入數字");
                return;
            }

            string freq = FreqToKey(comboBox_freq.Text);
            _db.InsertSchedule(textBox_name.Text.Trim(), comboBox_type.Text, amount, freq);

            textBox_name.Text   = string.Empty;
            textBox_amount.Text = string.Empty;
            RefreshGrid();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (dataGridView_schedule.CurrentRow == null) return;

            var item = (ScheduleItem)dataGridView_schedule.CurrentRow.DataBoundItem;
            if (MessageBox.Show($"確定刪除「{item.Name}」？", "刪除確認",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                _db.DeleteSchedule(item.Id);
                RefreshGrid();
            }
        }

        private void dataGridView_schedule_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // 啟用欄 checkbox 變更
            if (e.RowIndex < 0 || dataGridView_schedule.Columns[e.ColumnIndex].Name != "col_enabled") return;

            var item = (ScheduleItem)dataGridView_schedule.Rows[e.RowIndex].DataBoundItem;
            _db.SetScheduleEnabled(item.Id, item.Enabled);
        }

        private void dataGridView_schedule_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView_schedule.IsCurrentCellDirty)
                dataGridView_schedule.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        #region helpers

        private void RefreshGrid()
        {
            var items = new List<ScheduleItem>();
            foreach (DataRow row in _db.SelectSchedules().Rows)
                items.Add(ScheduleItem.FromRow(row));

            dataGridView_schedule.DataSource = null;
            dataGridView_schedule.DataSource = items;

            // 欄位顯示設定（只在欄位剛建立時跑一次）
            if (dataGridView_schedule.Columns.Count == 0) return;

            dataGridView_schedule.ReadOnly = false;
            dataGridView_schedule.AllowUserToAddRows    = false;
            dataGridView_schedule.AllowUserToDeleteRows = false;

            foreach (DataGridViewColumn col in dataGridView_schedule.Columns)
                col.ReadOnly = true;

            // 隱藏不需要顯示的欄
            foreach (string hide in new[] { "Id", "Date", "TypeClass", "LastApplied", "Frequency" })
                if (dataGridView_schedule.Columns.Contains(hide))
                    dataGridView_schedule.Columns[hide].Visible = false;

            SetHeader("Name",          "名稱");
            SetHeader("Type",          "類別");
            SetHeader("Amount",        "金額");
            SetHeader("FrequencyLabel","頻率");
            SetHeader("Enabled",       "啟用");

            // 只有 Enabled checkbox 可以點
            if (dataGridView_schedule.Columns.Contains("Enabled"))
            {
                dataGridView_schedule.Columns["Enabled"].ReadOnly = false;
                dataGridView_schedule.Columns["Enabled"].Name     = "col_enabled";
            }
        }

        private void SetHeader(string colName, string header)
        {
            if (dataGridView_schedule.Columns.Contains(colName))
                dataGridView_schedule.Columns[colName].HeaderText = header;
        }

        private void ResetTypeCombobox()
        {
            comboBox_type.Items.Clear();
            foreach (DataRow row in _db.SelectTypeTable().Rows)
                comboBox_type.Items.Add(row["Type"].ToString());
            if (comboBox_type.Items.Count > 0)
                comboBox_type.SelectedIndex = 0;
        }

        private static string FreqToKey(string label)
        {
            switch (label)
            {
                case "每天": return "daily";
                case "每月": return "monthly";
                case "每年": return "yearly";
                default:     return "monthly";
            }
        }

        #endregion
    }
}
