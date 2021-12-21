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
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace BikeStoresApi.Controllers
{
    /// <summary>
    /// acciones del personal
    /// </summary>
    /// 
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StaffsController : ControllerBase
    {
        private readonly IStaff _staffRepository;

        public StaffsController(IStaff staffEntity)
        {
            _staffRepository = staffEntity;
        }

        /// <summary>
        /// lista al personal
        /// </summary>
        /// <returns>List<Staff></returns>
        // GET: api/Staffs
        [HttpGet]
        public async Task<ActionResult<List<Staff>>> GetStaffs()
        {
            var model = await _staffRepository.Get();
            
            return model;
        }

        /// <summary>
        /// lista a un trabajador por su id
        /// </summary>
        /// <param name="id">id de personal</param>
        /// <returns>Staff</returns>
        // GET: api/Staffs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Staff>> GetStaff(int id)
        {
            var staff = await _staffRepository.Get(id);

            if (staff == null)
            {
                return NotFound();
            }
            return staff;
        }

        /// <summary>
        /// modifica a un trabajador
        /// </summary>
        /// <param name="id">id del personal</param>
        /// <param name="staff">JSON Staff</param>
        /// <returns></returns>
        // PUT: api/Staffs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStaff(int id, Staff staff)
        {
            if (id != staff.StaffId)
            {
                return BadRequest();
            }

            try
            {
                await _staffRepository.Update(staff);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _staffRepository.Exists(id) == false)
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
        /// agrega a un trabajador
        /// </summary>
        /// <param name="staff">JSON Staff</param>
        /// <remarks>no se agrega el id</remarks>
        /// <returns>Staff</returns>
        // POST: api/Staffs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Staff>> PostStaff(Staff staff)
        {
            await _staffRepository.Add(staff);
            return CreatedAtAction("GetStaff", new { id = staff.StaffId }, staff);
        }

        /// <summary>
        /// borra a un trabajador
        /// </summary>
        /// <param name="id">id de personal</param>
        /// <returns></returns>
        // DELETE: api/Staffs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var staff = await _staffRepository.Get(id);
            if (staff == null)
            {
                return NotFound();
            }

            await _staffRepository.Delete(id);

            return NoContent();
        }
    }
}
