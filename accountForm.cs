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

namespace AccountAPP
{
    public partial class Account : Form
    {
        public string DBPath = Path.Combine("DB", "Account.db");
        DataTable AccountfromDB = new DataTable(); //帳目db
        DataTable DespositfromDB = new DataTable();
        AccountDB _db = new AccountDB();

        public Account()
        {
            InitializeComponent();
            
            this.KeyPreview = true;
            this.KeyDown += Keydown;
        }

        private void Keydown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            
            if(e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if(keyData == Keys.Enter)
            {
                return false;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void Account_Load(object sender, EventArgs e)
        {
           
            if (!File.Exists(DBPath))
            {
                _db.creatDatabase();
            }
            else
            {
                _db.creatDatabase();
                Makedatagridview_month();
            }
            dataGridView1.DataSource = AccountfromDB;
            
            if(AccountfromDB.Columns.Count != 0)
            {
                dataGridView1.Columns[3].Width = 150;
                dataGridView1.AllowUserToDeleteRows = false; // 設為 true 允許刪除行
                dataGridView1.ReadOnly = true; // 設為 false 允許編輯資料
                dataGridView1.Columns["AccountName"].HeaderText = "帳目";
                dataGridView1.Columns["Type"].HeaderText = "類別";
                dataGridView1.Columns["AccountValue"].HeaderText = "數額";
                dataGridView1.Columns["DATE"].HeaderText = "日期";
            }

            label_todayDate.Text = dateTimePicker_searchDate.Value.ToString("yyyy-MM-dd");
            
        }
        private void Button_Search_Click(object sender, EventArgs e)
        {
            
        }

        private void Button_Input_Click(object sender, EventArgs e)
        {
            
            if(textbox_Name.Text == null || comboBox_Type.Text == null || textBox_Pay.Text == null)
            {
                MessageBox.Show("請輸入完整資訊");
            }
            else
            {
                if (comboBox_Type.Text == "支出")
                {
                    _db.InsertDataAccount(textbox_Name.Text, comboBox_Type.Text, Convert.ToInt32(textBox_Pay.Text), dateTimePicker_searchDate.Value.ToString("yyyy-MM-dd-ddd"));
                    Makedatagridview_month();
                }
                
            }
            dataGridView1.DataSource = DespositfromDB;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //int minustotal = 0;
            //int plustotal = 0;
            // TODO 支出用'-' 收入用'+'
            var columnValues = AccountfromDB.AsEnumerable().Select(row =>
            {
                int value;
                if (int.TryParse(row[2].ToString(), out value))
                {
                    return -value;
                }
                return 0; // 或其他適當的預設值
            });

            //if (dataGridView1.Columns[1].HeaderText.Contains("支出"))
            //{
            //    minustotal = -columnValues.Sum();
            //}
            //else if (dataGridView1.Columns[1].HeaderText.Contains("收入"))
            //{
            //    plustotal = columnValues.Sum();
            //}

            int total = columnValues.Sum();
            label_totalValue.Text = total.ToString();
        }
        /// <summary>
        /// 以月分來做datagridview
        /// </summary>
        private void Makedatagridview_month()
        {
            DataTable dataTablemonth = new DataTable();
            AccountfromDB = _db.getdatatable();
            // 複製 AccountfromDB 的結構到 dataTablemonth
            foreach (DataColumn column in AccountfromDB.Columns)
            {
                dataTablemonth.Columns.Add(column.ColumnName, column.DataType);
            }
            for (int i = 0; i < AccountfromDB.Rows.Count; i++)
            {
                if (AccountfromDB.Rows[i]["DATE"].ToString().Substring(5).Contains(dateTimePicker_searchDate.Value.Month.ToString()))
                {
                    DataRow row = dataTablemonth.NewRow();
                    row.ItemArray = AccountfromDB.Rows[i].ItemArray;
                    dataTablemonth.Rows.Add(row);
                }
            }

            DespositfromDB = _db.GetDataTotalTable();
            DespositfromDB.Merge(dataTablemonth);
        }

        private void Caculation()//計算用
        {

        }
    }
}
