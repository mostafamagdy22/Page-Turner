﻿namespace PageTurner.Models.ViewModels
{
	public class CartItemViewModel
	{
		public int BookID { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public string BookTitle { get; set; }
	}
}
