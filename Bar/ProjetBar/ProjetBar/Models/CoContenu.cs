using System;
using System.ComponentModel.DataAnnotations;

namespace Bar.Models
{
    public class CoContenu
    {
        public int ID { get; set; }
        public int Paye { get; set; }

        [Required]
        [Display(Name = "Article à ajouter")]
        public int LArticleID { get; set; }
        public Article? LArticle { get; set; }
        public int LaCommandeID { get; set; }
        public Commande? LaCommande { get; set; }
    }
}
