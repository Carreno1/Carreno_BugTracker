using Carreno_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace Carreno_BugTracker.ViewModel
{
    public class UserProfileVM
    {

        public string Id { get; set; } //current users Id

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Project> ProjectIn { get; set; } //Projects I am assigned to
        public ICollection<Project> ProjectOut { get; set; } //Projects I am not assigned to
        public ICollection<Ticket> TicketsIn { get; set; } //Tickets I am involved in
        public string AvatarPath { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Current Password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }




    }
}