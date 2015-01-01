using nmct.ba.cashlessproject.api.Helpers;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class ErrorlogDA
    {
        public static void InsertErrorlog(Errorlog e)
        {
            string sql = "INSERT INTO Errorlog VALUES(@RegisterID, @Timestamp, @Message, @Stacktrace)";
            DbParameter par1 = Database.AddParameter("KlantDB", "@RegisterID", e.RegisterID);
            DbParameter par2 = Database.AddParameter("KlantDB", "@Timestamp", e.Timestamp);
            DbParameter par3 = Database.AddParameter("KlantDB", "@Message", e.Message);
            DbParameter par4 = Database.AddParameter("KlantDB", "@Stacktrace", e.Stacktrace);

            Database.InsertData(Database.GetConnection("KlantDB"), sql, par1, par2, par3, par4);
        }
    }
}