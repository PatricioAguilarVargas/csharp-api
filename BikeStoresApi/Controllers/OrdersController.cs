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
    /// acciones de las ordenes
    /// </summary>
    /// 
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrder _orderRepository;

        public OrdersController(IOrder orderEntity)
        {
            _orderRepository = orderEntity;
        }

        /// <summary>
        /// lista todas las ordenes
        /// </summary>
        /// <returns>List<Order></returns>
        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            var model = await _orderRepository.Get();
            return model;
        }

        /// <summary>
        /// lista una orden segun su id
        /// </summary>
        /// <param name="id">id de orden</param>
        /// <returns>Order</returns>
        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderRepository.Get(id);

            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        /// <summary>
        /// modifica una orden
        /// </summary>
        /// <param name="id">id de orden</param>
        /// <param name="order">JSON Order</param>
        /// <returns></returns>
        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            try
            {
                await _orderRepository.Update(order);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _orderRepository.Exists(id) == false)
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
        /// agrega una orden
        /// </summary>
        /// <param name="order">JSON Order</param>
        /// <remarks>no se agrega el id</remarks>
        /// <returns>Order</returns>
        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            await _orderRepository.Add(order);
            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        /// <summary>
        /// elimina una orden
        /// </summary>
        /// <param name="id">id de orden</param>
        /// <returns></returns>
        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepository.Get(id);
            if (order == null)
            {
                return NotFound();
            }

            await _orderRepository.Delete(id);

            return NoContent();
        }
    }
}
