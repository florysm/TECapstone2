using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;

        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Transfers> ViewAllTransfers(int userId)
        {
            List<Transfers> returnTransfers = new List<Transfers>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    //todo make sure this still works after doing pending transfers
                    SqlCommand cmd = new SqlCommand("select * from transfers join accounts on transfers.account_from = accounts.account_id " +
                        "join users on accounts.user_id = users.user_id where users.user_id = @userID and transfer_type_id = 2;", conn);
                    cmd.Parameters.AddWithValue("@userID", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transfers tt = GetTransfersFromReader(reader);
                        returnTransfers.Add(tt);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return returnTransfers;
        }

        public Transfers CreateNewTransfer(Transfers transfer)
        {   
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))

                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Insert Into transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) " +
                        "values ((select transfer_type_id from transfer_types where transfer_type_id = @transfertype) , (select transfer_status_id from transfer_statuses where transfer_status_id = @transferstatus), (select account_id from accounts where user_id = @accountfrom), (select account_id from accounts where user_id = @accountto), @amount)", conn);
                    cmd.Parameters.AddWithValue("@transfertype", transfer.TransferTypeID);
                    cmd.Parameters.AddWithValue("@transferstatus", transfer.TransferStatusID);
                    cmd.Parameters.AddWithValue("@accountfrom", transfer.AccountFrom);
                    cmd.Parameters.AddWithValue("@accountto", transfer.AccountTo);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            if (transfer.TransferTypeID == 2)
            {
                TransferTo(transfer);
                TransferFrom(transfer);
                return transfer;
            }
            else
            {
                return transfer;
            }
        }

        public Transfers ApproveTransfer(Transfers transfer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("update transfers set transfer_status_id = 2 where transfer_id = @transferID", conn);
                    cmd.Parameters.AddWithValue("@transferID", transfer.TransferID);
                    cmd.ExecuteNonQuery();
                    TransferTo(transfer);
                    TransferFrom(transfer);
                    return transfer;
                }
            }
            catch (SqlException)
            {
                throw;
            }
            
        }

        public Transfers TransferTo(Transfers transfer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("update accounts set balance = (balance + @amount) where account_id = @account_to", conn);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);
                    cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                    cmd.ExecuteNonQuery();
                    //CreateNewTransfer 
                    //If statement amount can't less then balance 
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return transfer;
        }

        public Transfers TransferFrom(Transfers transfer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("update accounts set balance = (balance - @amount) where account_id = (select account_id from accounts where user_id = @accountfrom)", conn);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);                   
                    cmd.Parameters.AddWithValue("@accountfrom", transfer.AccountFrom);
                    cmd.ExecuteNonQuery();
                    //CreateNewTransfer 
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return transfer;
        }


            public Transfers FindTransferByID(int transferId)
            {
                Transfers returnTransfer = null;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("select *  from transfers where transfer_id = @transferid", conn);
                    // maybe use this to get username insetad of accountid for from and to
                    // select *  from transfers join accounts on transfers.account_to = accounts.account_id join users on accounts.user_id = users.user_id where transfer_id = @transferid
                    cmd.Parameters.AddWithValue("@transferid", transferId);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            returnTransfer = GetTransfersFromReader(reader);
                        }
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                return returnTransfer;
            }
        public List<Transfers> ViewPendingTransfers(int userId)
        {
            List<Transfers> returnTransfers = new List<Transfers>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("select * from transfers join accounts on transfers.account_from = accounts.account_id " +
                        "join users on accounts.user_id = users.user_id where users.user_id = @userID and transfers.transfer_status_id = 1", conn);
                    cmd.Parameters.AddWithValue("@userID", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transfers tt = GetTransfersFromReader(reader);
                        returnTransfers.Add(tt);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return returnTransfers;
        }



        private Transfers GetTransfersFromReader(SqlDataReader reader)
            {
                Transfers t = new Transfers()
                {
                    TransferID = Convert.ToInt32(reader["transfer_id"]),
                    TransferTypeID = Convert.ToInt32(reader["transfer_type_id"]),
                    TransferStatusID = Convert.ToInt32(reader["transfer_status_id"]),
                    AccountFrom = Convert.ToInt32(reader["account_from"]),
                    AccountTo = Convert.ToInt32(reader["account_to"]),
                    Amount = Convert.ToDecimal(reader["amount"])

                };
                return t;
            }
        }
    }

