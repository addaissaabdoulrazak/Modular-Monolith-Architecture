using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables
{

    public class OrderAccess
    {
        #region Default Methods
        public static Infrastructure.Data.Entities.Tables.OrderEntity Get(long orderid)
        {
            var dataTable = new DataTable();
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Order] WHERE [OrderId]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", orderid); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.OrderEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Infrastructure.Data.Entities.Tables.OrderEntity> Get()
        {  
            var dataTable = new DataTable();     
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Order]";
                var sqlCommand = new SqlCommand(query, sqlConnection); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.OrderEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.OrderEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.OrderEntity> Get(List<long> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<Infrastructure.Data.Entities.Tables.OrderEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.OrderEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.OrderEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.OrderEntity> get(List<long> ids)
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

                    sqlCommand.CommandText = $"SELECT * FROM [Order] WHERE [OrderId] IN ({queryIds})";                    
                new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.OrderEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.OrderEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.OrderEntity>();
        }

        public static long Insert(Infrastructure.Data.Entities.Tables.OrderEntity item)
        {
            long response = long.MinValue;
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();

                string query = "INSERT INTO [Order] ([CreatedAt],[CustomerId],[DeliveryAddress],[OrderDate],[OrderNumber],[OrderStatus],[OrderStatus_Id],[PaymentMethod],[PaymentMethod_Id],[PaymentStatus],[PaymentStatus_Id],[ShippingAddressId],[StoreId],[TotalAmount],[TrackingNumber],[UpdatedAt])  VALUES (@CreatedAt,@CustomerId,@DeliveryAddress,@OrderDate,@OrderNumber,@OrderStatus,@OrderStatus_Id,@PaymentMethod,@PaymentMethod_Id,@PaymentStatus,@PaymentStatus_Id,@ShippingAddressId,@StoreId,@TotalAmount,@TrackingNumber,@UpdatedAt); ";
                query += "SELECT SCOPE_IDENTITY();";

                using (var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CreatedAt",item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
					sqlCommand.Parameters.AddWithValue("CustomerId",item.CustomerId);
					sqlCommand.Parameters.AddWithValue("DeliveryAddress",item.DeliveryAddress == null ? (object)DBNull.Value  : item.DeliveryAddress);
					sqlCommand.Parameters.AddWithValue("OrderDate",item.OrderDate == null ? (object)DBNull.Value  : item.OrderDate);
					sqlCommand.Parameters.AddWithValue("OrderNumber",item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("OrderStatus",item.OrderStatus);
					sqlCommand.Parameters.AddWithValue("OrderStatus_Id",item.OrderStatus_Id);
					sqlCommand.Parameters.AddWithValue("PaymentMethod",item.PaymentMethod);
					sqlCommand.Parameters.AddWithValue("PaymentMethod_Id",item.PaymentMethod_Id);
					sqlCommand.Parameters.AddWithValue("PaymentStatus",item.PaymentStatus);
					sqlCommand.Parameters.AddWithValue("PaymentStatus_Id",item.PaymentStatus_Id);
					sqlCommand.Parameters.AddWithValue("ShippingAddressId",item.ShippingAddressId);
					sqlCommand.Parameters.AddWithValue("StoreId",item.StoreId);
					sqlCommand.Parameters.AddWithValue("TotalAmount",item.TotalAmount);
					sqlCommand.Parameters.AddWithValue("TrackingNumber",item.TrackingNumber == null ? (object)DBNull.Value  : item.TrackingNumber);
					sqlCommand.Parameters.AddWithValue("UpdatedAt",item.UpdatedAt == null ? (object)DBNull.Value  : item.UpdatedAt);

                    var result = sqlCommand.ExecuteScalar();
                    response = result == null? long.MinValue:  long.TryParse(result.ToString(), out var insertedId) ? insertedId : long.MinValue;
                }
                sqlTransaction.Commit();

                return response;
            }
        }
        public static int Insert(List<Infrastructure.Data.Entities.Tables.OrderEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 17; // Nb params per query
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
        private static int insert(List<Infrastructure.Data.Entities.Tables.OrderEntity> items)
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
                        query += " INSERT INTO [Order] ([CreatedAt],[CustomerId],[DeliveryAddress],[OrderDate],[OrderNumber],[OrderStatus],[OrderStatus_Id],[PaymentMethod],[PaymentMethod_Id],[PaymentStatus],[PaymentStatus_Id],[ShippingAddressId],[StoreId],[TotalAmount],[TrackingNumber],[UpdatedAt]) VALUES ( "

							+ "@CreatedAt"+ i +","
							+ "@CustomerId"+ i +","
							+ "@DeliveryAddress"+ i +","
							+ "@OrderDate"+ i +","
							+ "@OrderNumber"+ i +","
							+ "@OrderStatus"+ i +","
							+ "@OrderStatus_Id"+ i +","
							+ "@PaymentMethod"+ i +","
							+ "@PaymentMethod_Id"+ i +","
							+ "@PaymentStatus"+ i +","
							+ "@PaymentStatus_Id"+ i +","
							+ "@ShippingAddressId"+ i +","
							+ "@StoreId"+ i +","
							+ "@TotalAmount"+ i +","
							+ "@TrackingNumber"+ i +","
							+ "@UpdatedAt"+ i 
                            + "); ";

                            
							sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
							sqlCommand.Parameters.AddWithValue("CustomerId" + i, item.CustomerId);
							sqlCommand.Parameters.AddWithValue("DeliveryAddress" + i, item.DeliveryAddress == null ? (object)DBNull.Value  : item.DeliveryAddress);
							sqlCommand.Parameters.AddWithValue("OrderDate" + i, item.OrderDate == null ? (object)DBNull.Value  : item.OrderDate);
							sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber);
							sqlCommand.Parameters.AddWithValue("OrderStatus" + i, item.OrderStatus);
							sqlCommand.Parameters.AddWithValue("OrderStatus_Id" + i, item.OrderStatus_Id);
							sqlCommand.Parameters.AddWithValue("PaymentMethod" + i, item.PaymentMethod);
							sqlCommand.Parameters.AddWithValue("PaymentMethod_Id" + i, item.PaymentMethod_Id);
							sqlCommand.Parameters.AddWithValue("PaymentStatus" + i, item.PaymentStatus);
							sqlCommand.Parameters.AddWithValue("PaymentStatus_Id" + i, item.PaymentStatus_Id);
							sqlCommand.Parameters.AddWithValue("ShippingAddressId" + i, item.ShippingAddressId);
							sqlCommand.Parameters.AddWithValue("StoreId" + i, item.StoreId);
							sqlCommand.Parameters.AddWithValue("TotalAmount" + i, item.TotalAmount);
							sqlCommand.Parameters.AddWithValue("TrackingNumber" + i, item.TrackingNumber == null ? (object)DBNull.Value  : item.TrackingNumber);
							sqlCommand.Parameters.AddWithValue("UpdatedAt" + i, item.UpdatedAt == null ? (object)DBNull.Value  : item.UpdatedAt);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Update(Infrastructure.Data.Entities.Tables.OrderEntity item)
        {   
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "UPDATE [Order] SET [CreatedAt]=@CreatedAt, [CustomerId]=@CustomerId, [DeliveryAddress]=@DeliveryAddress, [OrderDate]=@OrderDate, [OrderNumber]=@OrderNumber, [OrderStatus]=@OrderStatus, [OrderStatus_Id]=@OrderStatus_Id, [PaymentMethod]=@PaymentMethod, [PaymentMethod_Id]=@PaymentMethod_Id, [PaymentStatus]=@PaymentStatus, [PaymentStatus_Id]=@PaymentStatus_Id, [ShippingAddressId]=@ShippingAddressId, [StoreId]=@StoreId, [TotalAmount]=@TotalAmount, [TrackingNumber]=@TrackingNumber, [UpdatedAt]=@UpdatedAt WHERE [OrderId]=@OrderId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                    
                sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("CreatedAt",item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
				sqlCommand.Parameters.AddWithValue("CustomerId",item.CustomerId);
				sqlCommand.Parameters.AddWithValue("DeliveryAddress",item.DeliveryAddress == null ? (object)DBNull.Value  : item.DeliveryAddress);
				sqlCommand.Parameters.AddWithValue("OrderDate",item.OrderDate == null ? (object)DBNull.Value  : item.OrderDate);
				sqlCommand.Parameters.AddWithValue("OrderNumber",item.OrderNumber);
				sqlCommand.Parameters.AddWithValue("OrderStatus",item.OrderStatus);
				sqlCommand.Parameters.AddWithValue("OrderStatus_Id",item.OrderStatus_Id);
				sqlCommand.Parameters.AddWithValue("PaymentMethod",item.PaymentMethod);
				sqlCommand.Parameters.AddWithValue("PaymentMethod_Id",item.PaymentMethod_Id);
				sqlCommand.Parameters.AddWithValue("PaymentStatus",item.PaymentStatus);
				sqlCommand.Parameters.AddWithValue("PaymentStatus_Id",item.PaymentStatus_Id);
				sqlCommand.Parameters.AddWithValue("ShippingAddressId",item.ShippingAddressId);
				sqlCommand.Parameters.AddWithValue("StoreId",item.StoreId);
				sqlCommand.Parameters.AddWithValue("TotalAmount",item.TotalAmount);
				sqlCommand.Parameters.AddWithValue("TrackingNumber",item.TrackingNumber == null ? (object)DBNull.Value  : item.TrackingNumber);
				sqlCommand.Parameters.AddWithValue("UpdatedAt",item.UpdatedAt == null ? (object)DBNull.Value  : item.UpdatedAt);
                        
                results = sqlCommand.ExecuteNonQuery();
            }
                
            return results;
        }
        public static int Update(List<Infrastructure.Data.Entities.Tables.OrderEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 17; // Nb params per query
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
        private static int update(List<Infrastructure.Data.Entities.Tables.OrderEntity> items)
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
                        query += " UPDATE [Order] SET "

							+ "[CreatedAt]=@CreatedAt"+ i +","
							+ "[CustomerId]=@CustomerId"+ i +","
							+ "[DeliveryAddress]=@DeliveryAddress"+ i +","
							+ "[OrderDate]=@OrderDate"+ i +","
							+ "[OrderNumber]=@OrderNumber"+ i +","
							+ "[OrderStatus]=@OrderStatus"+ i +","
							+ "[OrderStatus_Id]=@OrderStatus_Id"+ i +","
							+ "[PaymentMethod]=@PaymentMethod"+ i +","
							+ "[PaymentMethod_Id]=@PaymentMethod_Id"+ i +","
							+ "[PaymentStatus]=@PaymentStatus"+ i +","
							+ "[PaymentStatus_Id]=@PaymentStatus_Id"+ i +","
							+ "[ShippingAddressId]=@ShippingAddressId"+ i +","
							+ "[StoreId]=@StoreId"+ i +","
							+ "[TotalAmount]=@TotalAmount"+ i +","
							+ "[TrackingNumber]=@TrackingNumber"+ i +","
							+ "[UpdatedAt]=@UpdatedAt"+ i +" WHERE [OrderId]=@OrderId" + i 
                            + "; ";

                            sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
							sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
							sqlCommand.Parameters.AddWithValue("CustomerId" + i, item.CustomerId);
							sqlCommand.Parameters.AddWithValue("DeliveryAddress" + i, item.DeliveryAddress == null ? (object)DBNull.Value  : item.DeliveryAddress);
							sqlCommand.Parameters.AddWithValue("OrderDate" + i, item.OrderDate == null ? (object)DBNull.Value  : item.OrderDate);
							sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber);
							sqlCommand.Parameters.AddWithValue("OrderStatus" + i, item.OrderStatus);
							sqlCommand.Parameters.AddWithValue("OrderStatus_Id" + i, item.OrderStatus_Id);
							sqlCommand.Parameters.AddWithValue("PaymentMethod" + i, item.PaymentMethod);
							sqlCommand.Parameters.AddWithValue("PaymentMethod_Id" + i, item.PaymentMethod_Id);
							sqlCommand.Parameters.AddWithValue("PaymentStatus" + i, item.PaymentStatus);
							sqlCommand.Parameters.AddWithValue("PaymentStatus_Id" + i, item.PaymentStatus_Id);
							sqlCommand.Parameters.AddWithValue("ShippingAddressId" + i, item.ShippingAddressId);
							sqlCommand.Parameters.AddWithValue("StoreId" + i, item.StoreId);
							sqlCommand.Parameters.AddWithValue("TotalAmount" + i, item.TotalAmount);
							sqlCommand.Parameters.AddWithValue("TrackingNumber" + i, item.TrackingNumber == null ? (object)DBNull.Value  : item.TrackingNumber);
							sqlCommand.Parameters.AddWithValue("UpdatedAt" + i, item.UpdatedAt == null ? (object)DBNull.Value  : item.UpdatedAt);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Delete(long orderid)
        {
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "DELETE FROM [Order] WHERE [OrderId]=@OrderId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("OrderId", orderid);

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

                    string query = "DELETE FROM [Order] WHERE [OrderId] IN ("+ queryIds +")";                    
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
