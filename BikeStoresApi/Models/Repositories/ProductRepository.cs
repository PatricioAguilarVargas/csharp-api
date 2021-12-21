using BikeStoresApi.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BikeStoresApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BikeStoresApi.Models.Repositories
{
    public partial class ProductRepository : IProduct
    {
        private readonly IBikeStoresContext bikeStoresContext;
        public ProductRepository(IBikeStoresContext _bikeStoresContext)
        {
            bikeStoresContext = _bikeStoresContext;
        }

        public async Task<bool> Add(Product element)
        {
            var model = await bikeStoresContext.Products.AddAsync(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.ProductId) != null);
        }

        public async Task<bool> Delete(int id)
        {
            var model = await Get(id);
            bikeStoresContext.Products.Remove(model);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(id) == null);
        }

        public async Task<bool> Exists(int id)
        {
            return await bikeStoresContext.Products.AnyAsync(e => e.ProductId == id);
        }

        public async Task<List<Product>> Get()
        {
            var model = await bikeStoresContext.Products.ToListAsync();
            return model;
        }

        public async Task<Product> Get(int id)
        {
            return await bikeStoresContext.Products.FindAsync(id);
        }

        public async Task<bool> Update(Product element)
        {
            var model = bikeStoresContext.Products.Update(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.ProductId) != null);
        }
    }
}
