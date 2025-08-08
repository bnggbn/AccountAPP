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

       
        private event EventHandler EventChangeDate;
        public string SearchDate 
        {
            get { return targetDate; }
            set 
            {
                if(value != targetDate)
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
            SearchDate = dateTimePicker_searchDate.Value.ToString("yyyy-MM-dd-ddd");
            
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

            _db.creatDatabase();

            dataGridView1.DataSource = AccountfromDB;
            //initial
            if (AccountfromDB.Columns.Count != 0)
            {
                dataGridView1.Columns[3].Width = 150;
                //dataGridView1.AutoResizeColumns();
                dataGridView1.AllowUserToDeleteRows = false; // 設為 true 允許刪除行
                dataGridView1.ReadOnly = true; // 設為 false 允許編輯資料
                dataGridView1.Columns["AccountName"].HeaderText = "帳目";
                dataGridView1.Columns["Type"].HeaderText = "類別";
                dataGridView1.Columns["AccountValue"].HeaderText = "數額";
                dataGridView1.Columns["DATE"].HeaderText = "日期";
            }

            //initial
            #region
            if (_db.SelectTypeTable().Rows.Count < 1)
            {
                List<string> initiallist = new List<string>()
                {
                      "一般支出" ,
                      "月繳" ,
                      "年繳" ,
                      "上班收入" ,
                      "投資收入",
                      "總額"
                };
                foreach (var item in initiallist)
                {
                    if (item.ToString().Contains("總額"))
                    {
                        _db.InsertDataType(item.ToString(), TypeClass.total);
                    }
                    else if (item.ToString().Contains("收入"))
                    {
                        _db.InsertDataType(item.ToString(), TypeClass.income);
                    }
                    else
                    {
                        _db.InsertDataType(item.ToString(), TypeClass.expend);
                    }
                }

            }
            #endregion

            if (comboBox_Type.Items.Count != _db.SelectTypeTable().Rows.Count)
            {
                ResetTypeCombobox();
            }

            //初始化總額
            if (_db.SelectTotalFromAccount().Rows.Count > 0 && Convert.ToInt32(_db.SelectTotalFromAccount().Rows[0]["AccountValue"]) != 0)//只用於不存在總額若存在則找最新總額
            {
                label_totalValue.Text = _db.SelectTotalFromAccount().Rows[0]["AccountValue"].ToString();
            }
            else
            {
                label_totalValue.Text = Cal_total().ToString();
            }
           


        }
        private void Button_Search_Click(object sender, EventArgs e)
        {
            SearchDate = dateTimePicker_searchDate.Value.ToString("yyyy-MM-dd-ddd");
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
                    Makedatagridview_day();
                }
                
            }
            dataGridView1.DataSource = DespositfromDB;
        }

        private void comboBox_Type_Click(object sender, EventArgs e)
        {
            ResetTypeCombobox();
        }

        private void comboBox_Type_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void OnChangeDate(object sender, EventArgs e)
        {
            label_todayDate.Text = dateTimePicker_searchDate.Value.ToString("yyyy-MM-dd");
            Makedatagridview_day();
            dataGridView1.DataSource = AccountfromDB;
        }


        #region new type panel

        TypeClass InsertTypeClass;
        private void AddType_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            button_income.Enabled = true;
            button_income.Enabled = true;
            InsertTypeClass = TypeClass.Unknow;
        }

        private void button_income_Click(object sender, EventArgs e)
        {
            InsertTypeClass = TypeClass.income;
            button_income.BackColor = Color.LightBlue;
            button_expend.BackColor = Color.White;
            button_income.Enabled = false;
        }

        private void button_expend_Click(object sender, EventArgs e)
        {
            InsertTypeClass = TypeClass.expend;
            button_income.BackColor = Color.White;
            button_expend.BackColor = Color.LightBlue;
            button_expend.Enabled = false;
        }

        private void button_edit_Click(object sender, EventArgs e)
        {
            if(InsertTypeClass != TypeClass.Unknow && textBox_newType.Text != string.Empty)
            {
                _db.InsertDataType(textBox_newType.Text.ToString(), InsertTypeClass);
            }
            else
            {
                string ERROR = "";
                if (textBox_newType.Text != string.Empty)
                {
                    ERROR += "Please input typename";
                }
                if (InsertTypeClass != TypeClass.Unknow)
                {
                    ERROR += "please choose data type";
                }
                MessageBox.Show(ERROR);
            }
           
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }
        #endregion


        #region private method
        /// <summary>
        /// 以日來做datagridview
        /// </summary>
        private void Makedatagridview_day()
        {
            DataTable dataTablemonth = new DataTable();

            //select
            AccountfromDB = _db.SelectAccount(SearchDate);
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

            DespositfromDB = _db.SelectDepositTable();
            DespositfromDB.Merge(dataTablemonth);
        }

        private long Cal_total()//計算用(只用於不存在總額項時)
        {
            DataTable dataTable = _db.SelectAccountValue(); //經過處裡表內包含typeclass
            long total = 0;
            for (int i = 0; i < dataTable.Rows.Count; i++) 
            {
                string TypeclassStr = dataTable.Rows[i]["TypeClass"].ToString();

                if(Enum.TryParse(TypeclassStr, out TypeClass typeClass))
                {
                    if (typeClass == TypeClass.income)
                    {
                        total += (Int64)dataTable.Rows[i]["AccountValue"];
                    }
                    else if (typeClass == TypeClass.expend)
                    {
                        total -= (Int64)dataTable.Rows[i]["AccountValue"];
                    }
                }   
            }

            //將total寫入db 並將typeclass 設為總額
            _db.InsertDataAccount($"{DateTime.Now.ToString("yyyy-MM-dd")} : 總額", "總額", Convert.ToInt32(total) ,DateTime.Now.ToString("yyyy-MM-dd-ddd"));

            return total;
        }

        private void ResetTypeCombobox()
        {
            comboBox_Type.Items.Clear();

            for (int i = 0; i < _db.SelectTypeTable().Rows.Count; i++)
            {
                comboBox_Type.Items.Add(_db.SelectTypeTable().Rows[i]["Type"].ToString());
            }
        }





        #endregion

    
    }
}
