using BikeStoresApi.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BikeStoresApi.Models.Entities;

#nullable disable

namespace BikeStoresApi.Models.Repositories
{
    public class BrandRepository : IBrand
    {
        private readonly IBikeStoresContext bikeStoresContext;
        public BrandRepository(IBikeStoresContext _bikeStoresContext)
        {
            bikeStoresContext = _bikeStoresContext;
        }

        public async Task<bool> Add(Brand element)
        {
            var model = await bikeStoresContext.Brands.AddAsync(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.BrandId) != null);
        }

        public async Task<bool> Delete(int id)
        {
            var model = await Get(id);
            bikeStoresContext.Brands.Remove(model);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(id) == null);
        }

        public async Task<bool> Exists(int id)
        {
            return await bikeStoresContext.Brands.AnyAsync(e => e.BrandId == id);
        }

        public async Task<List<Brand>> Get()
        {
            var model = await bikeStoresContext.Brands.ToListAsync();
            return model;
        }

        public async Task<Brand> Get(int id)
        {
            return await bikeStoresContext.Brands.FindAsync(id);
        }

        public async Task<bool> Update(Brand element)
        {
            var model = bikeStoresContext.Brands.Update(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.BrandId) != null);
        }
    }
}
