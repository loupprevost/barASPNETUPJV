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

namespace ProjetBar.Pages.Commandes
{
    [Authorize(Roles = "Barman")]
    public class IndexBarmanModel : PageModel
    {
        private readonly ProjetBar.Data.ApplicationDbContext _context;

        public IndexBarmanModel(ProjetBar.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Commande> Commande { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Commande != null)
            {
                int userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                Barman barman = await _context.Barman.FirstOrDefaultAsync(s => s.UserConcernID == userID);

                Commande = await _context.Commande
                .Include(c => c.ServeurConcern)
                .Include(c => c.TableConcern)
                .Where(c => (c.BarmanID == barman.ID || c.BarmanID == null) && c.CoPayee != true && (c.TableConcern.TablEtat == 2 || c.TableConcern.TablEtat == 3))
                .ToListAsync();
            }  
        }

        public async Task<IActionResult> OnPostPrendreCommandeAsync(int CommandeID)
        {
            int userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Barman barman = await _context.Barman.FirstOrDefaultAsync(s => s.UserConcernID == userID);

            Commande commande = await _context.Commande.FirstOrDefaultAsync(c => c.ID == CommandeID);
            commande.BarmanID = barman.ID;
            _context.Update(commande);
            await _context.SaveChangesAsync();

            LstTable Table = await _context.LstTable.FirstOrDefaultAsync(c => c.ID == commande.TableConcernID);
            Table.TablEtat = 3;
            _context.Update(Table);
            await _context.SaveChangesAsync();

            return RedirectToPage("./IndexBarman");
        }

        public async Task<IActionResult> OnPostVisuCommandeAsync(int CommandeID)
        {
            return RedirectToPage("./VisuCommande", new { id = CommandeID });
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
