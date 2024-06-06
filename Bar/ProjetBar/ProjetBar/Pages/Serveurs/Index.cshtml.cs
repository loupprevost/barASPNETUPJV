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

namespace ProjetBar.Pages.Serveurs
{
    [Authorize(Roles = "Administrateur,Serveur")]
    public class IndexModel : PageModel
    {
        private readonly ProjetBar.Data.ApplicationDbContext _context;

        public IndexModel(ProjetBar.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Serveur> Serveur { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Serveur != null)
            {
                Serveur = await _context.Serveur.ToListAsync();
            }
        }
    }
}
