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
    public class EmployeeDA
    {
        // Gebruik de claims niet omdat ik voortdurend probleem heb om dynamisch de connectionstring te veranderen

        /*private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"0x0df01d4b-PC", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }*/

        public static List<Employee> GetEmployees(IEnumerable<Claim> claims)
        {
            List<Employee> list = new List<Employee>();
            string sql = "SELECT * FROM Employee";
            DbDataReader reader = Database.GetData(Database.GetConnection("KlantDB"), sql);
            while (reader.Read())
            {
                Employee e = new Employee();
                e.ID = Convert.ToInt32(reader["ID"]);
                e.EmployeeName = reader["EmployeeName"].ToString();
                e.Address = reader["Address"].ToString();
                e.Email = reader["Email"].ToString();
                e.Phone = reader["Phone"].ToString();

                list.Add(e);
            }

            return list;
        }

        public static int InsertEmployee(Employee e, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Employee VALUES(@EmployeeName,@Address,@Email,@Phone)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@EmployeeName", e.EmployeeName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Address", e.Address);
            DbParameter par3 = Database.AddParameter("AdminDB", "@Email", e.Email);
            DbParameter par4 = Database.AddParameter("AdminDB", "@Phone", e.Phone);
            return Database.InsertData(Database.GetConnection("KlantDB"), sql, par1, par2, par3, par4);
        }

        public static void UpdateEmployee(Employee e, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Employee SET EmployeeName=@EmployeeName, Address=@Address, Email=@Email, Phone=@Phone WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@EmployeeName", e.EmployeeName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Address", e.Address);
            DbParameter par3 = Database.AddParameter("AdminDB", "@Email", e.Email);
            DbParameter par4 = Database.AddParameter("AdminDB", "@Phone", e.Phone);
            DbParameter par5 = Database.AddParameter("AdminDB", "@ID", e.ID);
            Database.ModifyData(Database.GetConnection("KlantDB"), sql, par1, par2, par3, par4, par5);
        }

        // Relaties !!!! Register_Employee
        public static void DeleteEmployee(int id, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Employee WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", id);
            DbConnection con = Database.GetConnection("KlantDB");
            Database.ModifyData(con, sql, par1);
        }

        public static List<Employee> GetEmployeesByRegister(int rId, IEnumerable<Claim> claims)
        {
            List<Employee> list = new List<Employee>();
            List<int> employeeIDs = new List<int>();
            
            string sql = "SELECT EmployeeID FROM Register_Employee WHERE RegisterID=@RegisterID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterID", rId);
            DbDataReader reader = Database.GetData(Database.GetConnection("KlantDB"), sql, par1);
            while (reader.Read())
            {
                int employeeID = Convert.ToInt32(reader["EmployeeID"]);

                employeeIDs.Add(employeeID);
            }

            if (employeeIDs.Count > 0)
            {
                sql = "SELECT * FROM Employee WHERE ID=@ID";

                foreach (int id in employeeIDs)
                {
                    par1 = Database.AddParameter("AdminDB", "@ID", id);
                    reader = Database.GetData(Database.GetConnection("KlantDB"), sql, par1);

                    while (reader.Read())
                    {
                        Employee e = new Employee();
                        e.ID = Convert.ToInt32(reader["ID"]);
                        e.EmployeeName = reader["EmployeeName"].ToString();
                        e.Address = reader["Address"].ToString();
                        e.Email = reader["Email"].ToString();
                        e.Phone = reader["Phone"].ToString();

                        list.Add(e);
                    }
                }
            }

            return list;
        }

        public static Employee GetEmployeeById(int id)
        {
            Employee e = new Employee();

            string sql = "SELECT * FROM Employee WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection("KlantDB"), sql, par1);

            while (reader.Read())
            {
                e.ID = Convert.ToInt32(reader["ID"]);
                e.EmployeeName = reader["EmployeeName"].ToString();
                e.Address = reader["Address"].ToString();
                e.Email = reader["Email"].ToString();
                e.Phone = reader["Phone"].ToString();
            }

            return e;
        }
    }
}