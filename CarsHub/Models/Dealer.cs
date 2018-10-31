using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarsHub.Models
{
    public class Dealer
    {
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Key]
        public int DealerId { get; set; }
        [Required]
        [Display(Name="Company Name")]
        public string CompanyName { get; set; }
        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Required]
        [Display(Name = "Province")]
        public string Province { get; set; }
        [Required]
        [Display(Name = "Company Address")]
        public string CompanyAddress { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Required, DataType(DataType.PhoneNumber)]
        [Display(Name = "Office Contact")]
        public string OfficePhoneNo { get; set; }

        public string RegistrationNo { get; set; }

        public string Password { get; set; }
       

        

    }
}