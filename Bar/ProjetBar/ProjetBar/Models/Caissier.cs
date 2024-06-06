using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Bar.Models
{
    public class Caissier
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Nom du Caissier")]
        public string Nom { get; set; }

        [Required]
        [Display(Name = "Prenom du Caissier")]
        public string Prenom { get; set; }

        public int UserConcernID { get; set; }

        public string NomComplet
        { 
            get 
            {
                return Nom + " " + Prenom;
            }
        }
    }
}
