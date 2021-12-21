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
    /// acciones sobre las tiendas
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStore _storeRepository;

        public StoresController(IStore storeEntity)
        {
            _storeRepository = storeEntity;
        }

        /// <summary>
        /// obtiene las tiendas
        /// </summary>
        /// <returns>List<Store></returns>
        // GET: api/Stores
        [HttpGet]
        public async Task<ActionResult<List<Store>>> GetStores()
        {
            var model = await _storeRepository.Get();
            return model;
        }

        /// <summary>
        /// obtiene una tienda
        /// </summary>
        /// <param name="id">id de la tienda</param>
        /// <returns>Store</returns>
        // GET: api/Stores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Store>> GetStore(int id)
        {
            var store = await _storeRepository.Get(id);

            if (store == null)
            {
                return NotFound();
            }
            return store;
        }

        /// <summary>
        /// modifica una tienda
        /// </summary>
        /// <param name="id">id de latienda</param>
        /// <param name="store">JSON Store</param>
        /// <returns></returns>
        // PUT: api/Stores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStore(int id, Store store)
        {
            if (id != store.StoreId)
            {
                return BadRequest();
            }

            try
            {
                await _storeRepository.Update(store);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _storeRepository.Exists(id) == false)
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
        /// modifica una tienda
        /// </summary>
        /// <param name="store">JSON Store</param>
        /// <remarks>no se agrega el id</remarks>
        /// <returns>Store</returns>
        // POST: api/Stores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Store>> PostStore(Store store)
        {
            await _storeRepository.Add(store);
            return CreatedAtAction("GetStore", new { id = store.StoreId }, store);
        }

        /// <summary>
        /// borra una tienda
        /// </summary>
        /// <param name="id">id de la tienda</param>
        /// <returns></returns>
        // DELETE: api/Stores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var store = await _storeRepository.Get(id);
            if (store == null)
            {
                return NotFound();
            }

            await _storeRepository.Delete(id);

            return NoContent();
        }
    }
}
