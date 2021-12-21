using BikeStoresApi.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BikeStoresApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
#nullable disable

namespace BikeStoresApi.Models.Repositories
{
    public partial class CustomerRepository : ICustomer
    {

        private readonly IBikeStoresContext bikeStoresContext;
        public CustomerRepository(IBikeStoresContext _bikeStoresContext)
        {
            bikeStoresContext = _bikeStoresContext;
        }

        public async Task<bool> Add(Customer element)
        {
            var model = await bikeStoresContext.Customers.AddAsync(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.CustomerId) != null);
        }

        public async Task<bool> Delete(int id)
        {
            var model = await Get(id);
            bikeStoresContext.Customers.Remove(model);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(id) == null);
        }

        public async Task<bool> Exists(int id)
        {
            return await bikeStoresContext.Customers.AnyAsync(e => e.CustomerId == id);
        }

        public async Task<List<Customer>> Get()
        {
            var model = await bikeStoresContext.Customers.ToListAsync();
            return model;
        }

        public async Task<Customer> Get(int id)
        {
            return await bikeStoresContext.Customers.FindAsync(id);
        }

        public async Task<bool> Update(Customer element)
        {
            var model = bikeStoresContext.Customers.Update(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.CustomerId) != null);
        }
    }
}
