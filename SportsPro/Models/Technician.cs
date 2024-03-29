﻿using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Technician
    {
        [Key]
        public int TechnicianId { get; set; }

        [Required(ErrorMessage = "Please enter first name.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please enter email.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Incorrect format")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "The First Name must have at least 1 character and not more than or equal 50")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Enter the phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string? Phone { get; set; }

        public string FullName
        {
            get
            {

                return FirstName + " " + LastName;
            }
        }

    }
}
