using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables
{

    public class DeliveryServiceAccess
    {
        #region Default Methods
        public static Infrastructure.Data.Entities.Tables.DeliveryServiceEntity Get(int serviceid)
        {
            var dataTable = new DataTable();
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [DeliveryService] WHERE [ServiceId]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", serviceid); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.DeliveryServiceEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Infrastructure.Data.Entities.Tables.DeliveryServiceEntity> Get()
        {  
            var dataTable = new DataTable();     
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [DeliveryService]";
                var sqlCommand = new SqlCommand(query, sqlConnection); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.DeliveryServiceEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.DeliveryServiceEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.DeliveryServiceEntity> Get(List<int> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<Infrastructure.Data.Entities.Tables.DeliveryServiceEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.DeliveryServiceEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.DeliveryServiceEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.DeliveryServiceEntity> get(List<int> ids)
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

                    sqlCommand.CommandText = $"SELECT * FROM [DeliveryService] WHERE [ServiceId] IN ({queryIds})";                    
                new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.DeliveryServiceEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.DeliveryServiceEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.DeliveryServiceEntity>();
        }

        public static int Insert(Infrastructure.Data.Entities.Tables.DeliveryServiceEntity item)
        {
            int response = int.MinValue;
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();

                string query = "INSERT INTO [DeliveryService] ([Contact],[CoverageAreas],[CreatedAt],[DeliveryFee],[IsActive],[Name])  VALUES (@Contact,@CoverageAreas,@CreatedAt,@DeliveryFee,@IsActive,@Name); ";
                query += "SELECT SCOPE_IDENTITY();";

                using (var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Contact",item.Contact);
					sqlCommand.Parameters.AddWithValue("CoverageAreas",item.CoverageAreas == null ? (object)DBNull.Value  : item.CoverageAreas);
					sqlCommand.Parameters.AddWithValue("CreatedAt",item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
					sqlCommand.Parameters.AddWithValue("DeliveryFee",item.DeliveryFee);
					sqlCommand.Parameters.AddWithValue("IsActive",item.IsActive == null ? (object)DBNull.Value  : item.IsActive);
					sqlCommand.Parameters.AddWithValue("Name",item.Name);

                    var result = sqlCommand.ExecuteScalar();
                    response = result == null? int.MinValue:  int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
                }
                sqlTransaction.Commit();

                return response;
            }
        }
        public static int Insert(List<Infrastructure.Data.Entities.Tables.DeliveryServiceEntity> items)
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
        private static int insert(List<Infrastructure.Data.Entities.Tables.DeliveryServiceEntity> items)
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
                        query += " INSERT INTO [DeliveryService] ([Contact],[CoverageAreas],[CreatedAt],[DeliveryFee],[IsActive],[Name]) VALUES ( "

							+ "@Contact"+ i +","
							+ "@CoverageAreas"+ i +","
							+ "@CreatedAt"+ i +","
							+ "@DeliveryFee"+ i +","
							+ "@IsActive"+ i +","
							+ "@Name"+ i 
                            + "); ";

                            
							sqlCommand.Parameters.AddWithValue("Contact" + i, item.Contact);
							sqlCommand.Parameters.AddWithValue("CoverageAreas" + i, item.CoverageAreas == null ? (object)DBNull.Value  : item.CoverageAreas);
							sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
							sqlCommand.Parameters.AddWithValue("DeliveryFee" + i, item.DeliveryFee);
							sqlCommand.Parameters.AddWithValue("IsActive" + i, item.IsActive == null ? (object)DBNull.Value  : item.IsActive);
							sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Update(Infrastructure.Data.Entities.Tables.DeliveryServiceEntity item)
        {   
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "UPDATE [DeliveryService] SET [Contact]=@Contact, [CoverageAreas]=@CoverageAreas, [CreatedAt]=@CreatedAt, [DeliveryFee]=@DeliveryFee, [IsActive]=@IsActive, [Name]=@Name WHERE [ServiceId]=@ServiceId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                    
                sqlCommand.Parameters.AddWithValue("ServiceId", item.ServiceId);
				sqlCommand.Parameters.AddWithValue("Contact",item.Contact);
				sqlCommand.Parameters.AddWithValue("CoverageAreas",item.CoverageAreas == null ? (object)DBNull.Value  : item.CoverageAreas);
				sqlCommand.Parameters.AddWithValue("CreatedAt",item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
				sqlCommand.Parameters.AddWithValue("DeliveryFee",item.DeliveryFee);
				sqlCommand.Parameters.AddWithValue("IsActive",item.IsActive == null ? (object)DBNull.Value  : item.IsActive);
				sqlCommand.Parameters.AddWithValue("Name",item.Name);
                        
                results = sqlCommand.ExecuteNonQuery();
            }
                
            return results;
        }
        public static int Update(List<Infrastructure.Data.Entities.Tables.DeliveryServiceEntity> items)
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
        private static int update(List<Infrastructure.Data.Entities.Tables.DeliveryServiceEntity> items)
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
                        query += " UPDATE [DeliveryService] SET "

							+ "[Contact]=@Contact"+ i +","
							+ "[CoverageAreas]=@CoverageAreas"+ i +","
							+ "[CreatedAt]=@CreatedAt"+ i +","
							+ "[DeliveryFee]=@DeliveryFee"+ i +","
							+ "[IsActive]=@IsActive"+ i +","
							+ "[Name]=@Name"+ i +" WHERE [ServiceId]=@ServiceId" + i 
                            + "; ";

                            sqlCommand.Parameters.AddWithValue("ServiceId" + i, item.ServiceId);
							sqlCommand.Parameters.AddWithValue("Contact" + i, item.Contact);
							sqlCommand.Parameters.AddWithValue("CoverageAreas" + i, item.CoverageAreas == null ? (object)DBNull.Value  : item.CoverageAreas);
							sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
							sqlCommand.Parameters.AddWithValue("DeliveryFee" + i, item.DeliveryFee);
							sqlCommand.Parameters.AddWithValue("IsActive" + i, item.IsActive == null ? (object)DBNull.Value  : item.IsActive);
							sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Delete(int serviceid)
        {
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "DELETE FROM [DeliveryService] WHERE [ServiceId]=@ServiceId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("ServiceId", serviceid);

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

                    string query = "DELETE FROM [DeliveryService] WHERE [ServiceId] IN ("+ queryIds +")";                    
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
