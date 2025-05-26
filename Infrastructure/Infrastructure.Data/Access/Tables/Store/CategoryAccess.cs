using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables
{

    public class CategoryAccess
    {
        #region Default Methods
        public static Infrastructure.Data.Entities.Tables.CategoryEntity Get(int categoryid)
        {
            var dataTable = new DataTable();
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Category] WHERE [CategoryId]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", categoryid); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.CategoryEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Infrastructure.Data.Entities.Tables.CategoryEntity> Get()
        {  
            var dataTable = new DataTable();     
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Category]";
                var sqlCommand = new SqlCommand(query, sqlConnection); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CategoryEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.CategoryEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.CategoryEntity> Get(List<int> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<Infrastructure.Data.Entities.Tables.CategoryEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.CategoryEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.CategoryEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.CategoryEntity> get(List<int> ids)
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

                    sqlCommand.CommandText = $"SELECT * FROM [Category] WHERE [CategoryId] IN ({queryIds})";                    
                new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CategoryEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.CategoryEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.CategoryEntity>();
        }

        public static int Insert(Infrastructure.Data.Entities.Tables.CategoryEntity item)
        {
            int response = int.MinValue;
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();

                string query = "INSERT INTO [Category] ([CategoryRanking],[CreatedAt],[Description],[Name],[ParentCategoryId],[Slug],[Title],[UpdatedAt])  VALUES (@CategoryRanking,@CreatedAt,@Description,@Name,@ParentCategoryId,@Slug,@Title,@UpdatedAt); ";
                query += "SELECT SCOPE_IDENTITY();";

                using (var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CategoryRanking",item.CategoryRanking == null ? (object)DBNull.Value  : item.CategoryRanking);
					sqlCommand.Parameters.AddWithValue("CreatedAt",item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
					sqlCommand.Parameters.AddWithValue("Description",item.Description == null ? (object)DBNull.Value  : item.Description);
					sqlCommand.Parameters.AddWithValue("Name",item.Name);
					sqlCommand.Parameters.AddWithValue("ParentCategoryId",item.ParentCategoryId == null ? (object)DBNull.Value  : item.ParentCategoryId);
					sqlCommand.Parameters.AddWithValue("Slug",item.Slug);
					sqlCommand.Parameters.AddWithValue("Title",item.Title);
					sqlCommand.Parameters.AddWithValue("UpdatedAt",item.UpdatedAt == null ? (object)DBNull.Value  : item.UpdatedAt);

                    var result = sqlCommand.ExecuteScalar();
                    response = result == null? int.MinValue:  int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
                }
                sqlTransaction.Commit();

                return response;
            }
        }
        public static int Insert(List<Infrastructure.Data.Entities.Tables.CategoryEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
        private static int insert(List<Infrastructure.Data.Entities.Tables.CategoryEntity> items)
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
                        query += " INSERT INTO [Category] ([CategoryRanking],[CreatedAt],[Description],[Name],[ParentCategoryId],[Slug],[Title],[UpdatedAt]) VALUES ( "

							+ "@CategoryRanking"+ i +","
							+ "@CreatedAt"+ i +","
							+ "@Description"+ i +","
							+ "@Name"+ i +","
							+ "@ParentCategoryId"+ i +","
							+ "@Slug"+ i +","
							+ "@Title"+ i +","
							+ "@UpdatedAt"+ i 
                            + "); ";

                            
							sqlCommand.Parameters.AddWithValue("CategoryRanking" + i, item.CategoryRanking == null ? (object)DBNull.Value  : item.CategoryRanking);
							sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
							sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value  : item.Description);
							sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
							sqlCommand.Parameters.AddWithValue("ParentCategoryId" + i, item.ParentCategoryId == null ? (object)DBNull.Value  : item.ParentCategoryId);
							sqlCommand.Parameters.AddWithValue("Slug" + i, item.Slug);
							sqlCommand.Parameters.AddWithValue("Title" + i, item.Title);
							sqlCommand.Parameters.AddWithValue("UpdatedAt" + i, item.UpdatedAt == null ? (object)DBNull.Value  : item.UpdatedAt);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Update(Infrastructure.Data.Entities.Tables.CategoryEntity item)
        {   
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "UPDATE [Category] SET [CategoryRanking]=@CategoryRanking, [CreatedAt]=@CreatedAt, [Description]=@Description, [Name]=@Name, [ParentCategoryId]=@ParentCategoryId, [Slug]=@Slug, [Title]=@Title, [UpdatedAt]=@UpdatedAt WHERE [CategoryId]=@CategoryId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                    
                sqlCommand.Parameters.AddWithValue("CategoryId", item.CategoryId);
				sqlCommand.Parameters.AddWithValue("CategoryRanking",item.CategoryRanking == null ? (object)DBNull.Value  : item.CategoryRanking);
				sqlCommand.Parameters.AddWithValue("CreatedAt",item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
				sqlCommand.Parameters.AddWithValue("Description",item.Description == null ? (object)DBNull.Value  : item.Description);
				sqlCommand.Parameters.AddWithValue("Name",item.Name);
				sqlCommand.Parameters.AddWithValue("ParentCategoryId",item.ParentCategoryId == null ? (object)DBNull.Value  : item.ParentCategoryId);
				sqlCommand.Parameters.AddWithValue("Slug",item.Slug);
				sqlCommand.Parameters.AddWithValue("Title",item.Title);
				sqlCommand.Parameters.AddWithValue("UpdatedAt",item.UpdatedAt == null ? (object)DBNull.Value  : item.UpdatedAt);
                        
                results = sqlCommand.ExecuteNonQuery();
            }
                
            return results;
        }
        public static int Update(List<Infrastructure.Data.Entities.Tables.CategoryEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
        private static int update(List<Infrastructure.Data.Entities.Tables.CategoryEntity> items)
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
                        query += " UPDATE [Category] SET "

							+ "[CategoryRanking]=@CategoryRanking"+ i +","
							+ "[CreatedAt]=@CreatedAt"+ i +","
							+ "[Description]=@Description"+ i +","
							+ "[Name]=@Name"+ i +","
							+ "[ParentCategoryId]=@ParentCategoryId"+ i +","
							+ "[Slug]=@Slug"+ i +","
							+ "[Title]=@Title"+ i +","
							+ "[UpdatedAt]=@UpdatedAt"+ i +" WHERE [CategoryId]=@CategoryId" + i 
                            + "; ";

                            sqlCommand.Parameters.AddWithValue("CategoryId" + i, item.CategoryId);
							sqlCommand.Parameters.AddWithValue("CategoryRanking" + i, item.CategoryRanking == null ? (object)DBNull.Value  : item.CategoryRanking);
							sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
							sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value  : item.Description);
							sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
							sqlCommand.Parameters.AddWithValue("ParentCategoryId" + i, item.ParentCategoryId == null ? (object)DBNull.Value  : item.ParentCategoryId);
							sqlCommand.Parameters.AddWithValue("Slug" + i, item.Slug);
							sqlCommand.Parameters.AddWithValue("Title" + i, item.Title);
							sqlCommand.Parameters.AddWithValue("UpdatedAt" + i, item.UpdatedAt == null ? (object)DBNull.Value  : item.UpdatedAt);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Delete(int categoryid)
        {
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "DELETE FROM [Category] WHERE [CategoryId]=@CategoryId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("CategoryId", categoryid);

                results = sqlCommand.ExecuteNonQuery();
            }

            return results;
        }
        public static int Delete(List<int> ids)
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
        private static int delete(List<int> ids)
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

                    string query = "DELETE FROM [Category] WHERE [CategoryId] IN ("+ queryIds +")";                    
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
