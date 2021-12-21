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
    /// acciones relacionadas con la categorias
    /// </summary>
    /// 
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategory _categoryRepository;

        public CategoriesController(ICategory categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// lista todas las categorias
        /// </summary>
        /// <returns>List<Category></returns>
        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            return await _categoryRepository.Get();
        }

        /// <summary>
        /// lista una categoria
        /// </summary>
        /// <param name="id">id categoria</param>
        /// <returns>Category</returns>
        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryRepository.Get(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        /// <summary>
        /// modifica una categoria
        /// </summary>
        /// <param name="id">id categoria</param>
        /// <param name="category">JSON Category</param>
        /// <returns></returns>
        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            try
            {
                await _categoryRepository.Update(category);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _categoryRepository.Exists(id) == false)
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
        /// agrega una categoria
        /// </summary>
        /// <param name="category">JSON Category</param>
        /// <remarks>no se agrega el id</remarks>
        /// <returns>Category</returns>
        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            await _categoryRepository.Add(category);
            return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
        }

        /// <summary>
        /// alimina una categoria
        /// </summary>
        /// <param name="id">id categoria</param>
        /// <returns></returns>
        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepository.Get(id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryRepository.Delete(id);

            return NoContent();
        }

    }
}
