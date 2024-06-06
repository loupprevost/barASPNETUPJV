using Bar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ProjetBar.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ProjetBar.Data.ApplicationDbContext _context;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async void OnGet()
        {
            if (User.Identity?.Name != "" && User.Identity?.Name != null)
            {
                TempData["userID"] = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                TempData["userName"] = User.Identity.Name;
                TempData["userRole"] = User.FindFirstValue(ClaimTypes.Role);
                TempData["userEmail"] = User.FindFirstValue(ClaimTypes.Email);
            }
            else
            {
                TempData["userID"] = "";
                TempData["userName"] = "";
                TempData["userRole"] = "";
                TempData["userEmail"] = "";
            }

        }
    }
}