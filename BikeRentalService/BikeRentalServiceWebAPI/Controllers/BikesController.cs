using BikeRentalServiceWebAPI.Data;
using BikeRentalServiceWebAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalServiceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikesController : ControllerBase
    {
        private readonly BikeRentalDbContext _context;

        public BikesController(BikeRentalDbContext context)
        {
            _context = context;
        }

        // GET: api/Bikes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bike>>> GetBikes([FromQuery] bool available, string sort)
        {
            var bikes = await _context.Bikes.Include(b => b.Rentals).ToListAsync();
            if (available)
            {
                bikes = bikes.Where(b => !b.IsRented).ToList();
            }
            switch (sort)
            {
                case "priceFirstHour": return bikes.OrderBy(b => b.RentalPriceFirstHour).ToList();
                case "priceAdditionalHour": return bikes.OrderBy(b => b.RentalPricePerAdditionalHour).ToList();
                case "purchaseDate": return bikes.OrderByDescending(b => b.PurchaseDate).ToList();
            }
            return bikes;
        }

        // GET: api/Bikes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bike>> GetBike(int id)
        {
            var bike = await _context.Bikes.Include(b => b.Rentals).Where(b => b.ID == id).FirstOrDefaultAsync();

            if (bike == null)
            {
                return NotFound();
            }

            return bike;
        }

        //// PUT: api/Bikes/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutBike(int id, Bike bike)
        //{
        //    if (id != bike.ID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(bike).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BikeExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Bikes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Bike>> PostBike(Bike bike)
        {
            _context.Bikes.Add(bike);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBike", new { id = bike.ID }, bike);
        }

        // DELETE: api/Bikes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Bike>> DeleteBike(int id)
        {
            var bike = await _context.Bikes.Include(b => b.Rentals).Where(b => b.ID == id).FirstOrDefaultAsync();
            if (bike == null)
            {
                return NotFound();
            }

            if (bike.Rentals != null)
            {
                return BadRequest();
            }

            _context.Bikes.Remove(bike);
            await _context.SaveChangesAsync();

            return bike;
        }

        private bool BikeExists(int id)
        {
            return _context.Bikes.Any(e => e.ID == id);
        }
    }
}
