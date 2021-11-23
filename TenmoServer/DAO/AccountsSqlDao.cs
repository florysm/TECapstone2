using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountsSqlDao : IAccountsDao
    {
        private readonly string connectionString;

        public AccountsSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }


        public Accounts GetAccount (int id)
        {
            Accounts returnAccount = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("select * from accounts where user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        returnAccount = GetAccountFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return returnAccount;
        }

        private Accounts GetAccountFromReader(SqlDataReader reader)
        {
            Accounts a = new Accounts()
            {
                AccountID = Convert.ToInt32(reader["account_id"]),
                UserID = Convert.ToInt32(reader["user_id"]),
                Balance = Convert.ToDecimal(reader["balance"])
            };
            return a;
        }

    }
}
