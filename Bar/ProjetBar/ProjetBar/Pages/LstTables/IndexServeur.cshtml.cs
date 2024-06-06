using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bar.Models;
using ProjetBar.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.Design;

namespace ProjetBar.Pages.LstTables
{
    [Authorize(Roles = "Serveur")]
    public class IndexServeurModel : PageModel
    {
        private readonly ProjetBar.Data.ApplicationDbContext _context;
        public Commande commandeASuppr;
        public IndexServeurModel(ProjetBar.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<LstTable> LstTable { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.LstTable != null)
            {
                int userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                Serveur serveur = await _context.Serveur.FirstOrDefaultAsync(s => s.UserConcernID == userID);

                LstTable = await _context.LstTable
                .Include(c => c.ServeurConcern)
                .Where(c => c.ServeurID == serveur.ID)
                .OrderBy(l => l.TablNum)
                .ToListAsync();
            }
        }
        public async Task<IActionResult> OnPostNouvelleCommandeAsync(int TableID)
        {
            int userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Serveur serveur = await _context.Serveur.FirstOrDefaultAsync(s => s.UserConcernID == userID);
            LstTable Table= await _context.LstTable.FirstOrDefaultAsync(c => c.ID == TableID);

            var newCommande = new Commande { CoPayee = false, TableConcernID = TableID, TableConcern = Table, ServeurConcernID = serveur.ID, ServeurConcern = serveur};
            _context.Commande.Add(newCommande);
            _context.SaveChanges();

            Table.TablEtat = 1;
            _context.Update(Table);
            await _context.SaveChangesAsync();

            Commande commande = await _context.Commande.Where(c => c.CoPayee != true).FirstOrDefaultAsync(c => c.TableConcernID == TableID);

            return RedirectToPage("../CoContenus/ListContenuCommande", new { id =  commande.ID});
        }

        public async Task<IActionResult> OnPostContinuerCommandeAsync(int TableID)
        {
            Commande commande = await _context.Commande.Where(c => c.CoPayee != true).FirstOrDefaultAsync(c => c.TableConcernID == TableID);

            return RedirectToPage("../CoContenus/ListContenuCommande", new { id = commande.ID });
        }

        public async Task<IActionResult> OnPostAnnulerCommandeAsync(int TableID)
        {
            LstTable Table = await _context.LstTable.FirstOrDefaultAsync(c => c.ID == TableID);
            Table.TablEtat = 0;
            _context.Update(Table);
            await _context.SaveChangesAsync();

            var commande = await _context.Commande.Where(c => c.CoPayee != true).FirstOrDefaultAsync(c => c.TableConcernID == TableID);
            commandeASuppr = commande;
            _context.Commande.Remove(commandeASuppr);
            await _context.SaveChangesAsync();
            return RedirectToPage("./IndexServeur", new { id = commande.ID });
        }
        public async Task<IActionResult> OnPostServirCommandeAsync(int TableID)
        {
            LstTable Table = await _context.LstTable.FirstOrDefaultAsync(c => c.ID == TableID);
            Table.TablEtat = 5;
            _context.Update(Table);
            await _context.SaveChangesAsync();

            return RedirectToPage("../LstTables/IndexServeur");
        }
    }
}
