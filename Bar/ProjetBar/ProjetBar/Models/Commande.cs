using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Bar.Models
{
    public class Commande
    {
        public int ID { get; set; }
        public bool CoPayee { get; set; }
        public int? BarmanID { get; set; }
        public Barman? BarmanConcern { get; set; }
        public int? CaissierID { get; set; }
        public Caissier? CaissierConcern { get; set; }
        public int? TableConcernID { get; set; }
        // Lien de navigation
        public LstTable? TableConcern { get; set; }
        public int? ServeurConcernID { get; set; }
        // Lien de navigation
        public Serveur? ServeurConcern { get; set; }
    }
}
