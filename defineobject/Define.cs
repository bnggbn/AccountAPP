using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountAPP
{
    // 帳目基本欄位
    internal interface IAccount
    {
        string Name   { get; set; }
        string Type   { get; set; }
        int    Amount { get; set; }
        string Date   { get; set; }
    }

    // 分類資訊
    internal interface IType
    {
        TypeClass TypeClass { get; set; }
    }

    // 定期項目
    internal class ScheduleItem : IAccount, IType
    {
        public long      Id          { get; set; }
        public string    Name        { get; set; }
        public string    Type        { get; set; }
        public int       Amount      { get; set; }
        public string    Date        { get; set; }
        public TypeClass TypeClass   { get; set; }
        public string    Frequency   { get; set; }   // daily / monthly / yearly
        public string    LastApplied { get; set; }
        public bool      Enabled     { get; set; }

        public static ScheduleItem FromRow(System.Data.DataRow row)
        {
            return new ScheduleItem
            {
                Id          = System.Convert.ToInt64(row["Id"]),
                Name        = row["Name"].ToString(),
                Type        = row["Type"].ToString(),
                Amount      = System.Convert.ToInt32(row["Amount"]),
                Frequency   = row["Frequency"].ToString(),
                LastApplied = row["LastApplied"].ToString(),
                Enabled     = System.Convert.ToInt32(row["Enabled"]) == 1,
                Date        = string.Empty,
                TypeClass   = TypeClass.Unknow,
            };
        }

        public string FrequencyLabel
        {
            get
            {
                switch (Frequency)
                {
                    case "daily":   return "每天";
                    case "monthly": return "每月";
                    case "yearly":  return "每年";
                    default:        return Frequency;
                }
            }
        }
    }

    internal enum TypeClass
    {
        income,
        expend,
        total,
        Unknow
    }
}
