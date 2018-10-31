using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarsHub.Models
{
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CustomerId { get; set; }

        [Required,Display(Name="User Name")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, Display(Name = "Phone No")]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }
        [Required,Display(Name="Address")]
        public string Address { get; set; }
        [Display(Name="Country")]
        public int? CountryId { get; set; }
        [Display(Name = "Province")]
        public string State { get; set; }

        public virtual Country  Country { get; set; }

        //public virtual State  State { get; set; }

      
    }
}