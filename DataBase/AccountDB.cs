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
            
            string createAccount = "CREATE TABLE Account(AccountName TEXT, Type TEXT, AccountValue INTEGER, DATE TEXT)";
            string createDeposit = "CREATE TABLE Deposit(AccountName TEXT, Type TEXT, AccountValue INTEGER, DATE TEXT)";
            string createType = "CREATE TABLE Type(Type TEXT, TypeClass INTEGER)";
            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                
                dbconnect.Open();
                if(!TableExists(dbconnect, "Account"))
                {
                    using (SQLiteCommand CREATEcmd = new SQLiteCommand(createAccount, dbconnect))
                    {
                        CREATEcmd.ExecuteNonQuery();
                    }
                }
                if (!TableExists(dbconnect, "Deposit"))
                {
                    using (SQLiteCommand CREATEcmd = new SQLiteCommand(createDeposit, dbconnect))
                    {
                        CREATEcmd.ExecuteNonQuery();
                    }
                    dbconnect.Close();
                }
                if (!TableExists(dbconnect, "Type"))
                {
                    using (SQLiteCommand CREATEcmd = new SQLiteCommand(createType, dbconnect))
                    {
                        CREATEcmd.ExecuteNonQuery();
                    }
                    dbconnect.Close();
                }
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
        /// 取得帳項 (日期)
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataTable SelectAccount(string Date)
        {
            string Selectstring = $"SELECT * FROM Account WHERE DATE = @DATE";
            //string Selectstring = $"SELECT * FROM Account";
            DataTable dataTable = new DataTable();
            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                dbconnect.Open();
                using (SQLiteCommand Selectcmd = new SQLiteCommand(Selectstring, dbconnect))
                {
                    Selectcmd.Parameters.Clear();
                    Selectcmd.Parameters.AddWithValue("@DATE", Date);
                    Selectcmd.ExecuteNonQuery();
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
            string insertstring = "INSERT INTO Desposit (AccountName,Type,AccountValue,DATE)VALUES(@AccountName,@Type,@AccountValue,@DATE)";

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
    }
}
