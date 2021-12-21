using BikeStoresApi.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BikeStoresApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BikeStoresApi.Models.Repositories
{
    public partial class StaffRepository : IStaff
    {
        private readonly IBikeStoresContext bikeStoresContext;
        public StaffRepository(IBikeStoresContext _bikeStoresContext)
        {
            bikeStoresContext = _bikeStoresContext;
        }

        public async Task<bool> Add(Staff element)
        {
            var model = await bikeStoresContext.Staffs.AddAsync(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.StaffId) != null);
        }

        public async Task<bool> Delete(int id)
        {
            var model = await Get(id);
            bikeStoresContext.Staffs.Remove(model);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(id) == null);
        }

        public async Task<bool> Exists(int id)
        {
            return await bikeStoresContext.Staffs.AnyAsync(e => e.StaffId == id);
        }

        public async Task<List<Staff>> Get()
        {
            var model = await bikeStoresContext.Staffs.ToListAsync();
            return model;
        }

        public async Task<Staff> Get(int id)
        {
            return await bikeStoresContext.Staffs.FindAsync(id);
        }

        public async Task<bool> Update(Staff element)
        {
            var model = bikeStoresContext.Staffs.Update(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.StaffId) != null);
        }
    }
}
