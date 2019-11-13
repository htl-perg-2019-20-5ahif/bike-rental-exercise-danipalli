using System;

namespace BikeRentalServiceWebAPI.Controllers.Services
{
    public interface ICostCalculator
    {
        public double CalculateCosts(DateTime rentalBegin, DateTime rentalEnd, double rentalPriceFirstHour, double rentalPricePerAdditionalHour);
    }
}
