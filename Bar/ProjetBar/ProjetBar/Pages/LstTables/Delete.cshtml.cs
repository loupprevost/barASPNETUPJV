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

namespace ProjetBar.Pages.LstTables
{
    [Authorize(Roles = "Administrateur")]
    public class DeleteModel : PageModel
    {
        private readonly ProjetBar.Data.ApplicationDbContext _context;

        public DeleteModel(ProjetBar.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public LstTable LstTable { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.LstTable == null)
            {
                return NotFound();
            }

            var lsttable = await _context.LstTable.FirstOrDefaultAsync(m => m.ID == id);

            if (lsttable == null)
            {
                return NotFound();
            }
            else 
            {
                LstTable = lsttable;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.LstTable == null)
            {
                return NotFound();
            }
            var lsttable = await _context.LstTable.FindAsync(id);

            if (lsttable != null)
            {
                LstTable = lsttable;
                _context.LstTable.Remove(LstTable);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
