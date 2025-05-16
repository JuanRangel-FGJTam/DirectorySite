
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using DirectorySite.Data;
using DirectorySite.Services;
using DirectorySite.Models;

namespace DirectorySite.Controllers
{
    [Auth]
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly UserService userService;
        private readonly ILogger<ProfileController> logger;

        public ProfileController(UserService userService, ILogger<ProfileController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            // * current user
            var userId = User.Claims.FirstOrDefault( c => c.Type == "userId")?.Value;
            if(userId == null)
            {
                return RedirectToAction("Index", "Authentication");
            }
            
            var userInfo = await this.userService.GetUserData(int.Parse(userId));
            if(userInfo == null)
            {
                return RedirectToAction("Index", "Authentication");
            }

            ViewBag.Title = "Perfil: " + userInfo.FullName;
            
            ViewBag.Error = TempData["Error"];
            ViewBag.Success = TempData["Success"];
            return View(userInfo);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdatedPassword([FromForm] int userId, [FromForm] string oldPassword, [FromForm] string password, [FromForm] string confirmPassword)
        {
            if (password != confirmPassword)
            {
                TempData["Error"] = "Las contraseñas no coinciden.";
                return RedirectToAction("Index");
            }

            if(password.Length <= 8)
            {
                TempData["Error"] = "La contraseña debe tener al menos 8 caracteres.";
                return RedirectToAction("Index");
            }

            try
            {
                await this.userService.UpdateCredentials(userId, new UserUpdateCredentialsRequest
                {
                    Id = userId,
                    Password = password,
                    ConfirmPassword = confirmPassword
                });

                TempData["Success"] = "Contraseña actualizada";
                return RedirectToAction("Index");

            }
            catch(Exception err)
            {
                TempData["Error"] = err.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
