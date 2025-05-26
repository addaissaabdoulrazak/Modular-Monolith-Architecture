using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure.Data.Access.Tables.COR
{

    public class UserRolesAccess
    {
        #region Default Methods
        public static Infrastructure.Data.Entities.Tables.UserRolesEntity Get(int id)
        {
            var dataTable = new DataTable();
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [UserRoles] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.UserRolesEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Infrastructure.Data.Entities.Tables.UserRolesEntity> Get()
        {  
            var dataTable = new DataTable();     
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [UserRoles]";
                var sqlCommand = new SqlCommand(query, sqlConnection); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.UserRolesEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.UserRolesEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.UserRolesEntity> Get(List<int> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<Infrastructure.Data.Entities.Tables.UserRolesEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.UserRolesEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.UserRolesEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.UserRolesEntity> get(List<int> ids)
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

                    sqlCommand.CommandText = $"SELECT * FROM [UserRoles] WHERE [Id] IN ({queryIds})";                    
                new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.UserRolesEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.UserRolesEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.UserRolesEntity>();
        }

        public static int Insert(Infrastructure.Data.Entities.Tables.UserRolesEntity item)
        {
            int response = int.MinValue;
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();

                string query = "INSERT INTO [UserRoles] ([CreationTime],[Description],[DisplayName],[IsActive],[Level],[Name])  VALUES (@CreationTime,@Description,@DisplayName,@IsActive,@Level,@Name); ";
                query += "SELECT SCOPE_IDENTITY();";

                using (var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CreationTime",item.CreationTime);
					sqlCommand.Parameters.AddWithValue("Description",item.Description == null ? (object)DBNull.Value  : item.Description);
					sqlCommand.Parameters.AddWithValue("DisplayName",item.DisplayName);
					sqlCommand.Parameters.AddWithValue("IsActive",item.IsActive);
					sqlCommand.Parameters.AddWithValue("Level",item.Level);
					sqlCommand.Parameters.AddWithValue("Name",item.Name);

                    var result = sqlCommand.ExecuteScalar();
                    response = result == null? int.MinValue:  int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
                }
                sqlTransaction.Commit();

                return response;
            }
        }
        public static int Insert(List<Infrastructure.Data.Entities.Tables.UserRolesEntity> items)
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
        private static int insert(List<Infrastructure.Data.Entities.Tables.UserRolesEntity> items)
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
                        query += " INSERT INTO [UserRoles] ([CreationTime],[Description],[DisplayName],[IsActive],[Level],[Name]) VALUES ( "

							+ "@CreationTime"+ i +","
							+ "@Description"+ i +","
							+ "@DisplayName"+ i +","
							+ "@IsActive"+ i +","
							+ "@Level"+ i +","
							+ "@Name"+ i 
                            + "); ";

                            
							sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
							sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value  : item.Description);
							sqlCommand.Parameters.AddWithValue("DisplayName" + i, item.DisplayName);
							sqlCommand.Parameters.AddWithValue("IsActive" + i, item.IsActive);
							sqlCommand.Parameters.AddWithValue("Level" + i, item.Level);
							sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Update(Infrastructure.Data.Entities.Tables.UserRolesEntity item)
        {   
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "UPDATE [UserRoles] SET [CreationTime]=@CreationTime, [Description]=@Description, [DisplayName]=@DisplayName, [IsActive]=@IsActive, [Level]=@Level, [Name]=@Name WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                    
                sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("CreationTime",item.CreationTime);
				sqlCommand.Parameters.AddWithValue("Description",item.Description == null ? (object)DBNull.Value  : item.Description);
				sqlCommand.Parameters.AddWithValue("DisplayName",item.DisplayName);
				sqlCommand.Parameters.AddWithValue("IsActive",item.IsActive);
				sqlCommand.Parameters.AddWithValue("Level",item.Level);
				sqlCommand.Parameters.AddWithValue("Name",item.Name);
                        
                results = sqlCommand.ExecuteNonQuery();
            }
                
            return results;
        }
        public static int Update(List<Infrastructure.Data.Entities.Tables.UserRolesEntity> items)
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
        private static int update(List<Infrastructure.Data.Entities.Tables.UserRolesEntity> items)
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
                        query += " UPDATE [UserRoles] SET "

							+ "[CreationTime]=@CreationTime"+ i +","
							+ "[Description]=@Description"+ i +","
							+ "[DisplayName]=@DisplayName"+ i +","
							+ "[IsActive]=@IsActive"+ i +","
							+ "[Level]=@Level"+ i +","
							+ "[Name]=@Name"+ i +" WHERE [Id]=@Id" + i 
                            + "; ";

                            sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
							sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
							sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value  : item.Description);
							sqlCommand.Parameters.AddWithValue("DisplayName" + i, item.DisplayName);
							sqlCommand.Parameters.AddWithValue("IsActive" + i, item.IsActive);
							sqlCommand.Parameters.AddWithValue("Level" + i, item.Level);
							sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Delete(int id)
        {
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "DELETE FROM [UserRoles] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);

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

                    string query = "DELETE FROM [UserRoles] WHERE [Id] IN ("+ queryIds +")";                    
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
