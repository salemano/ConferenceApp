using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web;
using System.Web.Security;

namespace ConferenceApp.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class EditProfileModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public string PhotoThumbnail { get; set; }
        [Display(Name = "Photo")]
        public int? PhotoId { get; set; }

        [Display(Name = "Date of birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.mm.yy}")]
        public Nullable<DateTime> DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Comment")]
        [DataType(DataType.Text)]
        public string Comment { get; set; }

    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public string PhotoThumbnail { get; set; }
        [Display(Name = "Photo")]
        public int? PhotoId { get; set; }
        public int? FileDataId { get; set; }

        [Display(Name = "Date of birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.mm.yy}")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Comment")]
        [DataType(DataType.Text)]
        public string Comment { get; set; }

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

    public class RequestPasswordResetPageModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class PasswordResetModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public int MinimumPasswordLength { get; set; }
        public int UserId { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
