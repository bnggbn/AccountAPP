namespace AccountAPP
{
    partial class ScheduleForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dataGridView_schedule = new System.Windows.Forms.DataGridView();
            this.textBox_name          = new System.Windows.Forms.TextBox();
            this.textBox_amount        = new System.Windows.Forms.TextBox();
            this.comboBox_type         = new System.Windows.Forms.ComboBox();
            this.comboBox_freq         = new System.Windows.Forms.ComboBox();
            this.btn_add               = new System.Windows.Forms.Button();
            this.btn_delete            = new System.Windows.Forms.Button();
            this.label_name            = new System.Windows.Forms.Label();
            this.label_type            = new System.Windows.Forms.Label();
            this.label_amount          = new System.Windows.Forms.Label();
            this.label_freq            = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_schedule)).BeginInit();
            this.SuspendLayout();
            // dataGridView_schedule
            this.dataGridView_schedule.Location                       = new System.Drawing.Point(12, 12);
            this.dataGridView_schedule.Name                           = "dataGridView_schedule";
            this.dataGridView_schedule.Size                           = new System.Drawing.Size(560, 260);
            this.dataGridView_schedule.RowTemplate.Height             = 27;
            this.dataGridView_schedule.ColumnHeadersHeightSizeMode    = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_schedule.TabIndex                       = 0;
            this.dataGridView_schedule.CellValueChanged              += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_schedule_CellValueChanged);
            this.dataGridView_schedule.CurrentCellDirtyStateChanged  += new System.EventHandler(this.dataGridView_schedule_CurrentCellDirtyStateChanged);
            // label_name
            this.label_name.Text     = "名稱";
            this.label_name.Location = new System.Drawing.Point(12, 288);
            this.label_name.Size     = new System.Drawing.Size(40, 21);
            this.label_name.Font     = new System.Drawing.Font("微軟正黑體", 10F);
            // textBox_name
            this.textBox_name.Location = new System.Drawing.Point(55, 286);
            this.textBox_name.Size     = new System.Drawing.Size(120, 22);
            this.textBox_name.TabIndex = 1;
            // label_type
            this.label_type.Text     = "類別";
            this.label_type.Location = new System.Drawing.Point(185, 288);
            this.label_type.Size     = new System.Drawing.Size(40, 21);
            this.label_type.Font     = new System.Drawing.Font("微軟正黑體", 10F);
            // comboBox_type
            this.comboBox_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_type.Location      = new System.Drawing.Point(228, 286);
            this.comboBox_type.Size          = new System.Drawing.Size(100, 24);
            this.comboBox_type.TabIndex      = 2;
            // label_amount
            this.label_amount.Text     = "金額";
            this.label_amount.Location = new System.Drawing.Point(12, 322);
            this.label_amount.Size     = new System.Drawing.Size(40, 21);
            this.label_amount.Font     = new System.Drawing.Font("微軟正黑體", 10F);
            // textBox_amount
            this.textBox_amount.Location = new System.Drawing.Point(55, 320);
            this.textBox_amount.Size     = new System.Drawing.Size(120, 22);
            this.textBox_amount.TabIndex = 3;
            // label_freq
            this.label_freq.Text     = "頻率";
            this.label_freq.Location = new System.Drawing.Point(185, 322);
            this.label_freq.Size     = new System.Drawing.Size(40, 21);
            this.label_freq.Font     = new System.Drawing.Font("微軟正黑體", 10F);
            // comboBox_freq
            this.comboBox_freq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_freq.Location      = new System.Drawing.Point(228, 320);
            this.comboBox_freq.Size          = new System.Drawing.Size(100, 24);
            this.comboBox_freq.TabIndex      = 4;
            // btn_add
            this.btn_add.Text     = "新增";
            this.btn_add.Location = new System.Drawing.Point(348, 295);
            this.btn_add.Size     = new System.Drawing.Size(90, 50);
            this.btn_add.TabIndex = 5;
            this.btn_add.Click   += new System.EventHandler(this.btn_add_Click);
            // btn_delete
            this.btn_delete.Text     = "刪除選取";
            this.btn_delete.Location = new System.Drawing.Point(452, 295);
            this.btn_delete.Size     = new System.Drawing.Size(90, 50);
            this.btn_delete.TabIndex = 6;
            this.btn_delete.Click   += new System.EventHandler(this.btn_delete_Click);
            // ScheduleForm
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize    = new System.Drawing.Size(584, 362);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.dataGridView_schedule,
                this.label_name, this.textBox_name,
                this.label_type, this.comboBox_type,
                this.label_amount, this.textBox_amount,
                this.label_freq, this.comboBox_freq,
                this.btn_add, this.btn_delete
            });
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.Name            = "ScheduleForm";
            this.Text            = "定期項目管理";
            this.Load           += new System.EventHandler(this.ScheduleForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_schedule)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dataGridView_schedule;
        private System.Windows.Forms.TextBox      textBox_name;
        private System.Windows.Forms.TextBox      textBox_amount;
        private System.Windows.Forms.ComboBox     comboBox_type;
        private System.Windows.Forms.ComboBox     comboBox_freq;
        private System.Windows.Forms.Button       btn_add;
        private System.Windows.Forms.Button       btn_delete;
        private System.Windows.Forms.Label        label_name;
        private System.Windows.Forms.Label        label_type;
        private System.Windows.Forms.Label        label_amount;
        private System.Windows.Forms.Label        label_freq;
    }
}
