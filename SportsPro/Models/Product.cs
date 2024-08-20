using System;
using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
	public class Product
	{
		[Key]
		public int ProductId { get; set; }

		[Required(ErrorMessage = "Please enter product code.")]
		public string? ProductCode { get; set; }

		[Required(ErrorMessage = "Please enter name.")]
		public string? Name { get; set; }

		public DateTime ReleaseDate { get; set; }
		
		[Required(ErrorMessage = "Please enter the price")]
		[Range(0.0,9999.99, ErrorMessage = "Price must be between 0.0 and 9999.99")]
		public double Price { get; set; }

		

	}
}

