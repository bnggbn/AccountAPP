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
            
            string creatstring = "CREATE TABLE Account(AccountName TEXT, Type TEXT, AccountValue INTEGER, DATE TEXT)";
            string creatstring2 = "CREATE TABLE Deposit(AccountName TEXT, Type TEXT, AccountValue INTEGER, DATE TEXT)";
            using (SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                
                dbconnect.Open();
                if(!TableExists(dbconnect, "Account"))
                {
                    using (SQLiteCommand CREATEcmd = new SQLiteCommand(creatstring, dbconnect))
                    {
                        CREATEcmd.ExecuteNonQuery();
                    }
                }
                if (!TableExists(dbconnect, "Deposit"))
                {
                    using (SQLiteCommand CREATEcmd = new SQLiteCommand(creatstring2, dbconnect))
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

        /// <summary>
        /// 加入帳
        /// </summary>
        /// <param name="accountname"></param>
        /// <param name="type"></param>
        /// <param name="accountvalue"></param>
        /// <param name="date"></param>
        public void InsertDataAccount(string accountname ,string type ,int accountvalue, string date)
        {
            string insertstring = "INSERT INTO Account (AccountName,Type,AccountValue,DATE)VALUES(@AccountName,@Type,@AccountValue,@DATE)";

            using(SQLiteConnection dbconnect = new SQLiteConnection(Connectstring))
            {
                dbconnect.Open();

                using(SQLiteCommand INSERTcmd = new SQLiteCommand(insertstring, dbconnect))
                {
                    INSERTcmd.Parameters.Clear();
                    
                    INSERTcmd.Parameters.AddWithValue("@AccountName",accountname);
                    INSERTcmd.Parameters.AddWithValue("@Type", type);
                    INSERTcmd.Parameters.AddWithValue("@AccountValue", accountvalue);
                    INSERTcmd.Parameters.AddWithValue("@DATE", date);

                    INSERTcmd.ExecuteNonQuery();
                }
                dbconnect.Close();
            }
        }
        /// <summary>
        /// 取得帳項
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataTable getdatatable()
        {
            string Selectstring = "SELECT * FROM Account WHERE DATE";
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
        /// 取得總數錢
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataTotalTable()
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
    }
}
