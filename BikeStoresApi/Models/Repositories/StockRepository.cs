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
    public partial class StockRepository : IStock
    {

        private readonly IBikeStoresContext bikeStoresContext;
        public StockRepository(IBikeStoresContext _bikeStoresContext)
        {
            bikeStoresContext = _bikeStoresContext;
        }

        public async Task<bool> Add(Stock element)
        {
            var model = await bikeStoresContext.Stocks.AddAsync(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.StoreId, model.Entity.ProductId) != null);
        }

        public async Task<bool> Delete(int storeId, int productId)
        {
            var model = await Get(storeId, productId);
            bikeStoresContext.Stocks.Remove(model);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(storeId, productId) == null);
        }


        public async Task<bool> Exists(int storeId, int productId)
        {
            return await bikeStoresContext.Stocks.AnyAsync(e => e.StoreId == storeId && e.ProductId == productId);
        }

        public async Task<List<Stock>> Get()
        {
            var model = await bikeStoresContext.Stocks.ToListAsync();
            return model;
        }

        public async Task<List<Stock>> Get(int storeId)
        {
            var model = await bikeStoresContext.Stocks.Where(x => x.StoreId == storeId).ToListAsync();
            return model;
        }

        public async Task<Stock> Get(int storeId, int productId)
        {
            var model = await bikeStoresContext.Stocks.FirstOrDefaultAsync(x => x.StoreId == storeId && x.ProductId == productId);
            return model;
        }

        public async Task<bool> Update(Stock element)
        {
            var model = bikeStoresContext.Stocks.Update(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.StoreId, model.Entity.ProductId) != null);
        }

      
    }
}
