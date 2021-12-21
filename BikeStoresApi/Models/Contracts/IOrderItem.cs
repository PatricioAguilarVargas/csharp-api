using BikeStoresApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoresApi.Models.Contracts
{
    public interface IOrderItem 
    {
        Task<bool> Exists(int orderId, int itemId);
        Task<List<OrderItem>> Get();
        Task<List<OrderItem>> Get(int id);
        Task<OrderItem> Get(int orderId, int itemId);
        Task<bool> Add(OrderItem element);
        Task<bool> Update(OrderItem element);
        Task<bool> Delete(int orderId, int itemId);
        Task<int> GetNewItemId(int orderId);
    }
}
