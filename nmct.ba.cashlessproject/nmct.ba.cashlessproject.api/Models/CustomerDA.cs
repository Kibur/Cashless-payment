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
    public class CustomerDA
    {
        /*private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"0x0df01d4b-PC", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }*/

        public static List<Customer> GetCustomers(IEnumerable<Claim> claims)
        {
            List<Customer> list = new List<Customer>();
            string sql = "SELECT * FROM Customers";
            DbDataReader reader = Database.GetData(Database.GetConnection("KlantDB"), sql);
            while (reader.Read())
            {
                Customer c = new Customer();
                c.ID = Convert.ToInt32(reader["ID"]);
                c.CustomerName = reader["CustomerName"].ToString();
                c.Address = reader["Address"].ToString();
                if (!DBNull.Value.Equals(reader["Picture"]))
                    c.Picture = (byte[])reader["Picture"];
                else
                    c.Picture = new byte[0];
                c.Balance = Double.Parse(reader["Balance"].ToString());

                list.Add(c);
            }

            return list;
        }

        public static int InsertCustomer(Customer c, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Customers VALUES(@CustomerName,@Address,@Picture,@Balance)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@CustomerName", c.CustomerName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Address", c.Address);
            DbParameter par3 = Database.AddParameter("AdminDB", "@Picture", c.Picture);
            DbParameter par4 = Database.AddParameter("AdminDB", "@Balance", c.Balance);
            return Database.InsertData(Database.GetConnection("KlantDB"), sql, par1, par2, par3, par4);
        }

        public static void UpdateCustomer(Customer c, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Customers SET CustomerName=@CustomerName, Address=@Address, Picture=@Picture, Balance=@Balance WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@CustomerName", c.CustomerName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Address", c.Address);
            DbParameter par3 = Database.AddParameter("AdminDB", "@Picture", c.Picture);
            DbParameter par4 = Database.AddParameter("AdminDB", "@Balance", c.Balance);
            DbParameter par5 = Database.AddParameter("AdminDB", "@ID", c.ID);
            Database.ModifyData(Database.GetConnection("KlantDB"), sql, par1, par2, par3, par4, par5);
        }

        public static void DeleteCustomer(int id, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Customers WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", id);
            DbConnection con = Database.GetConnection("KlantDB");
            Database.ModifyData(con, sql, par1);
        }

        public static Customer GetCustomerByName(string name)
        {
            Customer c = new Customer();

            string sql = "SELECT * FROM Customers WHERE CustomerName like @CustomerName";
            DbParameter par1 = Database.AddParameter("AdminDB", "@CustomerName", name);
            DbDataReader reader = Database.GetData(Database.GetConnection("KlantDB"), sql, par1);

            while (reader.Read())
            {
                c.ID = Convert.ToInt32(reader["ID"]);
                c.CustomerName = reader["CustomerName"].ToString();
                c.Address = reader["Address"].ToString();
                if (!DBNull.Value.Equals(reader["Picture"]))
                    c.Picture = (byte[])reader["Picture"];
                else
                    c.Picture = new byte[0];
                c.Balance = Double.Parse(reader["Balance"].ToString());
            }

            return c;
        }

        public static Customer GetCustomerById(int id)
        {
            Customer c = new Customer();

            string sql = "SELECT * FROM Customers WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("KlantDB", "@ID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection("KlantDB"), sql, par1);

            while (reader.Read())
            {
                c.ID = Convert.ToInt32(reader["ID"]);
                c.CustomerName = reader["CustomerName"].ToString();
                c.Address = reader["Address"].ToString();
                if (!DBNull.Value.Equals(reader["Picture"]))
                    c.Picture = (byte[])reader["Picture"];
                else
                    c.Picture = new byte[0];
                c.Balance = Double.Parse(reader["Balance"].ToString());
            }

            return c;
        }
    }
}