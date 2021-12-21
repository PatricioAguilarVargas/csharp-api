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
    /// acciones de Stock
    /// </summary>
    /// 
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStock _stockRepository;

        public StocksController(IStock stockRepository)
        {
            _stockRepository = stockRepository;
        }

        /// <summary>
        /// lista todos los stocks
        /// </summary>
        /// <returns>List<Stock></returns>
        // GET: api/Stocks
        [HttpGet]
        public async Task<ActionResult<List<Stock>>> GetStocks()
        {
            return await _stockRepository.Get();
        }

        /// <summary>
        /// lista los stock por tienda
        /// </summary>
        /// <param name="storeId">id de tienda</param>
        /// <returns>List<Stock></returns>
        // GET: api/Stocks/1
        [HttpGet("{storeId}")]
        public async Task<ActionResult<List<Stock>>> GetStock(int storeId)
        {
            var stock = await _stockRepository.Get(storeId);

            if (stock.Count() == 0)
            {
                return NotFound();
            }

            return stock;
        }

        /// <summary>
        /// lista el stock de un producto correspondiente a una tienda
        /// </summary>
        /// <param name="storeId">id de tienda</param>
        /// <param name="productId">id del producto</param>
        /// <returns>Stock</returns>
        // GET: api/Stocks/1/1
        [HttpGet("{storeId}/{productId}")]
        public async Task<ActionResult<Stock>> GetStock(int storeId, int productId)
        {
            var stock = await _stockRepository.Get(storeId, productId);

            if (stock == null)
            {
                return NotFound();
            }

            return stock;
        }

        /// <summary>
        /// modifica un stock
        /// </summary>
        /// <param name="storeId">id de tienda</param>
        /// <param name="productId">id del producto</param>
        /// <param name="stock">JSON Stock</param>
        /// <returns></returns>
        // PUT: api/Stocks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{storeId}/{productId}")]
        public async Task<IActionResult> PutStock(int storeId, int productId, Stock stock)
        {
            if (storeId != stock.StoreId || productId != stock.ProductId)
            {
                return BadRequest();
            }

            try
            {
                await _stockRepository.Update(stock);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_stockRepository.Exists(storeId, productId) == null)
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
        /// agrega un stock
        /// </summary>
        /// <param name="stock">JSON Stock</param>
        /// <returns>Stock</returns>
        // POST: api/Stocks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Stock>> PostStock(Stock stock)
        {

            try
            {
                await _stockRepository.Add(stock);
            }
            catch (DbUpdateException)
            {
                if (_stockRepository.Exists(stock.StoreId, stock.ProductId) == null)
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStock", new { storeId = stock.StoreId }, stock);
        }

        /// <summary>
        /// elimina un stock
        /// </summary>
        /// <param name="storeId">id de tienda</param>
        /// <param name="productId">id producto</param>
        /// <returns></returns>
        // DELETE: api/Stocks/5
        [HttpDelete("{storeId}/{productId}")]
        public async Task<IActionResult> DeleteStock(int storeId, int productId)
        {
            var stock = await _stockRepository.Get(storeId, productId);
            if (stock == null)
            {
                return NotFound();
            }

            await _stockRepository.Delete(storeId, productId);

            return NoContent();
        }

    }
}
