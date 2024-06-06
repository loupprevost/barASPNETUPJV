using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Bar.Models
{
    public class Serveur
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Nom du Serveur")]
        public string Nom { get; set; }

        [Required]
        [Display(Name = "Prenom du Serveur")]
        public string Prenom { get; set; }
        public int UserConcernID { get; set; }

        [Display(Name = "Serveur Associé")]
        public string NomComplet
        { 
            get 
            {
                return Nom + " " + Prenom;
            }
        }
    }
}
