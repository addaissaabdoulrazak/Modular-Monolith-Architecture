using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexaShopify.Core.SharedKernel.Interfaces
{
    public interface IHandle<TData, TResponse>
    {
        TResponse Handle();
        TResponse Validate();
    }
    public interface IHandleAsync<TData, TResponse>
    {
        Task<TResponse> HandleAsync();
        Task<TResponse> ValidateAsync();
    }
}
