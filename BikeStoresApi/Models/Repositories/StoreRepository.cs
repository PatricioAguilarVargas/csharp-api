using BikeStoresApi.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BikeStoresApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BikeStoresApi.Models.Repositories
{
    public partial class StoreRepository : IStore
    {
        private readonly IBikeStoresContext bikeStoresContext;
        public StoreRepository(IBikeStoresContext _bikeStoresContext)
        {
            bikeStoresContext = _bikeStoresContext;
        }

        public async Task<bool> Add(Store element)
        {
            var model = await bikeStoresContext.Stores.AddAsync(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.StoreId) != null);
        }

        public async Task<bool> Delete(int id)
        {
            var model = await Get(id);
            bikeStoresContext.Stores.Remove(model);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(id) == null);
        }

        public async Task<bool> Exists(int id)
        {
            return await bikeStoresContext.Stores.AnyAsync(e => e.StoreId == id);
        }

        public async Task<List<Store>> Get()
        {
            var model = await bikeStoresContext.Stores.ToListAsync();
            return model;
        }

        public async Task<Store> Get(int id)
        {
            return await bikeStoresContext.Stores.FindAsync(id);
        }

        public async Task<bool> Update(Store element)
        {
            var model = bikeStoresContext.Stores.Update(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.StoreId) != null);
        }
    }
}
