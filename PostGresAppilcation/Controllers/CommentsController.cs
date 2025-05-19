using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostGresAppilcation.Data;
using PostGresAppilcation.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PostGresAppilcation.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CommentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var comments = await dbContext.Comments.OrderByDescending(c => c.CreatedAt).ToListAsync();
            return View(comments);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string text)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
            {
                return Forbid();
            }
            var comment = new Comment
            {
                Text = text,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UserName = User.Identity?.Name ?? User.FindFirst("preferred_username")?.Value ?? "Unknown"
            };
           
            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index","Comments");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var comment = await dbContext.Comments.FindAsync(id);

            if (comment == null|| comment.UserId != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
            {
                return Forbid();
            }

            return View(comment);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(int id,string text)
        {
            var comment = await dbContext.Comments.FindAsync(id);

            if(comment == null|| comment.UserId != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
            {
                return Forbid();
            }
            if (string.IsNullOrEmpty(text))
            {
                return RedirectToAction("Index", "Comments");
            }
            comment.Text = text;
            comment.CreatedAt = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await dbContext.Comments.FindAsync(id);
            if (comment == null || comment.UserId != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
            {
                return Forbid();
            }
            dbContext.Comments.Remove(comment);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Comments");
        }
    }
}
