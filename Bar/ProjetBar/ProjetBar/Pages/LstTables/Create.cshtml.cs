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

namespace ProjetBar.Pages.LstTables
{
    [Authorize(Roles = "Administrateur")]
    public class CreateModel : PageModel
    {
        private readonly ProjetBar.Data.ApplicationDbContext _context;

        public CreateModel(ProjetBar.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ServeurID"] = new SelectList(_context.Serveur, "ID", "NomComplet");
            return Page();
        }

        [BindProperty]
        public LstTable LstTable { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            _context.LstTable.Add(LstTable);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
