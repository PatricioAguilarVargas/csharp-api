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
    /// acciones de los productos
    /// </summary>
    /// 
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProduct _productRepository;

        public ProductsController(IProduct productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// lista todos los productos
        /// </summary>
        /// <returns>List<Product></returns>
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var model = await _productRepository.Get();
            return model;
        }

        /// <summary>
        /// lista un producto
        /// </summary>
        /// <param name="id">id producto</param>
        /// <returns>Product</returns>
        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.Get(id);

            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        /// <summary>
        /// modifica un producto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product">JSON Product</param>
        /// <returns></returns>
        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            try
            {
                await _productRepository.Update(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _productRepository.Exists(id) == false)
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
        /// agrega un producto
        /// </summary>
        /// <param name="product">JSON Product</param>
        /// <remarks>no se agrega el id</remarks>
        /// <returns>Product</returns>
        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            await _productRepository.Add(product);
            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        /// <summary>
        /// elimina un producto
        /// </summary>
        /// <param name="id">id producto</param>
        /// <returns></returns>
        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.Delete(id);

            return NoContent();
        }
    }
}
