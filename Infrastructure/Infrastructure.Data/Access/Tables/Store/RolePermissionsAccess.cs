using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables
{

    public class RolePermissionsAccess
    {
        #region Default Methods
        public static Infrastructure.Data.Entities.Tables.RolePermissionsEntity Get(int permissionid)
        {
            var dataTable = new DataTable();
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [RolePermissions] WHERE [PermissionId]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", permissionid); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.RolePermissionsEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Infrastructure.Data.Entities.Tables.RolePermissionsEntity> Get()
        {  
            var dataTable = new DataTable();     
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [RolePermissions]";
                var sqlCommand = new SqlCommand(query, sqlConnection); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.RolePermissionsEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.RolePermissionsEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.RolePermissionsEntity> Get(List<int> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<Infrastructure.Data.Entities.Tables.RolePermissionsEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.RolePermissionsEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.RolePermissionsEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.RolePermissionsEntity> get(List<int> ids)
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

                    sqlCommand.CommandText = $"SELECT * FROM [RolePermissions] WHERE [PermissionId] IN ({queryIds})";                    
                new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.RolePermissionsEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.RolePermissionsEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.RolePermissionsEntity>();
        }

        public static int Insert(Infrastructure.Data.Entities.Tables.RolePermissionsEntity item)
        {
            int response = int.MinValue;
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();

                string query = "INSERT INTO [RolePermissions] ([PermissionId],[RoleId])  VALUES (@PermissionId,@RoleId); ";
                query += "SELECT SCOPE_IDENTITY();";

                using (var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("PermissionId",item.PermissionId);
					sqlCommand.Parameters.AddWithValue("RoleId",item.RoleId == null ? (object)DBNull.Value  : item.RoleId);

                    var result = sqlCommand.ExecuteScalar();
                    response = result == null? int.MinValue:  int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
                }
                sqlTransaction.Commit();

                return response;
            }
        }
        public static int Insert(List<Infrastructure.Data.Entities.Tables.RolePermissionsEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 2; // Nb params per query
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
        private static int insert(List<Infrastructure.Data.Entities.Tables.RolePermissionsEntity> items)
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
                        query += " INSERT INTO [RolePermissions] ([PermissionId],[RoleId]) VALUES ( "

							+ "@PermissionId"+ i +","
							+ "@RoleId"+ i 
                            + "); ";

                            
							sqlCommand.Parameters.AddWithValue("PermissionId" + i, item.PermissionId);
							sqlCommand.Parameters.AddWithValue("RoleId" + i, item.RoleId == null ? (object)DBNull.Value  : item.RoleId);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Update(Infrastructure.Data.Entities.Tables.RolePermissionsEntity item)
        {   
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "UPDATE [RolePermissions] SET [RoleId]=@RoleId WHERE [PermissionId]=@PermissionId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                    
                sqlCommand.Parameters.AddWithValue("PermissionId", item.PermissionId);
				sqlCommand.Parameters.AddWithValue("RoleId",item.RoleId == null ? (object)DBNull.Value  : item.RoleId);
                        
                results = sqlCommand.ExecuteNonQuery();
            }
                
            return results;
        }
        public static int Update(List<Infrastructure.Data.Entities.Tables.RolePermissionsEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 2; // Nb params per query
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
        private static int update(List<Infrastructure.Data.Entities.Tables.RolePermissionsEntity> items)
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
                        query += " UPDATE [RolePermissions] SET "

							+ "[RoleId]=@RoleId"+ i +" WHERE [PermissionId]=@PermissionId" + i 
                            + "; ";

                            sqlCommand.Parameters.AddWithValue("PermissionId" + i, item.PermissionId);
							sqlCommand.Parameters.AddWithValue("RoleId" + i, item.RoleId == null ? (object)DBNull.Value  : item.RoleId);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Delete(int permissionid)
        {
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "DELETE FROM [RolePermissions] WHERE [PermissionId]=@PermissionId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("PermissionId", permissionid);

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

                    string query = "DELETE FROM [RolePermissions] WHERE [PermissionId] IN ("+ queryIds +")";                    
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
