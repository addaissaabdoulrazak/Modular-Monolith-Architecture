using Infrastructure.Data.Entities.Tables.COR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NexaShopify.Core.SharedKernel.Interfaces
{
    public interface IDAL<T> where T : class
    {
        T Get(int id);
        List<T> Get();
        List<T> Get(List<int> ids);
        int Insert(T item);
        int Insert(List<T> items);
        int Update(T item);
        int Update(List<T> items);
        int Delete(int id);
        int Delete(List<int> ids);
    }

    #region>> Adda Issa 4/2025 || Interface segregation principle

    public interface IUserAcess : IDAL<UsersEntity>
    {
        List<Infrastructure.Data.Entities.Tables.COR.UsersEntity> GetByCurrentUserId(int userId);
        bool CheckExists(string username);
        Infrastructure.Data.Entities.Tables.COR.UsersEntity GetByUsername(string username);
        Infrastructure.Data.Entities.Tables.COR.UsersEntity GetByName(string username);
    }

    #endregion
}
