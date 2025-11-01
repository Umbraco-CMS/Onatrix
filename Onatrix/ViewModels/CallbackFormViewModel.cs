using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Onatrix.ViewModels;

public class CallbackFormViewModel
{
    [Required(ErrorMessage = "Name is required.")]
    [Display(Name = "Name")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [Display(Name = "Email address")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Phone number is required.")]
    [Display(Name = "Phone number")]
    public string Phone { get; set; } = null!;

    [Required(ErrorMessage = "Please select an option.")]
    public string SelectedOption { get; set; } = null!;

    [BindNever]
    public IEnumerable<string> Options { get; set; } = [];

}
