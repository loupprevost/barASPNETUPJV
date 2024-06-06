using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Bar.Models
{
    public class LstTable
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Numero de la table")]
        public int TablNum { get; set; }
        public int? TablEtat { get; set; }
        [Required]
        [Display(Name = "Serveur Associé")]
        public int? ServeurID { get; set; }
        [Required]
        [Display(Name = "Numero de la table")]
        public Serveur? ServeurConcern { get; set; }
    }
}
