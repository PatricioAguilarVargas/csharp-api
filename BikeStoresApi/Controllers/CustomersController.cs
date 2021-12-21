using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeStoresApi.Models;
using BikeStoresApi.Models.Entities;
using BikeStoresApi.Models.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace BikeStoresApi.Controllers
{
    /// <summary>
    /// acciones relacionadas con los clientes
    /// </summary>
    /// 
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomer _custumerRepository;

        public CustomersController(ICustomer custumerRepository)
        {
            _custumerRepository = custumerRepository;
        }

        /// <summary>
        /// lista todos los clientes
        /// </summary>
        /// <returns>List<Customer></returns>
        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetCustomers()
        {
            var model = await _custumerRepository.Get();
            return model;
        }

        /// <summary>
        /// lista un cliente
        /// </summary>
        /// <param name="id">id del cliente</param>
        /// <returns>Customer</returns>
        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _custumerRepository.Get(id);

            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customer">JSON Customer</param>
        /// <returns></returns>
        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            try
            {
                await _custumerRepository.Update(customer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _custumerRepository.Exists(id) == false)
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
        /// agrega u cliente
        /// </summary>
        /// <param name="customer">JSON Customer</param>
        /// <remarks>no se agrega el id</remarks>
        /// <returns>Customer</returns>
        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            await _custumerRepository.Add(customer);
            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        /// <summary>
        /// elimina un cliente
        /// </summary>
        /// <param name="id">id de cliente</param>
        /// <returns></returns>
        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _custumerRepository.Get(id);
            if (customer == null)
            {
                return NotFound();
            }

            await _custumerRepository.Delete(id);

            return NoContent();
        }
    }
}
