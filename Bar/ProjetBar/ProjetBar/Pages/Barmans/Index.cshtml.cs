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

namespace ProjetBar.Pages.Barmans
{
    [Authorize(Roles = "Administrateur")]
    public class IndexModel : PageModel
    {
        private readonly ProjetBar.Data.ApplicationDbContext _context;

        public IndexModel(ProjetBar.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Barman> Barman { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Barman != null)
            {
                Barman = await _context.Barman.ToListAsync();
            }
        }
    }
}
