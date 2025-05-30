﻿using PageTurner.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace PageTurner.CustomValidtion
{
	public class UniqueISBNAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{

			var httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
			var actionName = httpContextAccessor.HttpContext?.Request.RouteValues["action"]?.ToString();
			
			if (actionName == "Edit")
				return ValidationResult.Success;

			if (value == null)
				return ValidationResult.Success;

			var isbn = value.ToString();

			var bookRepository = validationContext.GetRequiredService<IBookRepository>();

			var exists = bookRepository.IsIsbnExistAsync(isbn).GetAwaiter().GetResult();

			return exists
				? new ValidationResult(ErrorMessage ?? "ISBN already exists.")
				: ValidationResult.Success;
		}
	}
}
