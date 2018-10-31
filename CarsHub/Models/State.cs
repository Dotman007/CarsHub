using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace CarsHub.Models
{
    public class State
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int StateId { get; set; }
        public int CountryId { get; set; }
        [Required,Display(Name="Province")]
        public string StateName { get; set; }
        [Required, Display(Name = "Code")]
        public string StateCode { get; set; }

        public virtual Country Country { get; set; }
    }
}