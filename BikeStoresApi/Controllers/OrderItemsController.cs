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
    /// acciones relacionada con productos de una orden
    /// </summary>
    /// 
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItem _orderItemRepository;

        public OrderItemsController(IOrderItem orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        /// <summary>
        /// lista todos los items de una orden
        /// </summary>
        /// <returns>List<OrderItem></returns>
        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<List<OrderItem>>> GetOrderItems()
        {
            return await _orderItemRepository.Get();
        }

        /// <summary>
        /// lista los items de una orden
        /// </summary>
        /// <param name="orderId">id de la orden</param>
        /// <returns>List<OrderItem></returns>
        // GET: api/OrderItems/1
        [HttpGet("{orderId}")]
        public async Task<ActionResult<List<OrderItem>>> GetOrderItem(int orderId)
        {
            var orderItem = await _orderItemRepository.Get(orderId);

            if (orderItem.Count() == 0)
            {
                return NotFound();
            }

            return orderItem;
        }

        /// <summary>
        /// entrega una item de una orden
        /// </summary>
        /// <param name="orderId">id de la orden</param>
        /// <param name="itemId">id del item</param>
        /// <returns>OrderItem</returns>
        // GET: api/OrderItems/1/1
        [HttpGet("{orderId}/{itemId}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int orderId, int itemId)
        {
            var orderItem = await _orderItemRepository.Get(orderId, itemId);

            if (orderItem == null)
            {
                return NotFound();
            }

            return orderItem;
        }

        /// <summary>
        /// modifica un item de una orden
        /// </summary>
        /// <param name="orderId">id de la orden</param>
        /// <param name="itemId">id del item</param>
        /// <param name="orderItem">JSON OrderItem</param>
        /// <returns></returns>
        // PUT: api/OrderItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{orderId}/{itemId}")]
        public async Task<IActionResult> PutOrderItem(int orderId, int itemId, OrderItem orderItem)
        {
            if (orderId != orderItem.OrderId || itemId != orderItem.ItemId)
            {
                return BadRequest();
            }

            try
            {
                await _orderItemRepository.Update(orderItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_orderItemRepository.Exists(orderId, itemId) == null)
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
        /// agrega un item a una orden
        /// </summary>
        /// <param name="orderItem">JSON OrderItem</param>
        /// 
        /// <returns>OrderItem</returns>
        // POST: api/OrderItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItem orderItem)
        {
            
            try
            {
                orderItem.ItemId = await _orderItemRepository.GetNewItemId(orderItem.OrderId);
                await _orderItemRepository.Add(orderItem);
            }
            catch (DbUpdateException)
            {
                if (_orderItemRepository.Exists(orderItem.OrderId, orderItem.ItemId) == null)
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrderItem", new { orderId = orderItem.OrderId }, orderItem);
        }

        /// <summary>
        /// borra una item de una orden
        /// </summary>
        /// <param name="orderId">id de la orden</param>
        /// <param name="itemId">id del item</param>
        /// <returns></returns>
        // DELETE: api/OrderItems/5
        [HttpDelete("{orderId}/{itemId}")]
        public async Task<IActionResult> DeleteOrderItem(int orderId, int itemId)
        {
            var orderItem = await _orderItemRepository.Get(orderId, itemId);
            if (orderItem == null)
            {
                return NotFound();
            }

            await _orderItemRepository.Delete(orderId, itemId);

            return NoContent();
        }

    }
}
