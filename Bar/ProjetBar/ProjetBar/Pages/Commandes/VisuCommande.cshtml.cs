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

namespace ProjetBar.Pages.Commandes
{
    [Authorize(Roles = "Barman")]
    public class VisuCommandeModel : PageModel
    {
        private readonly ProjetBar.Data.ApplicationDbContext _context;
        public VisuCommandeModel(ProjetBar.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public int CommandeID;
        [BindProperty]
        public IList<CoContenu> CoContenu { get;set; } = default!;

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
        public async Task<IActionResult> OnPostActionRetourAsync()
        {
            return RedirectToPage("./IndexBarman");
        }

        public async Task<IActionResult> OnPostPreteCommandeAsync(int CommandeID)
        {
            Commande commande = await _context.Commande.FirstOrDefaultAsync(c => c.ID == CommandeID);

            LstTable Table = await _context.LstTable.FirstOrDefaultAsync(c => c.ID == commande.TableConcernID);
            Table.TablEtat = 4;
            _context.Update(Table);
            await _context.SaveChangesAsync();

            return RedirectToPage("./IndexBarman");
        }
    }
}
