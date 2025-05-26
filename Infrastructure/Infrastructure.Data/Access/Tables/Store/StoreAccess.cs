using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables
{

    public class StoreAccess
    {
        #region Default Methods
        public static Infrastructure.Data.Entities.Tables.StoreEntity Get(long storeid)
        {
            var dataTable = new DataTable();
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Store] WHERE [StoreId]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", storeid); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.StoreEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Infrastructure.Data.Entities.Tables.StoreEntity> Get()
        {  
            var dataTable = new DataTable();     
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Store]";
                var sqlCommand = new SqlCommand(query, sqlConnection); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.StoreEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.StoreEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.StoreEntity> Get(List<long> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<Infrastructure.Data.Entities.Tables.StoreEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.StoreEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.StoreEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.StoreEntity> get(List<long> ids)
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

                    sqlCommand.CommandText = $"SELECT * FROM [Store] WHERE [StoreId] IN ({queryIds})";                    
                new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.StoreEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.StoreEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.StoreEntity>();
        }

        public static long Insert(Infrastructure.Data.Entities.Tables.StoreEntity item)
        {
            long response = long.MinValue;
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();

                string query = "INSERT INTO [Store] ([Address],[CreatedAt],[Description],[IsActive],[Name],[OwnerId],[StoreImageUrl],[StoreLogoUrl],[SubscriptionPlan])  VALUES (@Address,@CreatedAt,@Description,@IsActive,@Name,@OwnerId,@StoreImageUrl,@StoreLogoUrl,@SubscriptionPlan); ";
                query += "SELECT SCOPE_IDENTITY();";

                using (var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Address",item.Address == null ? (object)DBNull.Value  : item.Address);
					sqlCommand.Parameters.AddWithValue("CreatedAt",item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
					sqlCommand.Parameters.AddWithValue("Description",item.Description == null ? (object)DBNull.Value  : item.Description);
					sqlCommand.Parameters.AddWithValue("IsActive",item.IsActive == null ? (object)DBNull.Value  : item.IsActive);
					sqlCommand.Parameters.AddWithValue("Name",item.Name);
					sqlCommand.Parameters.AddWithValue("OwnerId",item.OwnerId);
					sqlCommand.Parameters.AddWithValue("StoreImageUrl",item.StoreImageUrl == null ? (object)DBNull.Value  : item.StoreImageUrl);
					sqlCommand.Parameters.AddWithValue("StoreLogoUrl",item.StoreLogoUrl == null ? (object)DBNull.Value  : item.StoreLogoUrl);
					sqlCommand.Parameters.AddWithValue("SubscriptionPlan",item.SubscriptionPlan);

                    var result = sqlCommand.ExecuteScalar();
                    response = result == null? long.MinValue:  long.TryParse(result.ToString(), out var insertedId) ? insertedId : long.MinValue;
                }
                sqlTransaction.Commit();

                return response;
            }
        }
        public static int Insert(List<Infrastructure.Data.Entities.Tables.StoreEntity> items)
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
        private static int insert(List<Infrastructure.Data.Entities.Tables.StoreEntity> items)
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
                        query += " INSERT INTO [Store] ([Address],[CreatedAt],[Description],[IsActive],[Name],[OwnerId],[StoreImageUrl],[StoreLogoUrl],[SubscriptionPlan]) VALUES ( "

							+ "@Address"+ i +","
							+ "@CreatedAt"+ i +","
							+ "@Description"+ i +","
							+ "@IsActive"+ i +","
							+ "@Name"+ i +","
							+ "@OwnerId"+ i +","
							+ "@StoreImageUrl"+ i +","
							+ "@StoreLogoUrl"+ i +","
							+ "@SubscriptionPlan"+ i 
                            + "); ";

                            
							sqlCommand.Parameters.AddWithValue("Address" + i, item.Address == null ? (object)DBNull.Value  : item.Address);
							sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
							sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value  : item.Description);
							sqlCommand.Parameters.AddWithValue("IsActive" + i, item.IsActive == null ? (object)DBNull.Value  : item.IsActive);
							sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
							sqlCommand.Parameters.AddWithValue("OwnerId" + i, item.OwnerId);
							sqlCommand.Parameters.AddWithValue("StoreImageUrl" + i, item.StoreImageUrl == null ? (object)DBNull.Value  : item.StoreImageUrl);
							sqlCommand.Parameters.AddWithValue("StoreLogoUrl" + i, item.StoreLogoUrl == null ? (object)DBNull.Value  : item.StoreLogoUrl);
							sqlCommand.Parameters.AddWithValue("SubscriptionPlan" + i, item.SubscriptionPlan);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Update(Infrastructure.Data.Entities.Tables.StoreEntity item)
        {   
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "UPDATE [Store] SET [Address]=@Address, [CreatedAt]=@CreatedAt, [Description]=@Description, [IsActive]=@IsActive, [Name]=@Name, [OwnerId]=@OwnerId, [StoreImageUrl]=@StoreImageUrl, [StoreLogoUrl]=@StoreLogoUrl, [SubscriptionPlan]=@SubscriptionPlan WHERE [StoreId]=@StoreId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                    
                sqlCommand.Parameters.AddWithValue("StoreId", item.StoreId);
				sqlCommand.Parameters.AddWithValue("Address",item.Address == null ? (object)DBNull.Value  : item.Address);
				sqlCommand.Parameters.AddWithValue("CreatedAt",item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
				sqlCommand.Parameters.AddWithValue("Description",item.Description == null ? (object)DBNull.Value  : item.Description);
				sqlCommand.Parameters.AddWithValue("IsActive",item.IsActive == null ? (object)DBNull.Value  : item.IsActive);
				sqlCommand.Parameters.AddWithValue("Name",item.Name);
				sqlCommand.Parameters.AddWithValue("OwnerId",item.OwnerId);
				sqlCommand.Parameters.AddWithValue("StoreImageUrl",item.StoreImageUrl == null ? (object)DBNull.Value  : item.StoreImageUrl);
				sqlCommand.Parameters.AddWithValue("StoreLogoUrl",item.StoreLogoUrl == null ? (object)DBNull.Value  : item.StoreLogoUrl);
				sqlCommand.Parameters.AddWithValue("SubscriptionPlan",item.SubscriptionPlan);
                        
                results = sqlCommand.ExecuteNonQuery();
            }
                
            return results;
        }
        public static int Update(List<Infrastructure.Data.Entities.Tables.StoreEntity> items)
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
        private static int update(List<Infrastructure.Data.Entities.Tables.StoreEntity> items)
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
                        query += " UPDATE [Store] SET "

							+ "[Address]=@Address"+ i +","
							+ "[CreatedAt]=@CreatedAt"+ i +","
							+ "[Description]=@Description"+ i +","
							+ "[IsActive]=@IsActive"+ i +","
							+ "[Name]=@Name"+ i +","
							+ "[OwnerId]=@OwnerId"+ i +","
							+ "[StoreImageUrl]=@StoreImageUrl"+ i +","
							+ "[StoreLogoUrl]=@StoreLogoUrl"+ i +","
							+ "[SubscriptionPlan]=@SubscriptionPlan"+ i +" WHERE [StoreId]=@StoreId" + i 
                            + "; ";

                            sqlCommand.Parameters.AddWithValue("StoreId" + i, item.StoreId);
							sqlCommand.Parameters.AddWithValue("Address" + i, item.Address == null ? (object)DBNull.Value  : item.Address);
							sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
							sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value  : item.Description);
							sqlCommand.Parameters.AddWithValue("IsActive" + i, item.IsActive == null ? (object)DBNull.Value  : item.IsActive);
							sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
							sqlCommand.Parameters.AddWithValue("OwnerId" + i, item.OwnerId);
							sqlCommand.Parameters.AddWithValue("StoreImageUrl" + i, item.StoreImageUrl == null ? (object)DBNull.Value  : item.StoreImageUrl);
							sqlCommand.Parameters.AddWithValue("StoreLogoUrl" + i, item.StoreLogoUrl == null ? (object)DBNull.Value  : item.StoreLogoUrl);
							sqlCommand.Parameters.AddWithValue("SubscriptionPlan" + i, item.SubscriptionPlan);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Delete(long storeid)
        {
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "DELETE FROM [Store] WHERE [StoreId]=@StoreId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("StoreId", storeid);

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

                    string query = "DELETE FROM [Store] WHERE [StoreId] IN ("+ queryIds +")";                    
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
