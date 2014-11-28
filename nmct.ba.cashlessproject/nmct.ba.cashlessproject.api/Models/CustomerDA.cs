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
    public class CustomerDA
    {
        public static List<Customer> GetCustomers(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            List<Customer> list = new List<Customer>();
            string sql = "SELECT * FROM Customer";

            DbConnection con = Database.GetConnection(Database.CreateConnectionString("System.Data.SqlClient", @"0x0df01d4b-PC\ITbedrijf", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass)));

            DbDataReader reader = Database.GetData(con, sql);
            while (reader.Read())
            {
                list.Add(new Customer()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    CustomerName = reader["CustomerName"].ToString()
                });
            }

            return list;
        }
    }
}