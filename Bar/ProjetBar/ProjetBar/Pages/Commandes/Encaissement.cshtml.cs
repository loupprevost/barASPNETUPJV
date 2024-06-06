using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bar.Models;
using ProjetBar.Data;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ProjetBar.Pages.CoContenus
{
    [Authorize(Roles = "Caissier")]
    public class EncaissementModel : PageModel
    {
        private readonly ProjetBar.Data.ApplicationDbContext _context;

        public EncaissementModel(ProjetBar.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CoContenu> CoContenu { get; set; } = default!;
        public IList<CoContenu> CoContenuPaye { get; set; } = default!;
        public int CommandeTerminee=1;
        public decimal prixTotal = 0;
        public decimal prixRestant = 0;
        public async Task OnGetAsync(int id)
        {
            if (_context.CoContenu != null)
            {
                CoContenu = await _context.CoContenu
                .Include(c => c.LArticle)
                .Include(c => c.LaCommande)
                .Where(c => (c.LaCommandeID == id))
                .ToListAsync();

                foreach (var item in CoContenu)
                {
                    if (item.Paye == 0)
                    {
                        CommandeTerminee = 0;
                        break;
                    }
                }

                foreach (var item in CoContenu)
                {
                    prixTotal += decimal.Parse(item.LArticle.ArtPrix.Replace(".",","));
                }

                if (CommandeTerminee == 1)
                {
                    Commande commande = await _context.Commande.FirstOrDefaultAsync(c => c.ID == id);
                    commande.CoPayee = true;
                    _context.Update(commande);

                    LstTable table = await _context.LstTable.FirstOrDefaultAsync(c => c.ID == commande.TableConcernID);
                    table.TablEtat = 0;
                    _context.Update(table);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    prixRestant = prixTotal;

                    CoContenuPaye = await _context.CoContenu
                    .Include(c => c.LArticle)
                    .Include(c => c.LaCommande)
                    .Where(c => (c.LaCommandeID == id) && (c.Paye == 1))
                    .ToListAsync();

                    foreach (var item in CoContenuPaye)
                    {
                        prixRestant -= decimal.Parse(item.LArticle.ArtPrix.Replace(".", ","));
                    }
                }
            }
        }

        public async Task<IActionResult> OnPostEncaisserArticleAsync(int CommandeID, int CoContenuID)
        {
            CoContenu coContenu = await _context.CoContenu.FirstOrDefaultAsync(c => c.ID == CoContenuID);
            coContenu.Paye = 1;
            _context.Update(coContenu);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Encaissement", new { id = CommandeID });
        }
    }
}
