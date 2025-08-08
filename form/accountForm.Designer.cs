namespace AccountAPP
{
    partial class Account
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Button_Input = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dateTimePicker_searchDate = new System.Windows.Forms.DateTimePicker();
            this.Today_datepicker = new System.Windows.Forms.Label();
            this.Target_Datepicker = new System.Windows.Forms.Label();
            this.label_accountName = new System.Windows.Forms.Label();
            this.label_accountType = new System.Windows.Forms.Label();
            this.label_accountValue = new System.Windows.Forms.Label();
            this.textbox_Name = new System.Windows.Forms.TextBox();
            this.textBox_Pay = new System.Windows.Forms.TextBox();
            this.comboBox_Type = new System.Windows.Forms.ComboBox();
            this.Button_Search = new System.Windows.Forms.Button();
            this.label_todayDate = new System.Windows.Forms.Label();
            this.label_Total = new System.Windows.Forms.Label();
            this.label_totalValue = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.AddType = new System.Windows.Forms.Button();
            this.button_income = new System.Windows.Forms.Button();
            this.button_expend = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_newType = new System.Windows.Forms.Label();
            this.button_edit = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.textBox_newType = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Button_Input
            // 
            this.Button_Input.Location = new System.Drawing.Point(230, 300);
            this.Button_Input.Name = "Button_Input";
            this.Button_Input.Size = new System.Drawing.Size(124, 58);
            this.Button_Input.TabIndex = 0;
            this.Button_Input.Text = "輸入項目";
            this.Button_Input.UseVisualStyleBackColor = true;
            this.Button_Input.Click += new System.EventHandler(this.Button_Input_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(376, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(522, 354);
            this.dataGridView1.TabIndex = 1;
            // 
            // dateTimePicker_searchDate
            // 
            this.dateTimePicker_searchDate.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dateTimePicker_searchDate.Location = new System.Drawing.Point(104, 60);
            this.dateTimePicker_searchDate.Name = "dateTimePicker_searchDate";
            this.dateTimePicker_searchDate.Size = new System.Drawing.Size(139, 24);
            this.dateTimePicker_searchDate.TabIndex = 3;
            // 
            // Today_datepicker
            // 
            this.Today_datepicker.AutoSize = true;
            this.Today_datepicker.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Today_datepicker.Location = new System.Drawing.Point(6, 13);
            this.Today_datepicker.Name = "Today_datepicker";
            this.Today_datepicker.Size = new System.Drawing.Size(82, 21);
            this.Today_datepicker.TabIndex = 4;
            this.Today_datepicker.Text = "今日日期 :";
            // 
            // Target_Datepicker
            // 
            this.Target_Datepicker.AutoSize = true;
            this.Target_Datepicker.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Target_Datepicker.Location = new System.Drawing.Point(7, 62);
            this.Target_Datepicker.Name = "Target_Datepicker";
            this.Target_Datepicker.Size = new System.Drawing.Size(74, 21);
            this.Target_Datepicker.TabIndex = 5;
            this.Target_Datepicker.Text = "查詢日期";
            // 
            // label_accountName
            // 
            this.label_accountName.AutoSize = true;
            this.label_accountName.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_accountName.Location = new System.Drawing.Point(24, 269);
            this.label_accountName.Name = "label_accountName";
            this.label_accountName.Size = new System.Drawing.Size(42, 21);
            this.label_accountName.TabIndex = 6;
            this.label_accountName.Text = "帳目";
            // 
            // label_accountType
            // 
            this.label_accountType.AutoSize = true;
            this.label_accountType.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_accountType.Location = new System.Drawing.Point(24, 300);
            this.label_accountType.Name = "label_accountType";
            this.label_accountType.Size = new System.Drawing.Size(42, 21);
            this.label_accountType.TabIndex = 7;
            this.label_accountType.Text = "類別";
            // 
            // label_accountValue
            // 
            this.label_accountValue.AutoSize = true;
            this.label_accountValue.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_accountValue.Location = new System.Drawing.Point(24, 330);
            this.label_accountValue.Name = "label_accountValue";
            this.label_accountValue.Size = new System.Drawing.Size(42, 21);
            this.label_accountValue.TabIndex = 8;
            this.label_accountValue.Text = "金額";
            // 
            // textbox_Name
            // 
            this.textbox_Name.Location = new System.Drawing.Point(85, 268);
            this.textbox_Name.Name = "textbox_Name";
            this.textbox_Name.Size = new System.Drawing.Size(121, 22);
            this.textbox_Name.TabIndex = 9;
            // 
            // textBox_Pay
            // 
            this.textBox_Pay.Location = new System.Drawing.Point(85, 331);
            this.textBox_Pay.Name = "textBox_Pay";
            this.textBox_Pay.Size = new System.Drawing.Size(121, 22);
            this.textBox_Pay.TabIndex = 10;
            // 
            // comboBox_Type
            // 
            this.comboBox_Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Type.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox_Type.FormattingEnabled = true;
            this.comboBox_Type.Items.AddRange(new object[] {
            "一般支出",
            "月繳",
            "年繳",
            "上班收入",
            "投資收入"});
            this.comboBox_Type.Location = new System.Drawing.Point(85, 301);
            this.comboBox_Type.Name = "comboBox_Type";
            this.comboBox_Type.Size = new System.Drawing.Size(121, 24);
            this.comboBox_Type.TabIndex = 11;
            this.comboBox_Type.SelectedIndexChanged += new System.EventHandler(this.comboBox_Type_SelectedIndexChanged);
            this.comboBox_Type.Click += new System.EventHandler(this.comboBox_Type_Click);
            // 
            // Button_Search
            // 
            this.Button_Search.Location = new System.Drawing.Point(254, 56);
            this.Button_Search.Name = "Button_Search";
            this.Button_Search.Size = new System.Drawing.Size(94, 38);
            this.Button_Search.TabIndex = 12;
            this.Button_Search.Text = "查詢";
            this.Button_Search.UseVisualStyleBackColor = true;
            this.Button_Search.Click += new System.EventHandler(this.Button_Search_Click);
            // 
            // label_todayDate
            // 
            this.label_todayDate.AutoSize = true;
            this.label_todayDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_todayDate.Location = new System.Drawing.Point(104, 13);
            this.label_todayDate.Name = "label_todayDate";
            this.label_todayDate.Size = new System.Drawing.Size(88, 21);
            this.label_todayDate.TabIndex = 13;
            this.label_todayDate.Text = "todaydate";
            // 
            // label_Total
            // 
            this.label_Total.AutoSize = true;
            this.label_Total.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_Total.Location = new System.Drawing.Point(22, 37);
            this.label_Total.Name = "label_Total";
            this.label_Total.Size = new System.Drawing.Size(50, 21);
            this.label_Total.TabIndex = 14;
            this.label_Total.Text = "存款 :";
            // 
            // label_totalValue
            // 
            this.label_totalValue.AutoSize = true;
            this.label_totalValue.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_totalValue.Location = new System.Drawing.Point(104, 37);
            this.label_totalValue.Name = "label_totalValue";
            this.label_totalValue.Size = new System.Drawing.Size(20, 21);
            this.label_totalValue.TabIndex = 15;
            this.label_totalValue.Text = "0";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            // 
            // AddType
            // 
            this.AddType.Location = new System.Drawing.Point(230, 259);
            this.AddType.Name = "AddType";
            this.AddType.Size = new System.Drawing.Size(124, 35);
            this.AddType.TabIndex = 16;
            this.AddType.Text = "新增類別";
            this.AddType.UseVisualStyleBackColor = true;
            this.AddType.Click += new System.EventHandler(this.AddType_Click);
            // 
            // button_income
            // 
            this.button_income.Location = new System.Drawing.Point(46, 34);
            this.button_income.Name = "button_income";
            this.button_income.Size = new System.Drawing.Size(66, 33);
            this.button_income.TabIndex = 17;
            this.button_income.Text = "收入";
            this.button_income.UseVisualStyleBackColor = true;
            this.button_income.Visible = false;
            this.button_income.Click += new System.EventHandler(this.button_income_Click);
            // 
            // button_expend
            // 
            this.button_expend.Location = new System.Drawing.Point(111, 34);
            this.button_expend.Name = "button_expend";
            this.button_expend.Size = new System.Drawing.Size(68, 33);
            this.button_expend.TabIndex = 18;
            this.button_expend.Text = "支出";
            this.button_expend.UseVisualStyleBackColor = true;
            this.button_expend.Visible = false;
            this.button_expend.Click += new System.EventHandler(this.button_expend_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label_newType);
            this.panel1.Controls.Add(this.button_edit);
            this.panel1.Controls.Add(this.button_cancel);
            this.panel1.Controls.Add(this.textBox_newType);
            this.panel1.Controls.Add(this.button_income);
            this.panel1.Controls.Add(this.button_expend);
            this.panel1.Location = new System.Drawing.Point(14, 139);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 123);
            this.panel1.TabIndex = 19;
            this.panel1.Visible = false;
            // 
            // label_newType
            // 
            this.label_newType.AutoSize = true;
            this.label_newType.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_newType.Location = new System.Drawing.Point(10, 9);
            this.label_newType.Name = "label_newType";
            this.label_newType.Size = new System.Drawing.Size(58, 21);
            this.label_newType.TabIndex = 24;
            this.label_newType.Text = "新類別";
            // 
            // button_edit
            // 
            this.button_edit.Location = new System.Drawing.Point(2, 85);
            this.button_edit.Name = "button_edit";
            this.button_edit.Size = new System.Drawing.Size(66, 33);
            this.button_edit.TabIndex = 22;
            this.button_edit.Text = "確認新增";
            this.button_edit.UseVisualStyleBackColor = true;
            this.button_edit.Visible = false;
            this.button_edit.Click += new System.EventHandler(this.button_edit_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(128, 85);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(68, 33);
            this.button_cancel.TabIndex = 23;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Visible = false;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // textBox_newType
            // 
            this.textBox_newType.Location = new System.Drawing.Point(67, 9);
            this.textBox_newType.Name = "textBox_newType";
            this.textBox_newType.Size = new System.Drawing.Size(121, 22);
            this.textBox_newType.TabIndex = 21;
            // 
            // Account
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(907, 377);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.AddType);
            this.Controls.Add(this.label_totalValue);
            this.Controls.Add(this.label_Total);
            this.Controls.Add(this.label_todayDate);
            this.Controls.Add(this.Button_Search);
            this.Controls.Add(this.comboBox_Type);
            this.Controls.Add(this.textBox_Pay);
            this.Controls.Add(this.textbox_Name);
            this.Controls.Add(this.label_accountValue);
            this.Controls.Add(this.label_accountType);
            this.Controls.Add(this.label_accountName);
            this.Controls.Add(this.Target_Datepicker);
            this.Controls.Add(this.Today_datepicker);
            this.Controls.Add(this.dateTimePicker_searchDate);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.Button_Input);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "Account";
            this.Text = "記帳器";
            this.Load += new System.EventHandler(this.Account_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Button_Input;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker dateTimePicker_searchDate;
        private System.Windows.Forms.Label Today_datepicker;
        private System.Windows.Forms.Label Target_Datepicker;
        private System.Windows.Forms.Label label_accountName;
        private System.Windows.Forms.Label label_accountType;
        private System.Windows.Forms.Label label_accountValue;
        private System.Windows.Forms.TextBox textbox_Name;
        private System.Windows.Forms.TextBox textBox_Pay;
        private System.Windows.Forms.ComboBox comboBox_Type;
        private System.Windows.Forms.Button Button_Search;
        private System.Windows.Forms.Label label_todayDate;
        private System.Windows.Forms.Label label_Total;
        private System.Windows.Forms.Label label_totalValue;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button AddType;
        private System.Windows.Forms.Button button_income;
        private System.Windows.Forms.Button button_expend;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox_newType;
        private System.Windows.Forms.Label label_newType;
        private System.Windows.Forms.Button button_edit;
        private System.Windows.Forms.Button button_cancel;
    }
}

