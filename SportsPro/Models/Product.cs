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
		public double Price { get; set; }
		

	}
}

