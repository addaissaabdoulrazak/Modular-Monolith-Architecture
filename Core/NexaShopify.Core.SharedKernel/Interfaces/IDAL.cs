
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
}
