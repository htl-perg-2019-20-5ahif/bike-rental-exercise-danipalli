using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BikeRentalServiceWebAPI.Model
{

    public enum GenderType { Male, Female, Unknown }

    public class Customer
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public GenderType Gender { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(75)]
        public string LastName { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        [MaxLength(75)]
        public string Street { get; set; }

        [Required]
        [MaxLength(10)]
        public string HouseNumber { get; set; }

        [Required]
        [MaxLength(10)]
        public string ZipCode { get; set; }

        [Required]
        [MaxLength(75)]
        public string Town { get; set; }

        public bool HasActiveRental { get; set; }

        public List<Rental> Rentals { get; set; }
    }
}
