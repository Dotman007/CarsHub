using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace CarsHub.Models
{
    public class Brand
    {
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Key]
        public int Brand_Id { get; set; }
        [Required,Display(Name="Brand Name")]
        public string Brand_Name { get; set; }

    }
}