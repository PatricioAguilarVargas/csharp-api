using BikeStoresApi.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BikeStoresApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

#nullable disable

namespace BikeStoresApi.Models.Repositories
{
    public partial class OrderItemRepository : IOrderItem
    {
     
        private readonly IBikeStoresContext bikeStoresContext;
        public OrderItemRepository(IBikeStoresContext _bikeStoresContext)
        {
            bikeStoresContext = _bikeStoresContext;
        }

        public async Task<bool> Add(OrderItem element)
        {
            var model = await bikeStoresContext.OrderItems.AddAsync(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.OrderId, model.Entity.ItemId) != null);
        }

        public async Task<bool> Delete(int orderId, int itemId)
        {
            var model = await Get(orderId, itemId);
            bikeStoresContext.OrderItems.Remove(model);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(orderId, itemId) == null);
        }

        public async Task<bool> Exists(int orderId, int itemId)
        {
            return await bikeStoresContext.OrderItems.AnyAsync(e => e.OrderId == orderId && e.ItemId == itemId);
        }

        public async Task<List<OrderItem>> Get()
        {
            var model = await bikeStoresContext.OrderItems.ToListAsync();
            return model;
        }

        public async Task<List<OrderItem>> Get(int id)
        {
            var model = await bikeStoresContext.OrderItems.Where(x => x.OrderId == id).ToListAsync();
            return model;
        }

        public async Task<OrderItem> Get(int orderId, int itemId)
        {
            var model = await bikeStoresContext.OrderItems.FirstOrDefaultAsync(x => x.OrderId == orderId && x.ItemId == itemId);
            return model;
        }

        public async Task<int> GetNewItemId(int orderId)
        {
            int value = await bikeStoresContext.OrderItems.Where(x => x.OrderId == orderId).MaxAsync(x => x.ItemId);
            return value + 1;
        }

        public async Task<bool> Update(OrderItem element)
        {
            var model = bikeStoresContext.OrderItems.Update(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.OrderId, model.Entity.ItemId) != null);
        }
    }
}
