using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables
{

    public class ProductAccess
    {
        #region Default Methods
        public static Infrastructure.Data.Entities.Tables.ProductEntity Get(long productid)
        {
            var dataTable = new DataTable();
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Product] WHERE [ProductId]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", productid); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.ProductEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Infrastructure.Data.Entities.Tables.ProductEntity> Get()
        {  
            var dataTable = new DataTable();     
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Product]";
                var sqlCommand = new SqlCommand(query, sqlConnection); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.ProductEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.ProductEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.ProductEntity> Get(List<long> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<Infrastructure.Data.Entities.Tables.ProductEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.ProductEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.ProductEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.ProductEntity> get(List<long> ids)
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

                    sqlCommand.CommandText = $"SELECT * FROM [Product] WHERE [ProductId] IN ({queryIds})";                    
                new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.ProductEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.ProductEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.ProductEntity>();
        }

        public static long Insert(Infrastructure.Data.Entities.Tables.ProductEntity item)
        {
            long response = long.MinValue;
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();

                string query = "INSERT INTO [Product] ([BasePrice],[CategoryId],[CreatedAt],[Name],[StoreId],[UpdatedAt])  VALUES (@BasePrice,@CategoryId,@CreatedAt,@Name,@StoreId,@UpdatedAt); ";
                query += "SELECT SCOPE_IDENTITY();";

                using (var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("BasePrice",item.BasePrice);
					sqlCommand.Parameters.AddWithValue("CategoryId",item.CategoryId);
					sqlCommand.Parameters.AddWithValue("CreatedAt",item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
					sqlCommand.Parameters.AddWithValue("Name",item.Name);
					sqlCommand.Parameters.AddWithValue("StoreId",item.StoreId);
					sqlCommand.Parameters.AddWithValue("UpdatedAt",item.UpdatedAt == null ? (object)DBNull.Value  : item.UpdatedAt);

                    var result = sqlCommand.ExecuteScalar();
                    response = result == null? long.MinValue:  long.TryParse(result.ToString(), out var insertedId) ? insertedId : long.MinValue;
                }
                sqlTransaction.Commit();

                return response;
            }
        }
        public static int Insert(List<Infrastructure.Data.Entities.Tables.ProductEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
        private static int insert(List<Infrastructure.Data.Entities.Tables.ProductEntity> items)
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
                        query += " INSERT INTO [Product] ([BasePrice],[CategoryId],[CreatedAt],[Name],[StoreId],[UpdatedAt]) VALUES ( "

							+ "@BasePrice"+ i +","
							+ "@CategoryId"+ i +","
							+ "@CreatedAt"+ i +","
							+ "@Name"+ i +","
							+ "@StoreId"+ i +","
							+ "@UpdatedAt"+ i 
                            + "); ";

                            
							sqlCommand.Parameters.AddWithValue("BasePrice" + i, item.BasePrice);
							sqlCommand.Parameters.AddWithValue("CategoryId" + i, item.CategoryId);
							sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
							sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
							sqlCommand.Parameters.AddWithValue("StoreId" + i, item.StoreId);
							sqlCommand.Parameters.AddWithValue("UpdatedAt" + i, item.UpdatedAt == null ? (object)DBNull.Value  : item.UpdatedAt);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Update(Infrastructure.Data.Entities.Tables.ProductEntity item)
        {   
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "UPDATE [Product] SET [BasePrice]=@BasePrice, [CategoryId]=@CategoryId, [CreatedAt]=@CreatedAt, [Name]=@Name, [StoreId]=@StoreId, [UpdatedAt]=@UpdatedAt WHERE [ProductId]=@ProductId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                    
                sqlCommand.Parameters.AddWithValue("ProductId", item.ProductId);
				sqlCommand.Parameters.AddWithValue("BasePrice",item.BasePrice);
				sqlCommand.Parameters.AddWithValue("CategoryId",item.CategoryId);
				sqlCommand.Parameters.AddWithValue("CreatedAt",item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
				sqlCommand.Parameters.AddWithValue("Name",item.Name);
				sqlCommand.Parameters.AddWithValue("StoreId",item.StoreId);
				sqlCommand.Parameters.AddWithValue("UpdatedAt",item.UpdatedAt == null ? (object)DBNull.Value  : item.UpdatedAt);
                        
                results = sqlCommand.ExecuteNonQuery();
            }
                
            return results;
        }
        public static int Update(List<Infrastructure.Data.Entities.Tables.ProductEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
        private static int update(List<Infrastructure.Data.Entities.Tables.ProductEntity> items)
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
                        query += " UPDATE [Product] SET "

							+ "[BasePrice]=@BasePrice"+ i +","
							+ "[CategoryId]=@CategoryId"+ i +","
							+ "[CreatedAt]=@CreatedAt"+ i +","
							+ "[Name]=@Name"+ i +","
							+ "[StoreId]=@StoreId"+ i +","
							+ "[UpdatedAt]=@UpdatedAt"+ i +" WHERE [ProductId]=@ProductId" + i 
                            + "; ";

                            sqlCommand.Parameters.AddWithValue("ProductId" + i, item.ProductId);
							sqlCommand.Parameters.AddWithValue("BasePrice" + i, item.BasePrice);
							sqlCommand.Parameters.AddWithValue("CategoryId" + i, item.CategoryId);
							sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
							sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
							sqlCommand.Parameters.AddWithValue("StoreId" + i, item.StoreId);
							sqlCommand.Parameters.AddWithValue("UpdatedAt" + i, item.UpdatedAt == null ? (object)DBNull.Value  : item.UpdatedAt);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Delete(long productid)
        {
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "DELETE FROM [Product] WHERE [ProductId]=@ProductId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("ProductId", productid);

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

                    string query = "DELETE FROM [Product] WHERE [ProductId] IN ("+ queryIds +")";                    
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
