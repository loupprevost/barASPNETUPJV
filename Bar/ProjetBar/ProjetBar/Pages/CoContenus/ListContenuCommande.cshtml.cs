using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bar.Models;
using ProjetBar.Data;
using System.ComponentModel.Design;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ProjetBar.Pages.CoContenus
{
    [Authorize(Roles = "Serveur")]
    public class ListContenuCommandeModel : PageModel
    {
        private readonly ProjetBar.Data.ApplicationDbContext _context;
        public ListContenuCommandeModel(ProjetBar.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public int CommandeID;
        [BindProperty]
        public IList<CoContenu> CoContenu { get;set; } = default!;
        public CoContenu CoContenuSuppr { get; set; } = default!;
        public Commande CommandeASuppr { get; set; } = default!;

        public async Task OnGetAsync(int id)
        {
            if (_context.CoContenu != null)
            {
                CommandeID = id;

                CoContenu = await _context.CoContenu
                .Include(c => c.LArticle)
                .Include(c => c.LaCommande)
                .Where(c => c.LaCommande.ID == id)
                .ToListAsync();
            }
        }
        public async Task<IActionResult> OnPostNouvelArticleAsync(int CommandeID)
        {
            return RedirectToPage("../CoContenus/Create", new { id = CommandeID });
        }
        public async Task<IActionResult> OnPostSupprArticleAsync(int CoContenuID, int CommandeID)
        {
            var cocontenu = await _context.CoContenu.FindAsync(CoContenuID);
            CoContenuSuppr = cocontenu;
            _context.CoContenu.Remove(CoContenuSuppr);
            await _context.SaveChangesAsync();
            return RedirectToPage("./ListContenuCommande", new { id = CommandeID });
        }
        public async Task<IActionResult> OnPostValiderCommandeAsync(int CommandeID)
        {
            Commande commande = await _context.Commande.FirstOrDefaultAsync(c => c.ID == CommandeID);
            LstTable table = await _context.LstTable.FirstOrDefaultAsync(c => c.ID == commande.TableConcernID);
            table.TablEtat = 2;
            _context.Update(table);
            await _context.SaveChangesAsync();

            int userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Serveur serveur = await _context.Serveur.FirstOrDefaultAsync(s => s.UserConcernID == userID);
            return RedirectToPage("../LstTables/IndexServeur", new { id = serveur.ID });
        }
    }
}
