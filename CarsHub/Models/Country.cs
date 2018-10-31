using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarsHub.Models
{
    public class Country
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CountryId { get; set; }
        [Required, Display(Name="Country")]
        public string CountryName { get; set; }
        [Required, Display(Name="Country Code")]
        public string CountryCode { get; set; }
    }
}