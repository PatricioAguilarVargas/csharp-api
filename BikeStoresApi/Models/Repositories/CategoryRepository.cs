using BikeStoresApi.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BikeStoresApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BikeStoresApi.Models.Repositories
{
    public partial class CategoryRepository : ICategory
    {

        private readonly IBikeStoresContext bikeStoresContext;
        public CategoryRepository(IBikeStoresContext _bikeStoresContext)
        {
            bikeStoresContext = _bikeStoresContext;
        }

        public async Task<bool> Add(Category element)
        {
            var model = await bikeStoresContext.Categories.AddAsync(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.CategoryId) != null);
        }

        public async Task<bool> Delete(int id)
        {
            var model = await Get(id);
            bikeStoresContext.Categories.Remove(model);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(id) == null);
        }

        public async Task<bool> Exists(int id)
        {
            return await bikeStoresContext.Categories.AnyAsync(e => e.CategoryId == id);
        }

        public async Task<List<Category>> Get()
        {
            var model = await bikeStoresContext.Categories.ToListAsync();
            return model;
        }

        public async Task<Category> Get(int id)
        {
            return await bikeStoresContext.Categories.FindAsync(id);
        }

        public async Task<bool> Update(Category element)
        {
            var model = bikeStoresContext.Categories.Update(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.CategoryId) != null);
        }
    }
}
