using nmct.ba.cashlessproject.api.Helpers;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class SaleDA
    {
        public static List<Sale> GetSales(IEnumerable<Claim> claims)
        {
            List<Sale> list = new List<Sale>();
            string sql = "SELECT * FROM Sales";
            DbDataReader reader = Database.GetData(Database.GetConnection("KlantDB"), sql);
            while (reader.Read())
            {
                Sale s = new Sale();
                s.ID = Convert.ToInt32(reader["ID"]);
                s.Timestamp = Convert.ToInt32(reader["Timestamp"]);
                s.Customer = CustomerDA.GetCustomerById(Convert.ToInt32(reader["CustomerID"]));
                s.Register = RegisterDA.GetRegisterById(Convert.ToInt32(reader["RegisterID"]), claims);
                s.Product = ProductDA.GetProductById(Convert.ToInt32(reader["ProductID"]));
                s.Amount = Convert.ToInt32(reader["Amount"]);
                s.TotalPrice = Convert.ToDouble(reader["TotalPrice"]);

                list.Add(s);
            }

            return list;
        }

        public static int InsertSale(Sale s, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Sales VALUES(@Timestamp,@CustomerID,@RegisterID,@ProductID,@Amount,@TotalPrice)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@Timestamp", s.Timestamp);
            DbParameter par2 = Database.AddParameter("AdminDB", "@CustomerID", s.Customer.ID);
            DbParameter par3 = Database.AddParameter("AdminDB", "@RegisterID", s.Register.ID);
            DbParameter par4 = Database.AddParameter("AdminDB", "@ProductID", s.Product.ID);
            DbParameter par5 = Database.AddParameter("AdminDB", "@Amount", s.Amount);
            DbParameter par6 = Database.AddParameter("AdminDB", "@TotalPrice", s.TotalPrice);
            return Database.InsertData(Database.GetConnection("KlantDB"), sql, par1, par2, par3, par4, par5, par6);
        }
    }
}