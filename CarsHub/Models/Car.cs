using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarsHub.Models
{
    public class Car
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CarId { get; set; }

        [Display(Name="Car Brand")]
        public int Brand_Id { get; set; }

        [Required, Display(Name = "Car Model")]
        public string CarModel { get; set; }

        [Required, Display(Name = "Milage")]
        public string Milage { get; set; }

        [Required, Display(Name = "Year")]
        public string Year { get; set; }

        [Required, Display(Name = "Condition")]
        public string Condition { get; set; }

           

        
        [Display(Name="Front View")]
        public string FrontView { get; set; }
        [Display(Name = "Price")]
        public decimal Price { get; set; }
       
        

        public int DealerId { get; set; }
        public virtual Dealer Dealer { get; set; }

        public bool Book { get; set; }

        public bool Approve { get; set; }

        //Booker Details
        [Display(Name = "Mobile Phone")]
        public string MobileNo { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Home Address")]
        public string HomeAddress { get; set; }
        public virtual Brand Brand { get; set; }

       

        public DateTime ActualTime { get; set; }

        public DateTime? BookedDate { get; set; }

        

        
        public int? CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}