using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.Helpers
{
    public class Database
    {
        private static DbConnection GetConnection(string ConnectionString)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[ConnectionString];
            DbConnection con = DbProviderFactories.GetFactory(settings.ProviderName).CreateConnection();
            con.ConnectionString = settings.ConnectionString;
            con.Open();

            return con;
        }

        public static void ReleaseConnection(DbConnection con)
        {
            if (con != null)
            {
                con.Close();
                con = null;
            }
        }

        private static DbCommand BuildCommand(string ConnectionString, string sql, params DbParameter[] parameters)
        {
            DbCommand command = GetConnection(ConnectionString).CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sql;

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            return command;
        }

        public static DbDataReader GetData(string ConnectionString, string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            DbDataReader reader = null;

            try
            {
                command = BuildCommand(ConnectionString, sql, parameters);
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (reader != null)
                    reader.Close();
                if (command != null)
                    ReleaseConnection(command.Connection);
                throw;
            }
        }

        public static int ModifyData(string ConnectionString, string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            try
            {
                command = BuildCommand(ConnectionString, sql, parameters);
                int affected = command.ExecuteNonQuery();
                command.Connection.Close();

                return affected;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (command != null)
                    ReleaseConnection(command.Connection);
                return 0;
            }
        }

        public static int InsertData(string ConnectionString, string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            try
            {
                command = BuildCommand(ConnectionString, sql, parameters);
                command.ExecuteNonQuery();

                command.Parameters.Clear();
                command.CommandText = "SELECT @@IDENTITY";

                int identity = Convert.ToInt32(command.ExecuteScalar());
                command.Connection.Close();

                return identity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (command != null)
                    ReleaseConnection(command.Connection);
                return 0;
            }
        }


        public static DbParameter AddParameter(string ConnectionString, string name, object value)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[ConnectionString];
            DbParameter par = DbProviderFactories.GetFactory(settings.ProviderName).CreateParameter();
            par.ParameterName = name;
            par.Value = value;

            return par;
        }

        public static DbTransaction BeginTransaction(string ConnectionString)
        {
            DbConnection con = null;
            try
            {
                con = GetConnection(ConnectionString);
                return con.BeginTransaction();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ReleaseConnection(con);
                throw;
            }
        }

        private static DbCommand BuildCommand(DbTransaction trans, string sql, params DbParameter[] parameters)
        {
            DbCommand command = trans.Connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sql;

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            return command;
        }

        public static DbDataReader GetData(DbTransaction trans, string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            DbDataReader reader = null;
            try
            {
                command = BuildCommand(trans, sql, parameters);
                command.Transaction = trans;
                reader = command.ExecuteReader();

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static int ModifyData(DbTransaction trans, string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            try
            {
                command = BuildCommand(trans, sql, parameters);
                command.Transaction = trans;
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static int InsertData(DbTransaction trans, string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            try
            {
                command = BuildCommand(trans, sql, parameters);
                command.Transaction = trans;
                command.ExecuteNonQuery();

                command.Parameters.Clear();
                command.CommandText = "SELECT @@IDENTITY";

                int identity = Convert.ToInt32(command.ExecuteScalar());
                return identity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}