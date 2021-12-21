using BikeStoresApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoresApi.Models.Contracts
{
    public interface IStock 
    {
        Task<bool> Exists(int storeId, int productId);
        Task<List<Stock>> Get();
        Task<List<Stock>> Get(int storeId);
        Task<Stock> Get(int storeId, int productId);
        Task<bool> Add(Stock element);
        Task<bool> Update(Stock element);
        Task<bool> Delete(int storeId, int productId);
    }
}
