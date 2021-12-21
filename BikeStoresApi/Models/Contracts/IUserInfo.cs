using BikeStoresApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoresApi.Models.Contracts
{
    public interface IUserInfo : IBase<UserInfo>
    {
        Task<UserInfo> GetByEmailAndPass(string Email, string Password);
    }
}
