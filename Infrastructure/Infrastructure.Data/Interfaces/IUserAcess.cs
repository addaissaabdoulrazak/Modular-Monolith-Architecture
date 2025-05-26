using Infrastructure.Data.Entities.Tables.COR;
using NexaShopify.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Interfaces
{
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
