using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables
{

    public class CustomerAccess
    {
        #region Default Methods
        public static Infrastructure.Data.Entities.Tables.CustomerEntity Get(long customerid)
        {
            var dataTable = new DataTable();
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Customer] WHERE [CustomerId]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", customerid); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.CustomerEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Infrastructure.Data.Entities.Tables.CustomerEntity> Get()
        {  
            var dataTable = new DataTable();     
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Customer]";
                var sqlCommand = new SqlCommand(query, sqlConnection); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CustomerEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.CustomerEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.CustomerEntity> Get(List<long> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<Infrastructure.Data.Entities.Tables.CustomerEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.CustomerEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.CustomerEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.CustomerEntity> get(List<long> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                var dataTable = new DataTable();
                using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    
                    string queryIds = string.Empty;
                    for(int i=0; i<ids.Count; i++)
                    {
                        queryIds += "@Id" + i + ",";
                        sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
                    }
                    queryIds = queryIds.TrimEnd(',');

                    sqlCommand.CommandText = $"SELECT * FROM [Customer] WHERE [CustomerId] IN ({queryIds})";                    
                new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CustomerEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.CustomerEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.CustomerEntity>();
        }

        public static long Insert(Infrastructure.Data.Entities.Tables.CustomerEntity item)
        {
            long response = long.MinValue;
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();

                string query = "INSERT INTO [Customer] ([CreatedAt],[Email],[FullName],[PhoneNumber])  VALUES (@CreatedAt,@Email,@FullName,@PhoneNumber); ";
                query += "SELECT SCOPE_IDENTITY();";

                using (var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CreatedAt",item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
					sqlCommand.Parameters.AddWithValue("Email",item.Email == null ? (object)DBNull.Value  : item.Email);
					sqlCommand.Parameters.AddWithValue("FullName",item.FullName);
					sqlCommand.Parameters.AddWithValue("PhoneNumber",item.PhoneNumber);

                    var result = sqlCommand.ExecuteScalar();
                    response = result == null? long.MinValue:  long.TryParse(result.ToString(), out var insertedId) ? insertedId : long.MinValue;
                }
                sqlTransaction.Commit();

                return response;
            }
        }
        public static int Insert(List<Infrastructure.Data.Entities.Tables.CustomerEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; // Nb params per query
                int results=0;
                if(items.Count <= maxParamsNumber)
                {
                    results = insert(items);
                }else
                {
                    int batchNumber = items.Count / maxParamsNumber;
                    for(int i = 0; i < batchNumber; i++)
                    {
                        results += insert(items.GetRange(i * maxParamsNumber, maxParamsNumber));
                    }
                    results += insert(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
                }
                return results;
            }

            return -1;
        }
        private static int insert(List<Infrastructure.Data.Entities.Tables.CustomerEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int results = -1;
                using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "";
                    var sqlCommand = new SqlCommand(query, sqlConnection);

                    int i = 0;
                    foreach (var item in items)
                    {
                        i++;
                        query += " INSERT INTO [Customer] ([CreatedAt],[Email],[FullName],[PhoneNumber]) VALUES ( "

							+ "@CreatedAt"+ i +","
							+ "@Email"+ i +","
							+ "@FullName"+ i +","
							+ "@PhoneNumber"+ i 
                            + "); ";

                            
							sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
							sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value  : item.Email);
							sqlCommand.Parameters.AddWithValue("FullName" + i, item.FullName);
							sqlCommand.Parameters.AddWithValue("PhoneNumber" + i, item.PhoneNumber);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Update(Infrastructure.Data.Entities.Tables.CustomerEntity item)
        {   
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "UPDATE [Customer] SET [CreatedAt]=@CreatedAt, [Email]=@Email, [FullName]=@FullName, [PhoneNumber]=@PhoneNumber WHERE [CustomerId]=@CustomerId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                    
                sqlCommand.Parameters.AddWithValue("CustomerId", item.CustomerId);
				sqlCommand.Parameters.AddWithValue("CreatedAt",item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
				sqlCommand.Parameters.AddWithValue("Email",item.Email == null ? (object)DBNull.Value  : item.Email);
				sqlCommand.Parameters.AddWithValue("FullName",item.FullName);
				sqlCommand.Parameters.AddWithValue("PhoneNumber",item.PhoneNumber);
                        
                results = sqlCommand.ExecuteNonQuery();
            }
                
            return results;
        }
        public static int Update(List<Infrastructure.Data.Entities.Tables.CustomerEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; // Nb params per query
                int results = 0;
                if(items.Count <= maxParamsNumber)
                {
                    results = update(items);
                }else
                {
                    int batchNumber = items.Count / maxParamsNumber;
                    for(int i = 0; i < batchNumber; i++)
                    {
                        results += update(items.GetRange(i * maxParamsNumber, maxParamsNumber));
                    }
                    results += update(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
                }

                return results;
            }

            return -1;
        }
        private static int update(List<Infrastructure.Data.Entities.Tables.CustomerEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int results = -1;
                using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "";
                    var sqlCommand = new SqlCommand(query, sqlConnection);

                    int i = 0;
                    foreach (var item in items)
                    {
                        i++;
                        query += " UPDATE [Customer] SET "

							+ "[CreatedAt]=@CreatedAt"+ i +","
							+ "[Email]=@Email"+ i +","
							+ "[FullName]=@FullName"+ i +","
							+ "[PhoneNumber]=@PhoneNumber"+ i +" WHERE [CustomerId]=@CustomerId" + i 
                            + "; ";

                            sqlCommand.Parameters.AddWithValue("CustomerId" + i, item.CustomerId);
							sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
							sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value  : item.Email);
							sqlCommand.Parameters.AddWithValue("FullName" + i, item.FullName);
							sqlCommand.Parameters.AddWithValue("PhoneNumber" + i, item.PhoneNumber);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Delete(long customerid)
        {
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "DELETE FROM [Customer] WHERE [CustomerId]=@CustomerId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("CustomerId", customerid);

                results = sqlCommand.ExecuteNonQuery();
            }

            return results;
        }
        public static int Delete(List<long> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE; 
                int results=0;
                if(ids.Count <= maxParamsNumber)
                {
                    results = delete(ids);
                } else
                {
                    int batchNumber = ids.Count / maxParamsNumber;
                    for(int i = 0; i < batchNumber; i++)
                    {
                        results += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
                    }
                    results += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
                }
            }
            return -1;
        }
        private static int delete(List<long> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int results = -1;
                using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;

                    string queryIds = string.Empty;
                    for(int i=0; i<ids.Count; i++)
                    {
                        queryIds += "@Id" + i + ",";
                        sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
                    }
                    queryIds = queryIds.TrimEnd(',');

                    string query = "DELETE FROM [Customer] WHERE [CustomerId] IN ("+ queryIds +")";                    
                    sqlCommand.CommandText = query;
                        
                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }
            return -1;
        }
        #endregion

        #region Custom Methods


        
        #endregion
    }
}
