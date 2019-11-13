using BikeRentalServiceWebAPI.Controllers.Services;
using BikeRentalServiceWebAPI.Data;
using BikeRentalServiceWebAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BikeRentalServiceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly BikeRentalDbContext _context;
        private readonly ICostCalculator _calculator;

        public RentalsController(BikeRentalDbContext context, ICostCalculator calculator)
        {
            _context = context;
            _calculator = calculator;
        }

        // GET: api/Rentals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
        {
            return await _context.Rentals.Include(r => r.Bike).Include(r => r.Customer).ToListAsync();
        }

        // GET: api/Rentals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> GetRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);

            if (rental == null)
            {
                return NotFound();
            }

            return rental;
        }

        //// PUT: api/Rentals/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutRental(int id, Rental rental)
        //{
        //    if (id != rental.ID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(rental).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!RentalExists(id))
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

        // POST: api/Rentals
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.

        [HttpPost]
        public async Task<ActionResult<Rental>> PostRental(Rental rental)
        {
            var bike = await _context.Bikes.FindAsync(rental.BikeID);
            var customer = await _context.Customers.FindAsync(rental.CustomerID);
            if (bike.IsRented || customer.HasActiveRental)
            {
                return BadRequest();
            }

            bike.IsRented = true;
            customer.HasActiveRental = true;

            rental.RentalBegin = System.DateTime.Now;
            rental.RentalEnd = default;
            rental.TotalCost = default;
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRental", new { id = rental.ID }, rental);
        }

        [HttpPost("{id}/end")]
        public async Task<ActionResult<Rental>> EndRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);

            if (rental == null)
            {
                return NotFound();
            }

            if (rental.Ended)
            {
                return BadRequest("Rental is already ended");
            }

            rental.RentalEnd = System.DateTime.Now;
            rental.Ended = true;
            rental.Bike.IsRented = false;
            rental.Customer.HasActiveRental = false;
            rental.TotalCost = _calculator.CalculateCosts(rental.RentalBegin, rental.RentalEnd, rental.Bike.RentalPriceFirstHour, rental.Bike.RentalPricePerAdditionalHour);

            _context.Entry(rental).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/paid")]
        public async Task<ActionResult<Rental>> PayRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);

            if (rental == null)
            {
                return NotFound();
            }

            if (rental.TotalCost <= 0 || !rental.Ended)
            {
                return BadRequest();
            }

            rental.Paid = true;

            _context.Entry(rental).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Rentals/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Rental>> DeleteRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }

            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();

            return rental;
        }
    }
}
