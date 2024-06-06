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
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ProjetBar.Pages.Commandes
{
    [Authorize(Roles = "Caissier")]
    public class IndexCaissierModel : PageModel
    {
        private readonly ProjetBar.Data.ApplicationDbContext _context;

        public IndexCaissierModel(ProjetBar.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Commande> Commande { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Commande != null)
            { 
                int userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                Caissier caissier = await _context.Caissier.FirstOrDefaultAsync(s => s.UserConcernID == userID);

                Commande = await _context.Commande
                .Include(c => c.ServeurConcern)
                .Include(c => c.TableConcern)
                .Where(c => c.CoPayee != true && c.TableConcern.TablEtat == 5 && (c.CaissierID == null || c.CaissierID == caissier.ID))
                .ToListAsync();
            }  
        }

        public async Task<IActionResult> OnPostEncaisserCommandeAsync(int CommandeID)
        {
            int userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Caissier caissier = await _context.Caissier.FirstOrDefaultAsync(s => s.UserConcernID == userID);

            Commande commande = await _context.Commande.FirstOrDefaultAsync(c => c.ID == CommandeID);
            commande.CaissierID = caissier.ID;
            _context.Update(commande);
            await _context.SaveChangesAsync();

            System.Diagnostics.Debug.WriteLine("id index caissier:" + CommandeID);

            return RedirectToPage("./Encaissement", new { id = CommandeID });
        }
    }
}
