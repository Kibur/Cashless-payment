using nmct.ba.cashlessproject.api.Helpers;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class ProductDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"0x0df01d4b-PC", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static List<Product> GetProducts(IEnumerable<Claim> claims)
        {
            List<Product> list = new List<Product>();
            string sql = "SELECT * FROM Products";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);
            while (reader.Read())
            {
                Product p = new Product();
                p.ID = Convert.ToInt32(reader["ID"]);
                p.ProductName = reader["ProductName"].ToString();
                p.Price = Double.Parse(reader["Price"].ToString());

                list.Add(p);
            }

            return list;
        }

        public static int InsertProduct(Product p, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Products VALUES(@ProductName,@Price)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ProductName", p.ProductName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Price", p.Price);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2);
        }

        public static void UpdateProduct(Product p, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Products SET ProductName=@ProductName, Price=@Price WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ProductName", p.ProductName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Price", p.Price);
            DbParameter par3 = Database.AddParameter("AdminDB", "@ID", p.ID);
            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3);
        }

        public static void DeleteProduct(int id, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Products WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", id);
            DbConnection con = Database.GetConnection(CreateConnectionString(claims));
            Database.ModifyData(con, sql, par1);
        }
    }
}