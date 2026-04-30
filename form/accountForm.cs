using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms.VisualStyles;
using System.Diagnostics;
using OfficeOpenXml;

namespace AccountAPP
{
    public partial class Account : Form
    {
        DataTable AccountfromDB = new DataTable();
        AccountDB _db = new AccountDB();

        private event EventHandler EventChangeDate;
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string SearchDate
        {
            get { return targetDate; }
            set
            {
                if (value != targetDate)
                {
                    targetDate = value;
                    EventChangeDate?.Invoke(this, new EventArgs());
                }
            }
        }
        private string targetDate;

        public Account()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += Keydown;
            EventChangeDate += OnChangeDate;
            // 直接設 backing field，避免在 DB 建立前就觸發 EventChangeDate
            targetDate = dateTimePicker_searchDate.Value.ToString("yyyy-MM");
            label_todayDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void Keydown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (e.KeyCode == Keys.Enter)
                e.SuppressKeyPress = true;
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter) return false;
            return base.ProcessDialogKey(keyData);
        }

        private void Account_Load(object sender, EventArgs e)
        {
            _db.creatDatabase();
            Program.Log.Info("DB ready");

            // 初始化預設類別
            if (_db.SelectTypeTable().Rows.Count < 1)
            {
                var initiallist = new List<(string name, TypeClass cls)>
                {
                    ("一般支出", TypeClass.expend),
                    ("月繳",     TypeClass.expend),
                    ("年繳",     TypeClass.expend),
                    ("上班收入", TypeClass.income),
                    ("投資收入", TypeClass.income),
                };
                foreach (var (name, cls) in initiallist)
                    _db.InsertDataType(name, cls);
            }

            ResetTypeCombobox();
            RefreshGrid();
            RefreshTotal();

            // 啟動時執行到期的定期項目
            int applied = _db.ApplyDueSchedules();
            if (applied > 0)
            {
                Program.Log.Info($"auto-applied {applied} schedule(s)");
                RefreshGrid();
                RefreshTotal();
                MessageBox.Show($"已自動記錄 {applied} 筆定期項目", "定期項目",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Button_Search_Click(object sender, EventArgs e)
        {
            SearchDate = dateTimePicker_searchDate.Value.ToString("yyyy-MM");
        }

        private void Button_Schedule_Click(object sender, EventArgs e)
        {
            new ScheduleForm(_db).ShowDialog();
            RefreshGrid();
            RefreshTotal();
        }

        private void Button_Input_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textbox_Name.Text) ||
                string.IsNullOrEmpty(comboBox_Type.Text) ||
                string.IsNullOrEmpty(textBox_Pay.Text))
            {
                MessageBox.Show("請輸入完整資訊");
                return;
            }

            if (!int.TryParse(textBox_Pay.Text, out int payValue))
            {
                MessageBox.Show("金額請輸入數字");
                return;
            }

            _db.InsertDataAccount(textbox_Name.Text, comboBox_Type.Text, payValue,
                dateTimePicker_input.Value.ToString("yyyy-MM-dd-ddd"));
            Program.Log.Info($"insert: {textbox_Name.Text} / {comboBox_Type.Text} / {payValue}");

            textbox_Name.Text = string.Empty;
            textBox_Pay.Text = string.Empty;

            RefreshGrid();
            RefreshTotal();
        }

        private void Button_Delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            DataRow row = ((DataRowView)dataGridView1.CurrentRow.DataBoundItem).Row;
            string name  = row["AccountName"].ToString();
            string type  = row["Type"].ToString();
            int    value = Convert.ToInt32(row["AccountValue"]);
            string date  = row["DATE"].ToString();

            if (MessageBox.Show($"確定刪除「{name}」？", "刪除確認",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                _db.DeleteAccount(name, type, value, date);
                Program.Log.Info($"delete: {name} / {type} / {value} / {date}");
                RefreshGrid();
                RefreshTotal();
            }
        }

        private void comboBox_Type_Click(object sender, EventArgs e)
        {
            ResetTypeCombobox();
        }

        private void comboBox_Type_SelectedIndexChanged(object sender, EventArgs e) { }

        private void OnChangeDate(object sender, EventArgs e)
        {
            RefreshGrid();
        }


        #region new type panel

        TypeClass InsertTypeClass;
        private void AddType_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            button_income.Visible = true;
            button_expend.Visible = true;
            button_edit.Visible = true;
            button_cancel.Visible = true;
            button_income.Enabled = true;
            button_expend.Enabled = true;
            textBox_newType.Text = string.Empty;
            InsertTypeClass = TypeClass.Unknow;
        }

        private void button_income_Click(object sender, EventArgs e)
        {
            InsertTypeClass = TypeClass.income;
            button_income.BackColor = Color.LightBlue;
            button_expend.BackColor = Color.White;
            button_income.Enabled = false;
            button_expend.Enabled = true;
        }

        private void button_expend_Click(object sender, EventArgs e)
        {
            InsertTypeClass = TypeClass.expend;
            button_income.BackColor = Color.White;
            button_expend.BackColor = Color.LightBlue;
            button_expend.Enabled = false;
            button_income.Enabled = true;
        }

        private void button_edit_Click(object sender, EventArgs e)
        {
            if (InsertTypeClass != TypeClass.Unknow && !string.IsNullOrEmpty(textBox_newType.Text))
            {
                _db.InsertDataType(textBox_newType.Text.Trim(), InsertTypeClass);
                panel1.Visible = false;
                textBox_newType.Text = string.Empty;
                button_income.BackColor = Color.White;
                button_expend.BackColor = Color.White;
                InsertTypeClass = TypeClass.Unknow;
                ResetTypeCombobox();
            }
            else
            {
                string err = "";
                if (string.IsNullOrEmpty(textBox_newType.Text)) err += "請輸入類別名稱\n";
                if (InsertTypeClass == TypeClass.Unknow)         err += "請選擇收入或支出";
                MessageBox.Show(err);
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }
        #endregion


        #region private helpers

        private void RefreshGrid()
        {
            AccountfromDB = _db.SelectAccountByMonth(SearchDate);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = AccountfromDB;
            SetupGridColumns();
        }

        private void SetupGridColumns()
        {
            if (dataGridView1.Columns.Count == 0) return;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            if (dataGridView1.Columns.Contains("AccountName"))
                dataGridView1.Columns["AccountName"].HeaderText = "帳目";
            if (dataGridView1.Columns.Contains("Type"))
                dataGridView1.Columns["Type"].HeaderText = "類別";
            if (dataGridView1.Columns.Contains("AccountValue"))
                dataGridView1.Columns["AccountValue"].HeaderText = "數額";
            if (dataGridView1.Columns.Contains("DATE"))
            {
                dataGridView1.Columns["DATE"].HeaderText = "日期";
                dataGridView1.Columns["DATE"].Width = 150;
            }
        }

        private void RefreshTotal()
        {
            DataTable dataTable = _db.SelectAccountValue();
            long total = 0;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (Enum.TryParse(dataTable.Rows[i]["TypeClass"].ToString(), out TypeClass tc))
                {
                    if (tc == TypeClass.income)       total += (Int64)dataTable.Rows[i]["AccountValue"];
                    else if (tc == TypeClass.expend)  total -= (Int64)dataTable.Rows[i]["AccountValue"];
                }
            }
            label_totalValue.Text = total.ToString("N0");
        }

        private void ResetTypeCombobox()
        {
            comboBox_Type.Items.Clear();
            DataTable types = _db.SelectTypeTable();
            for (int i = 0; i < types.Rows.Count; i++)
                comboBox_Type.Items.Add(types.Rows[i]["Type"].ToString());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label_todayDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        #endregion


        #region Excel import

        private void Button_Import_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title  = "選擇 Excel 檔案";
                ofd.Filter = "Excel 檔案 (*.xlsx)|*.xlsx";
                if (ofd.ShowDialog() != DialogResult.OK) return;

                int ok = 0, skip = 0;
                try
                {
                    using (var pkg = new ExcelPackage(new FileInfo(ofd.FileName)))
                    {
                        var ws = pkg.Workbook.Worksheets[1];
                        if (ws == null)
                        {
                            MessageBox.Show("找不到工作表");
                            return;
                        }

                        int rows = ws.Dimension?.Rows ?? 0;
                        for (int r = 2; r <= rows; r++)
                        {
                            string name  = ws.Cells[r, 1].Text?.Trim();
                            string type  = ws.Cells[r, 2].Text?.Trim();
                            string amtTx = ws.Cells[r, 3].Text?.Trim();
                            string date  = ws.Cells[r, 4].Text?.Trim();

                            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type) ||
                                !int.TryParse(amtTx, out int amt) || string.IsNullOrEmpty(date))
                            {
                                skip++;
                                continue;
                            }

                            // accept both yyyy-MM-dd and yyyy-MM-dd-ddd
                            if (DateTime.TryParse(date.Length > 10 ? date.Substring(0, 10) : date,
                                    out DateTime dt))
                            {
                                date = dt.ToString("yyyy-MM-dd-ddd");
                            }

                            _db.InsertDataAccount(name, type, amt, date);
                            ok++;
                        }
                    }

                    Program.Log.Info($"excel import: ok={ok} skip={skip} file={ofd.SafeFileName}");
                    MessageBox.Show($"匯入完成：{ok} 筆成功，{skip} 筆略過",
                        "匯入結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshGrid();
                    RefreshTotal();
                }
                catch (Exception ex)
                {
                    Program.Log.Error("excel import failed", ex);
                    MessageBox.Show($"匯入失敗：{ex.Message}", "錯誤",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

    }
}
