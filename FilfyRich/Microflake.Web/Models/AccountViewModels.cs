using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microflake.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        public string Address { get; set; }
        [StringLength(15, ErrorMessage = "The FirstName field should be less then 15 letters")]
        public string FirstName { get; set; }

        [StringLength(15, ErrorMessage = "The LastName field should be less then 15 letters")]
        public string LastName { get; set; }

        public string  City { get; set; }

        public string  State { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Country { get; set; }

        public string Role { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }

    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "The FirstName field is required")]
        [StringLength(15, ErrorMessage = "The FirstName field should be less then 15 letters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The LastName field is required")]
        [StringLength(15, ErrorMessage = "The LastName field should be less then 15 letters")]
        public string LastName { get; set; }
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }
        public bool Status { get; set; }
    }

    public class EditAppUserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        [Required]
        public bool Status { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
