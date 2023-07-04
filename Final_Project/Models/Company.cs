﻿using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        [Display(Name = "Logo")]
        public byte[] Logo { get; set; }
        [Display(Name = "Company Name")]
        [MinLength(6)]
        public string CompanyName { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Address")]
        [MinLength(6)]
        public string Address { get; set; }
        [Display(Name = "Commercial Record")]
        public byte[] Doc1 { get; set; }
        [Display(Name = "Tax card")]
        public byte[] Doc2 { get; set; }

        public bool Trusted { get; set; }


        public List<Job> Jobs { get; set; }

    }
}
