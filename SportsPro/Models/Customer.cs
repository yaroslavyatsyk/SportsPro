using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please enter first name.")]
        [StringLength(50, MinimumLength = 1,ErrorMessage = "The First Name must have at least 1 character and not more than or equal 50")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "The First Name must have at least 1 character and not more than or equal 50")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please enter city.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "The First Name must have at least 1 character and not more than or equal 50")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Please enter state.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "The First Name must have at least 1 character and not more than or equal 50")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Please enter postal code.")]
        [DataType(DataType.PostalCode)]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The First Name must have at least 1 character and not more than or equal 20")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Please enter country.")]
        public string? Country{ get; set; }

        [Required(ErrorMessage = "Please enter email.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Incorrect format")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "The First Name must have at least 1 character and not more than or equal 50")]
        
        public string? Email { get; set; }

        [Required(ErrorMessage = "Enter the phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]

        public string? Phone { get; set; }

        public string FullName => FirstName?.Replace(' ', '-') + " " + LastName?.Replace(' ', '-');

    }
}
