using BikeStoresApi.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BikeStoresApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BikeStoresApi.Models.Repositories
{
    public partial class OrderRepository : IOrder
    {

        private readonly IBikeStoresContext bikeStoresContext;
        public OrderRepository(IBikeStoresContext _bikeStoresContext)
        {
            bikeStoresContext = _bikeStoresContext;
        }

        public async Task<bool> Add(Order element)
        {
            var model = await bikeStoresContext.Orders.AddAsync(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.OrderId) != null);
        }

        public async Task<bool> Delete(int id)
        {
            var model = await Get(id);
            bikeStoresContext.Orders.Remove(model);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(id) == null);
        }

        public async Task<bool> Exists(int id)
        {
            return await bikeStoresContext.Orders.AnyAsync(e => e.OrderId == id);
        }

        public async Task<List<Order>> Get()
        {
            var model = await bikeStoresContext.Orders.ToListAsync();
            return model;
        }

        public async Task<Order> Get(int id)
        {
            return await bikeStoresContext.Orders.FindAsync(id);
        }

        public async Task<bool> Update(Order element)
        {
            var model = bikeStoresContext.Orders.Update(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.OrderId) != null);
        }
    }
}
