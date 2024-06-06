using System;
using System.ComponentModel.DataAnnotations;

namespace Bar.Models
{
    public class Article
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Nom de l'article")]
        public string ArtNom { get; set; }

        [Required]
        [Display(Name = "Prix de l'article")]
        public string ArtPrix { get; set; }
    }
}
