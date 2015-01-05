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
    public class RegisterDA
    {
        // Gebruik de claims niet omdat ik voortdurend probleem heb om dynamisch de connectionstring te veranderen

        /*private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"0x0df01d4b-PC", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }*/

        public static List<Register> GetRegisters(IEnumerable<Claim> claims)
        {
            List<Register> list = new List<Register>();
            string sql = "SELECT * FROM Registers";
            DbDataReader reader = Database.GetData(Database.GetConnection("KlantDB"), sql);
            while (reader.Read())
            {
                Register r = new Register();
                r.ID = Convert.ToInt32(reader["ID"]);
                r.RegisterName = reader["RegisterName"].ToString();
                r.Device = reader["Device"].ToString();

                list.Add(r);
            }

            return list;
        }

        public static int InsertRegister(Register r, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Registers VALUES(@RegisterName,@Device)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterName", r.RegisterName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Device", r.Device);
            return Database.InsertData(Database.GetConnection("KlantDB"), sql, par1, par2);
        }

        public static void UpdateRegister(Register r, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Products SET RegisterName=@RegisterName, Device=@Device WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterName", r.RegisterName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Device", r.Device);
            DbParameter par3 = Database.AddParameter("AdminDB", "@ID", r.ID);
            Database.ModifyData(Database.GetConnection("KlantDB"), sql, par1, par2, par3);
        }

        // Relaties !!!! Register_Employee
        public static void DeleteRegister(int id, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Registers WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", id);
            DbConnection con = Database.GetConnection("KlantDB");
            Database.ModifyData(con, sql, par1);
        }

        public static Register GetRegisterById(int id, IEnumerable<Claim> claims)
        {
            Register r = new Register();
            string sql = "SELECT * FROM Registers WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection("KlantDB"), sql, par1);

            while (reader.Read())
            {
                r.ID = Convert.ToInt32(reader["ID"]);
                r.RegisterName = reader["RegisterName"].ToString();
                r.Device = reader["Device"].ToString();
            }

            return r;
        }

        public static Register GetRegisterByEmployee(int id)
        {
            int rID = 0;
            string sql = "SELECT RegisterID FROM Register_Employee WHERE EmployeeID=@EmployeeID";
            DbParameter par1 = Database.AddParameter("KlantDB", "@EmployeeID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection("KlantDB"), sql, par1);

            reader.Read();

            try
            {
                rID = Convert.ToInt32(reader["RegisterID"]);

                sql = "SELECT * FROM Registers WHERE ID=@ID";
                par1 = Database.AddParameter("KlantDB", "@ID", rID);
                reader = Database.GetData(Database.GetConnection("KlantDB"), sql, par1);

                Register r = new Register();

                while (reader.Read())
                {
                    r.ID = rID;
                    r.RegisterName = reader["RegisterName"].ToString();
                    r.Device = reader["Device"].ToString();
                }

                return r;
            }
            catch (InvalidOperationException)
            {
                // Gebeurt wanneer een employee geen toegewezen kassa heeft.
            }

            return null;
        }
    }
}