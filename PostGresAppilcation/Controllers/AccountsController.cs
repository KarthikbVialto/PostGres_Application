using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using PostGresAppilcation.Models;
namespace PostGresAppilcation.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        public IActionResult Index()
        {
            var claims = User.Claims.Select(x => new ClaimViewModel
            {
                Type = x.Type,
                Value = x.Value
            }).ToList();
            return View(claims);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties
            {
                RedirectUri = "/"
            }, "cookie", "oidc");
        }
    }
}
