using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bar.Models;
using ProjetBar.Data;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ProjetBar.Pages.CoContenus
{
    [Authorize(Roles = "Serveur")]
    public class CreateModel : PageModel
    {
        private readonly ProjetBar.Data.ApplicationDbContext _context;

        public CreateModel(ProjetBar.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
            TempData["CommandeIDConcern"] = id;
            ViewData["LArticleID"] = new SelectList(_context.Article, "ID", "ArtNom");
            ViewData["LaCommandeID"] = new SelectList(_context.Commande, "ID", "ID");
            return Page();
        }

        [BindProperty]
        public CoContenu CoContenu { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.CoContenu == null || CoContenu == null)
            {
                return Page();
            }

            _context.CoContenu.Add(CoContenu);
            await _context.SaveChangesAsync();

            ViewData["LaCommandeID"] = new SelectList(_context.Commande, "ID", "ID");

            return RedirectToPage("./ListContenuCommande", new { id = TempData.Peek("CommandeIDConcern") });
        }
    }
}
