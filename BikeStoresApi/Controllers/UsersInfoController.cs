using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeStoresApi.Models;
using BikeStoresApi.Models.Entities;
using BikeStoresApi.Models.Repositories;
using BikeStoresApi.Models.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace BikeStoresApi.Controllers
{
    /// <summary>
    /// acciones de usuarios de sistema
    /// </summary>
    /// 
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersInfoController : ControllerBase
    {
        private readonly IUserInfo _userInfoRepository;

        public UsersInfoController(IUserInfo userInfoEntity)
        {
            _userInfoRepository = userInfoEntity;
        }

        /// <summary>
        /// lista los usuarios
        /// </summary>
        /// <returns>List<UserInfo>></returns>
        // GET: api/UserInfos
        [HttpGet]
        public async Task<ActionResult<List<UserInfo>>> GetUserInfos()
        {
            var model = await _userInfoRepository.Get();
            return model;
        }

        /// <summary>
        /// obtiene un usuario segun id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>UserInfo</returns>
        // GET: api/UserInfos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfo>> GetUserInfo(int id)
        {
            var userInfo = await _userInfoRepository.Get(id);

            if (userInfo == null)
            {
                return NotFound();
            }
            return userInfo;
        }

        /// <summary>
        /// modifica un usuario
        /// </summary>
        /// <param name="id">id del usuario</param>
        /// <param name="userInfo">JSON UserInfo</param>
        /// <returns></returns>
        // PUT: api/UserInfos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserInfo(int id, UserInfo userInfo)
        {
            if (id != userInfo.UserId)
            {
                return BadRequest();
            }

            try
            {
                await _userInfoRepository.Update(userInfo);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _userInfoRepository.Exists(id) == false)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// agrega un usuario 
        /// </summary>
        /// <param name="userInfo">JSON UserInfo</param>
        /// <remarks>no se agrega el id</remarks>
        /// <returns>UserInfo</returns>
        // POST: api/UserInfos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserInfo>> PostUserInfo(UserInfo userInfo)
        {
            await _userInfoRepository.Add(userInfo);
            return CreatedAtAction("GetUserInfo", new { id = userInfo.UserId  }, userInfo);
        }

        /// <summary>
        /// borra un usuario
        /// </summary>
        /// <param name="id">id de usuario</param>
        /// <returns></returns>
        // DELETE: api/UserInfos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserInfo(int id)
        {
            var userInfo = await _userInfoRepository.Get(id);
            if (userInfo == null)
            {
                return NotFound();
            }

            await _userInfoRepository.Delete(id);

            return NoContent();
        }
    }
}
