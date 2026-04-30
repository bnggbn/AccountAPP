using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace AccountAPP
{
    internal class AccountDB
    {
        public static string DBPath = Path.Combine("DB", "Account.db");
        public string Connectstring { get; set; } = $"Data Source={DBPath};version=3";

        public void creatDatabase()
        {
            
            if (!Directory.Exists("DB"))
            {
                Directory.CreateDirectory("DB");
            }
            
            string createAccount  = "CREATE TABLE Account(AccountName TEXT, Type TEXT, AccountValue INTEGER, DATE TEXT)";
            string createDeposit  = "CREATE TABLE Deposit(AccountName TEXT, Type TEXT, AccountValue INTEGER, DATE TEXT)";
            string createType     = "CREATE TABLE Type(Type TEXT, TypeClass INTEGER)";
            string createSchedule = "CREATE TABLE Schedule(Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Type TEXT, Amount INTEGER, Frequency TEXT, LastApplied TEXT DEFAULT '', Enabled INTEGER DEFAULT 1)";
            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                dbconnect.Open();
                if (!TableExists(dbconnect, "Account"))
                {
                    using (SQLiteCommand CREATEcmd = new SQLiteCommand(createAccount, dbconnect))
                        CREATEcmd.ExecuteNonQuery();
                }
                if (!TableExists(dbconnect, "Deposit"))
                {
                    using (SQLiteCommand CREATEcmd = new SQLiteCommand(createDeposit, dbconnect))
                        CREATEcmd.ExecuteNonQuery();
                }
                if (!TableExists(dbconnect, "Type"))
                {
                    using (SQLiteCommand CREATEcmd = new SQLiteCommand(createType, dbconnect))
                        CREATEcmd.ExecuteNonQuery();
                }
                if (!TableExists(dbconnect, "Schedule"))
                {
                    using (SQLiteCommand CREATEcmd = new SQLiteCommand(createSchedule, dbconnect))
                        CREATEcmd.ExecuteNonQuery();
                }
                dbconnect.Close();
            }
        }

        private bool TableExists(SQLiteConnection dbconnect, string tableName)
        {
            string query = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'";
            using (SQLiteCommand command = new SQLiteCommand(query, dbconnect))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }

            //throw new NotImplementedException();
        }



        #region Account
        /// <summary>
        /// 加入帳
        /// </summary>

        /// <param name="accountname"></param>
        /// <param name="type"></param>
        /// <param name="accountvalue"></param>
        /// <param name="date"></param>
        public void InsertDataAccount(string accountname, string type, int accountvalue, string date)
        {
            string insertstring = "INSERT INTO Account (AccountName,Type,AccountValue,DATE)VALUES(@AccountName,@Type,@AccountValue,@DATE)";

            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                dbconnect.Open();

                using (SQLiteCommand INSERTcmd = new SQLiteCommand(insertstring, dbconnect))
                {
                    INSERTcmd.Parameters.Clear();

                    INSERTcmd.Parameters.AddWithValue("@AccountName", accountname);
                    INSERTcmd.Parameters.AddWithValue("@Type", type);
                    INSERTcmd.Parameters.AddWithValue("@AccountValue", accountvalue);
                    INSERTcmd.Parameters.AddWithValue("@DATE", date);

                    INSERTcmd.ExecuteNonQuery();
                }
                dbconnect.Close();
            }
        }
        /// <summary>
        /// 取得帳項 (月份, 格式 yyyy-MM)
        /// </summary>
        public DataTable SelectAccountByMonth(string yearMonth)
        {
            string Selectstring = "SELECT * FROM Account WHERE DATE LIKE @YearMonth ORDER BY DATE";
            DataTable dataTable = new DataTable();
            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                dbconnect.Open();
                using (SQLiteCommand Selectcmd = new SQLiteCommand(Selectstring, dbconnect))
                {
                    Selectcmd.Parameters.AddWithValue("@YearMonth", yearMonth + "%");
                    using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(Selectcmd))
                    {
                        dataAdapter.Fill(dataTable);
                    }
                }
                dbconnect.Close();
            }
            return dataTable;
        }

        /// <summary>
        /// 刪除一筆帳目 (依欄位比對)
        /// </summary>
        public void DeleteAccount(string accountName, string type, int accountValue, string date)
        {
            string deleteString = "DELETE FROM Account WHERE rowid = (SELECT rowid FROM Account WHERE AccountName=@AccountName AND Type=@Type AND AccountValue=@AccountValue AND DATE=@DATE LIMIT 1)";
            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                dbconnect.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(deleteString, dbconnect))
                {
                    cmd.Parameters.AddWithValue("@AccountName", accountName);
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@AccountValue", accountValue);
                    cmd.Parameters.AddWithValue("@DATE", date);
                    cmd.ExecuteNonQuery();
                }
                dbconnect.Close();
            }
        }
        #endregion

        #region
        /// <summary>
        /// 取得帳錢
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataTable SelectAccountValue()
        {
            string Selectstring = $"SELECT Account.AccountValue, Account.Type, TYPE.TypeClass FROM Account JOIN TYPE ON Account.TYPE = TYPE.Type;";
            //string Selectstring = $"SELECT * FROM Account";
            DataTable dataTable = new DataTable();
            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                dbconnect.Open();
                using (SQLiteCommand Selectcmd = new SQLiteCommand(Selectstring, dbconnect))
                {
                    using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(Selectcmd))
                    {
                        dataAdapter.Fill(dataTable);
                    }
                }
                dbconnect.Close();
            }
            return dataTable;
        }
        #endregion

        #region
        /// <summary>
        /// 取得總額
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataTable SelectTotalFromAccount()
        {
            string Selectstring = $"SELECT Account.AccountValue, Account.Type, TYPE.TypeClass FROM Account JOIN TYPE ON Account.TYPE = TYPE.Type WHERE TYPE.TypeClass = 2 ORDER BY Account.DATE DESC;";
            //string Selectstring = $"SELECT * FROM Account";
            DataTable dataTable = new DataTable();
            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                dbconnect.Open();
                using (SQLiteCommand Selectcmd = new SQLiteCommand(Selectstring, dbconnect))
                {
                    using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(Selectcmd))
                    {
                        dataAdapter.Fill(dataTable);
                    }
                }
                dbconnect.Close();
            }

            return dataTable;
        }
        #endregion


        #region Deposit
        /// <summary>
        /// 取得總數錢
        /// </summary>
        /// <returns></returns>
        public DataTable SelectDepositTable()
        {
            string Selectstring = "SELECT * FROM Deposit";
            DataTable dataTable = new DataTable();
            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                dbconnect.Open();
                using (SQLiteCommand Selectcmd = new SQLiteCommand(Selectstring, dbconnect))
                {
                    using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(Selectcmd))
                    {
                        dataAdapter.Fill(dataTable);
                    }
                }
                dbconnect.Close();
            }
            return dataTable;
        }
        /// <summary>
        /// 加入目前餘額
        /// </summary>
        /// <param name="accountname"></param>
        /// <param name="type"></param>
        /// <param name="accountvalue"></param>
        /// <param name="date"></param>
        public void InsertDataDesposit(string accountname, string type, int accountvalue, string date)
        {
            string insertstring = "INSERT INTO Deposit (AccountName,Type,AccountValue,DATE)VALUES(@AccountName,@Type,@AccountValue,@DATE)";

            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                dbconnect.Open();

                using (SQLiteCommand INSERTcmd = new SQLiteCommand(insertstring, dbconnect))
                {
                    INSERTcmd.Parameters.Clear();

                    INSERTcmd.Parameters.AddWithValue("@AccountName", accountname);
                    INSERTcmd.Parameters.AddWithValue("@Type", type);
                    INSERTcmd.Parameters.AddWithValue("@AccountValue", accountvalue);
                    INSERTcmd.Parameters.AddWithValue("@DATE", date);

                    INSERTcmd.ExecuteNonQuery();
                }
                dbconnect.Close();
            }
        }
        #endregion


        #region Type
        /// <summary>
        /// 取得總數錢
        /// </summary>
        /// <returns></returns>
        public DataTable SelectTypeTable()
        {
            string Selectstring = "SELECT * FROM Type";
            DataTable dataTable = new DataTable();
            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                dbconnect.Open();
                using (SQLiteCommand Selectcmd = new SQLiteCommand(Selectstring, dbconnect))
                {
                    using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(Selectcmd))
                    {
                        dataAdapter.Fill(dataTable);
                    }
                }
                dbconnect.Close();
            }
            return dataTable;
        }
        /// <summary>
        /// 加入目前餘額
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeclass">0 = 收入, 1 = 支出</param>
        public void InsertDataType(string type, TypeClass typeclass)
        {
            string insertstring = "INSERT INTO Type (Type, TypeClass)VALUES(@Type, @TypeClass)";

            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                dbconnect.Open();

                using (SQLiteCommand INSERTcmd = new SQLiteCommand(insertstring, dbconnect))
                {
                    INSERTcmd.Parameters.Clear();
                    INSERTcmd.Parameters.AddWithValue("@Type", type);
                    INSERTcmd.Parameters.AddWithValue("@TypeClass", (int)typeclass);
                    INSERTcmd.ExecuteNonQuery();
                }
                dbconnect.Close();
            }
        }
        #endregion


        #region Schedule
        /// <summary>
        /// 取得所有定期項目
        /// </summary>
        public DataTable SelectSchedules()
        {
            string sql = "SELECT * FROM Schedule ORDER BY Id";
            DataTable dataTable = new DataTable();
            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                dbconnect.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, dbconnect))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    adapter.Fill(dataTable);
                dbconnect.Close();
            }
            return dataTable;
        }

        /// <summary>
        /// 新增定期項目
        /// </summary>
        /// <param name="frequency">daily / monthly / yearly</param>
        public void InsertSchedule(string name, string type, int amount, string frequency)
        {
            string sql = "INSERT INTO Schedule (Name, Type, Amount, Frequency, LastApplied, Enabled) VALUES (@Name, @Type, @Amount, @Frequency, '', 1)";
            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                dbconnect.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, dbconnect))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@Frequency", frequency);
                    cmd.ExecuteNonQuery();
                }
                dbconnect.Close();
            }
        }

        /// <summary>
        /// 刪除定期項目
        /// </summary>
        public void DeleteSchedule(long id)
        {
            string sql = "DELETE FROM Schedule WHERE Id = @Id";
            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                dbconnect.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, dbconnect))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
                dbconnect.Close();
            }
        }

        /// <summary>
        /// 啟用 / 停用定期項目
        /// </summary>
        public void SetScheduleEnabled(long id, bool enabled)
        {
            string sql = "UPDATE Schedule SET Enabled = @Enabled WHERE Id = @Id";
            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                dbconnect.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, dbconnect))
                {
                    cmd.Parameters.AddWithValue("@Enabled", enabled ? 1 : 0);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
                dbconnect.Close();
            }
        }

        /// <summary>
        /// 更新最後執行日期
        /// </summary>
        public void UpdateScheduleLastApplied(long id, string date)
        {
            string sql = "UPDATE Schedule SET LastApplied = @Date WHERE Id = @Id";
            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                dbconnect.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, dbconnect))
                {
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
                dbconnect.Close();
            }
        }

        /// <summary>
        /// 檢查並執行到期的定期項目，回傳本次執行了幾筆
        /// </summary>
        public int ApplyDueSchedules()
        {
            DataTable schedules = SelectSchedules();
            string today     = DateTime.Now.ToString("yyyy-MM-dd");
            string thisMonth = DateTime.Now.ToString("yyyy-MM");
            string thisYear  = DateTime.Now.ToString("yyyy");
            string dateFull  = DateTime.Now.ToString("yyyy-MM-dd-ddd");
            int count = 0;

            foreach (DataRow row in schedules.Rows)
            {
                if (Convert.ToInt32(row["Enabled"]) == 0) continue;

                string lastApplied = row["LastApplied"].ToString();
                string frequency   = row["Frequency"].ToString();

                bool isDue = false;
                switch (frequency)
                {
                    case "daily":   isDue = lastApplied != today;                  break;
                    case "monthly": isDue = !lastApplied.StartsWith(thisMonth);    break;
                    case "yearly":  isDue = !lastApplied.StartsWith(thisYear);     break;
                }

                if (!isDue) continue;

                long id     = Convert.ToInt64(row["Id"]);
                string name = row["Name"].ToString();
                string type = row["Type"].ToString();
                int amount  = Convert.ToInt32(row["Amount"]);

                InsertDataAccount(name, type, amount, dateFull);
                UpdateScheduleLastApplied(id, today);
                count++;
            }
            return count;
        }
        #endregion
    }
}
