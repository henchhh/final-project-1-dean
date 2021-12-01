using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectApp.Data;
using ProjectApp.Models;
using System;

namespace ProjectApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProjectController : ControllerBase
    {
        // [Route("TestRun")]
        // [HttpGet]
        private readonly ApiDbContext _context;

        // public ActionResult TestRun()
        // {
        //     return Ok("success");
        // }


        public ProjectController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "Get All")]
        public async Task<ActionResult> GetItems()
        {
            var items = await _context.Items.ToListAsync();
            return Ok(items);
        }

        [HttpPost(Name = "Create Data")]
        public async Task<ActionResult> CreateItem(ItemData data)
        {
            if (ModelState.IsValid)
            {
                await _context.Items.AddAsync(data);
                await _context.SaveChangesAsync();
                System.Console.WriteLine("Data berhasil dibuat.");
                return CreatedAtAction("GetItem", new { data.paymentDetailId }, data);
            }
            else{
                return new JsonResult("Something went wrong") { StatusCode = 500 };
            }
            
        }

        [HttpGet("{id}", Name="Get Data Where")]
        public async Task<ActionResult> GetItem(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.paymentDetailId == id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPut("{id}",Name="Update Data")]
        public async Task<ActionResult> UpdateItem(int id, ItemData item)
        {
            if (id != item.paymentDetailId)
            {
                return BadRequest();
            }

            var existItem = await _context.Items.FirstOrDefaultAsync(x => x.paymentDetailId == id);

            if (existItem == null)
            {
                return NotFound();
            }

            existItem.cardOwnerName = item.cardOwnerName;
            existItem.cardNumber = item.cardNumber;
            existItem.expirationDate = item.expirationDate;
            existItem.securityCode = item.securityCode;

            await _context.SaveChangesAsync();

            return Ok(item);
        }

        [HttpDelete("{id}", Name="Delete Data")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var existItem = await _context.Items.FirstOrDefaultAsync(x => x.paymentDetailId == id);

            if (existItem == null)
            {
                return NotFound();
            }

            _context.Items.Remove(existItem);
            await _context.SaveChangesAsync();

            return Ok(existItem);
        }
    }

}