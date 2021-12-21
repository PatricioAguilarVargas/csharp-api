using BikeStoresApi.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BikeStoresApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BikeStoresApi.Models.Repositories
{
    public partial class UserInfoRepository : IUserInfo
    {
        private readonly IBikeStoresContext bikeStoresContext;
        public UserInfoRepository(IBikeStoresContext _bikeStoresContext)
        {
            bikeStoresContext = _bikeStoresContext;
        }

        public async Task<bool> Add(UserInfo element)
        {
            var model = await bikeStoresContext.UsersInfo.AddAsync(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.UserId) != null);
        }

        public async Task<bool> Delete(int id)
        {
            var model = await Get(id);
            bikeStoresContext.UsersInfo.Remove(model);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(id) == null);
        }

        public async Task<bool> Exists(int id)
        {
            return await bikeStoresContext.UsersInfo.AnyAsync(e => e.UserId == id);
        }

        public async Task<List<UserInfo>> Get()
        {
            var model = await bikeStoresContext.UsersInfo.ToListAsync();
            return model;
        }

        public async Task<UserInfo> Get(int id)
        {
            return await bikeStoresContext.UsersInfo.FindAsync(id);
        }

        public async Task<UserInfo> GetByEmailAndPass(string email, string password)
        {
            return await bikeStoresContext.UsersInfo.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
        }

        public async Task<bool> Update(UserInfo element)
        {
            var model = bikeStoresContext.UsersInfo.Update(element);
            await bikeStoresContext.SaveChangesAsync();
            return (await Get(model.Entity.UserId) != null);
        }
    }
}
