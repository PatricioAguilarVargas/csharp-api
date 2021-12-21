using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeStoresApi.Models;
using BikeStoresApi.Models.Contracts;
using BikeStoresApi.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BikeStoresApi.Controllers
{
    /// <summary>
    /// acciones relacionadas con las marcas
    /// </summary>
    /// 
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrand _brandRepository;

        public BrandsController(IBrand brandEntity)
        {
            _brandRepository = brandEntity;
        }

        /// <summary>
        /// lista todas las marcas
        /// </summary>
        /// <returns>List<Brand></returns>
        // GET: api/Brands
        [HttpGet]
        public async Task<ActionResult<List<Brand>>> GetBrands()
        {
            var model = await _brandRepository.Get();
            return model;
        }

        /// <summary>
        /// lista una marca
        /// </summary>
        /// <param name="id">id marca</param>
        /// <returns>Brand</returns>
        // GET: api/Brands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            var brand = await _brandRepository.Get(id);

            if (brand == null)
            {
                return NotFound();
            }
            return brand;
        }

        /// <summary>
        /// modifica una marca
        /// </summary>
        /// <param name="id">id de lamarca</param>
        /// <param name="brand">JSON Brand</param>
        /// <returns></returns>
        // PUT: api/Brands/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrand(int id, Brand brand)
        {
            if (id != brand.BrandId)
            {
                return BadRequest();
            }

            try
            {
                await _brandRepository.Update(brand);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _brandRepository.Exists(id) == false)
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
        /// agrega una marca
        /// </summary>
        /// <param name="brand">JSON Brand</param>
        /// <remarks>no se agrega el id</remarks>
        /// <returns></returns>
        // POST: api/Brands
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            await _brandRepository.Add(brand);
            return CreatedAtAction("GetBrand", new { id = brand.BrandId }, brand);
        }

        /// <summary>
        /// alimina una marca
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Brands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var brand = await _brandRepository.Get(id);
            if (brand == null)
            {
                return NotFound();
            }

            await _brandRepository.Delete(id);

            return NoContent();
        }
    }
}
