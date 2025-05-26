using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service.Utils
{
    public class TransactionsManager
    {
        public enum Database
        {
            Default = 0,
            NexaCorp = 1,
            Test = 2
        }
        public SqlTransaction transaction { get; set; }
        public SqlConnection connection { get; set; }


        public TransactionsManager(Database database = Database.Default) 
        {
            switch (database)
            {
                case Database.NexaCorp:
                    connection = new SqlConnection(Data.Access.Settings.DefaultConnectionString);
                    break;
                case Database.Test:
                    connection = new SqlConnection(Data.Access.Settings.TestConnectionString);
                    break;
                case Database.Default:
                default:
                    connection = new SqlConnection(Data.Access.Settings.DefaultConnectionString);
                    break;
            }
        }
        public void beginTransaction()
        {
            connection.Close();
            connection.Open();
            transaction = connection.BeginTransaction();
            Debug.WriteLine($"transaction started {transaction.ToString()}");
        }
        public bool commit()
        {
            try
            {
                transaction.Commit();
                connection.Close();
                Debug.WriteLine($"transaction committed {transaction.ToString()}");
                return true;
            }
            catch (Exception e)
            {
                Infrastructure.Services.Logging.Logger.Log(e);
                return rollback();
            }
        }
        public bool rollback()
        {
            try
            {
                transaction.Rollback();
                connection.Close();
                return true;
            }
            catch (Exception e)
            {
                Infrastructure.Services.Logging.Logger.Log(e);
                connection.Close();
                return false;
            }
        }
    }
}
