using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables
{

    public class AddressAccess
    {
        #region Default Methods
        public static Infrastructure.Data.Entities.Tables.AddressEntity Get(long addressid)
        {
            var dataTable = new DataTable();
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Address] WHERE [AddressId]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", addressid); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.AddressEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Infrastructure.Data.Entities.Tables.AddressEntity> Get()
        {  
            var dataTable = new DataTable();     
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Address]";
                var sqlCommand = new SqlCommand(query, sqlConnection); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.AddressEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.AddressEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.AddressEntity> Get(List<long> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<Infrastructure.Data.Entities.Tables.AddressEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.AddressEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.AddressEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.AddressEntity> get(List<long> ids)
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

                    sqlCommand.CommandText = $"SELECT * FROM [Address] WHERE [AddressId] IN ({queryIds})";                    
                new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.AddressEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.AddressEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.AddressEntity>();
        }

        public static long Insert(Infrastructure.Data.Entities.Tables.AddressEntity item)
        {
            long response = long.MinValue;
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();

                string query = "INSERT INTO [Address] ([AddressLine1],[AddressLine2],[City],[Country],[CustomerId],[PhoneNumber],[PostalCode],[RecipientName],[StateProvince])  VALUES (@AddressLine1,@AddressLine2,@City,@Country,@CustomerId,@PhoneNumber,@PostalCode,@RecipientName,@StateProvince); ";
                query += "SELECT SCOPE_IDENTITY();";

                using (var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AddressLine1",item.AddressLine1);
					sqlCommand.Parameters.AddWithValue("AddressLine2",item.AddressLine2 == null ? (object)DBNull.Value  : item.AddressLine2);
					sqlCommand.Parameters.AddWithValue("City",item.City);
					sqlCommand.Parameters.AddWithValue("Country",item.Country);
					sqlCommand.Parameters.AddWithValue("CustomerId",item.CustomerId);
					sqlCommand.Parameters.AddWithValue("PhoneNumber",item.PhoneNumber == null ? (object)DBNull.Value  : item.PhoneNumber);
					sqlCommand.Parameters.AddWithValue("PostalCode",item.PostalCode);
					sqlCommand.Parameters.AddWithValue("RecipientName",item.RecipientName);
					sqlCommand.Parameters.AddWithValue("StateProvince",item.StateProvince == null ? (object)DBNull.Value  : item.StateProvince);

                    var result = sqlCommand.ExecuteScalar();
                    response = result == null? long.MinValue:  long.TryParse(result.ToString(), out var insertedId) ? insertedId : long.MinValue;
                }
                sqlTransaction.Commit();

                return response;
            }
        }
        public static int Insert(List<Infrastructure.Data.Entities.Tables.AddressEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
        private static int insert(List<Infrastructure.Data.Entities.Tables.AddressEntity> items)
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
                        query += " INSERT INTO [Address] ([AddressLine1],[AddressLine2],[City],[Country],[CustomerId],[PhoneNumber],[PostalCode],[RecipientName],[StateProvince]) VALUES ( "

							+ "@AddressLine1"+ i +","
							+ "@AddressLine2"+ i +","
							+ "@City"+ i +","
							+ "@Country"+ i +","
							+ "@CustomerId"+ i +","
							+ "@PhoneNumber"+ i +","
							+ "@PostalCode"+ i +","
							+ "@RecipientName"+ i +","
							+ "@StateProvince"+ i 
                            + "); ";

                            
							sqlCommand.Parameters.AddWithValue("AddressLine1" + i, item.AddressLine1);
							sqlCommand.Parameters.AddWithValue("AddressLine2" + i, item.AddressLine2 == null ? (object)DBNull.Value  : item.AddressLine2);
							sqlCommand.Parameters.AddWithValue("City" + i, item.City);
							sqlCommand.Parameters.AddWithValue("Country" + i, item.Country);
							sqlCommand.Parameters.AddWithValue("CustomerId" + i, item.CustomerId);
							sqlCommand.Parameters.AddWithValue("PhoneNumber" + i, item.PhoneNumber == null ? (object)DBNull.Value  : item.PhoneNumber);
							sqlCommand.Parameters.AddWithValue("PostalCode" + i, item.PostalCode);
							sqlCommand.Parameters.AddWithValue("RecipientName" + i, item.RecipientName);
							sqlCommand.Parameters.AddWithValue("StateProvince" + i, item.StateProvince == null ? (object)DBNull.Value  : item.StateProvince);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Update(Infrastructure.Data.Entities.Tables.AddressEntity item)
        {   
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "UPDATE [Address] SET [AddressLine1]=@AddressLine1, [AddressLine2]=@AddressLine2, [City]=@City, [Country]=@Country, [CustomerId]=@CustomerId, [PhoneNumber]=@PhoneNumber, [PostalCode]=@PostalCode, [RecipientName]=@RecipientName, [StateProvince]=@StateProvince WHERE [AddressId]=@AddressId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                    
                sqlCommand.Parameters.AddWithValue("AddressId", item.AddressId);
				sqlCommand.Parameters.AddWithValue("AddressLine1",item.AddressLine1);
				sqlCommand.Parameters.AddWithValue("AddressLine2",item.AddressLine2 == null ? (object)DBNull.Value  : item.AddressLine2);
				sqlCommand.Parameters.AddWithValue("City",item.City);
				sqlCommand.Parameters.AddWithValue("Country",item.Country);
				sqlCommand.Parameters.AddWithValue("CustomerId",item.CustomerId);
				sqlCommand.Parameters.AddWithValue("PhoneNumber",item.PhoneNumber == null ? (object)DBNull.Value  : item.PhoneNumber);
				sqlCommand.Parameters.AddWithValue("PostalCode",item.PostalCode);
				sqlCommand.Parameters.AddWithValue("RecipientName",item.RecipientName);
				sqlCommand.Parameters.AddWithValue("StateProvince",item.StateProvince == null ? (object)DBNull.Value  : item.StateProvince);
                        
                results = sqlCommand.ExecuteNonQuery();
            }
                
            return results;
        }
        public static int Update(List<Infrastructure.Data.Entities.Tables.AddressEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
        private static int update(List<Infrastructure.Data.Entities.Tables.AddressEntity> items)
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
                        query += " UPDATE [Address] SET "

							+ "[AddressLine1]=@AddressLine1"+ i +","
							+ "[AddressLine2]=@AddressLine2"+ i +","
							+ "[City]=@City"+ i +","
							+ "[Country]=@Country"+ i +","
							+ "[CustomerId]=@CustomerId"+ i +","
							+ "[PhoneNumber]=@PhoneNumber"+ i +","
							+ "[PostalCode]=@PostalCode"+ i +","
							+ "[RecipientName]=@RecipientName"+ i +","
							+ "[StateProvince]=@StateProvince"+ i +" WHERE [AddressId]=@AddressId" + i 
                            + "; ";

                            sqlCommand.Parameters.AddWithValue("AddressId" + i, item.AddressId);
							sqlCommand.Parameters.AddWithValue("AddressLine1" + i, item.AddressLine1);
							sqlCommand.Parameters.AddWithValue("AddressLine2" + i, item.AddressLine2 == null ? (object)DBNull.Value  : item.AddressLine2);
							sqlCommand.Parameters.AddWithValue("City" + i, item.City);
							sqlCommand.Parameters.AddWithValue("Country" + i, item.Country);
							sqlCommand.Parameters.AddWithValue("CustomerId" + i, item.CustomerId);
							sqlCommand.Parameters.AddWithValue("PhoneNumber" + i, item.PhoneNumber == null ? (object)DBNull.Value  : item.PhoneNumber);
							sqlCommand.Parameters.AddWithValue("PostalCode" + i, item.PostalCode);
							sqlCommand.Parameters.AddWithValue("RecipientName" + i, item.RecipientName);
							sqlCommand.Parameters.AddWithValue("StateProvince" + i, item.StateProvince == null ? (object)DBNull.Value  : item.StateProvince);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Delete(long addressid)
        {
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "DELETE FROM [Address] WHERE [AddressId]=@AddressId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("AddressId", addressid);

                results = sqlCommand.ExecuteNonQuery();
            }

            return results;
        }
        public static int Delete(List<long> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxParamsNumber =  Settings.MAX_BATCH_SIZE; 
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

                    string query = "DELETE FROM [Address] WHERE [AddressId] IN ("+ queryIds +")";                    
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
