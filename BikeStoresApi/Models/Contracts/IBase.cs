using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoresApi.Models.Contracts
{
    public interface IBase<T> where T : class
    {
        Task<bool> Exists(int id);
        Task<List<T>> Get();
        Task<T> Get(int id);
        Task<bool> Add(T element);
        Task<bool> Update(T element);
        Task<bool> Delete(int id);

    }
}
