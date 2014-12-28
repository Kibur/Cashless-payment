using nmct.ba.cashlessproject.api.Helpers;
using nmct.ba.cashlessproject.model.IT_bedrijf;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class OrganisationDA
    {
        public static Organisation CheckCredentials(string username, string password)
        {
            string sql = "SELECT * FROM Organisation WHERE Login=@Login AND Password=@Password";
            DbParameter par1 = Database.AddParameter("AdminDB", "@Login", Cryptography.Encrypt(username));
            DbParameter par2 = Database.AddParameter("AdminDB", "@Password", Cryptography.Encrypt(password));
            try
            {
                DbDataReader reader = Database.GetData(Database.GetConnection("AdminDB"), sql, par1, par2);
                reader.Read();
                return new Organisation()
                {
                    ID = Int32.Parse(reader["ID"].ToString()),
                    Login = reader["Login"].ToString(),
                    Password = reader["Password"].ToString(),
                    DbName = reader["DbName"].ToString(),
                    DbLogin = reader["DbLogin"].ToString(),
                    DbPassword = reader["DbPassword"].ToString(),
                    OrganisationName = reader["OrganisationName"].ToString(),
                    Address = reader["Address"].ToString(),
                    Email = reader["Email"].ToString(),
                    Phone = reader["Phone"].ToString()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public static bool CheckAccount(string username, string password)
        {
            bool result = false;

            string sql = "SELECT Login, Password FROM Organisation WHERE Login=@Login AND Password=@Password";
            DbParameter par1 = Database.AddParameter("AdminDB", "@Login", username);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Password", password);

            try
            {
                DbDataReader reader = Database.GetData(Database.GetConnection("AdminDB"), sql, par1, par2);
                reader.Read();

                string wachtwoord = reader["Password"].ToString();

                if (wachtwoord.Equals(password))
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }

            return result;
        }

        private static int GetOrganisationID()
        {
            string sql = "SELECT ID FROM Organisation";
            DbDataReader reader = Database.GetData(Database.GetConnection("AdminDB"), sql);
            reader.Read();

            return Convert.ToInt32(reader["ID"]);
        }

        public static void UpdateOrganisation(Organisation o, IEnumerable<Claim> claims)
        {
            int id = GetOrganisationID();

            string sql = "UPDATE Organisation SET Login=@Login, Password=@Password, DbLogin=@DbLogin, DbPassword=@DbPassword";
            sql += " WHERE ID=@ID";

            DbParameter par1 = Database.AddParameter("AdminDB", "@Login", o.Login);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Password", o.Password);
            DbParameter par3 = Database.AddParameter("AdminDB", "@DbLogin", o.DbLogin);
            DbParameter par4 = Database.AddParameter("AdminDB", "@DbPassword", o.DbPassword);
            DbParameter par5 = Database.AddParameter("AdminDB", "@ID", id);
            Database.ModifyData(Database.GetConnection("AdminDB"), sql, par1, par2, par3, par4, par5);
        }
    }
}