using System;

namespace BikeRentalServiceWebAPI.Controllers.Services
{
    public class CostCalculator : ICostCalculator
    {
        public double CalculateCosts(DateTime rentalBegin, DateTime rentalEnd, double rentalPriceFirstHour, double rentalPricePerAdditionalHour)
        {
            var duration = rentalEnd - rentalBegin;
            if (duration < TimeSpan.Zero)
            {
                throw new InvalidOperationException();
            }
            if (duration <= TimeSpan.FromMinutes(15))
            {
                return 0;
            }

            double totalCost = rentalPriceFirstHour;
            var hours = (int)Math.Ceiling(duration.TotalHours - 1);
            totalCost += hours * rentalPricePerAdditionalHour;

            return totalCost;
        }
    }
}
