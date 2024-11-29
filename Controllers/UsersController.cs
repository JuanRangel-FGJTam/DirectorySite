using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DirectorySite.Data;
using DirectorySite.Models;
using DirectorySite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DirectorySite.Controllers
{

    [Auth]
    [Route("[controller]")]
    public class UsersController(ILogger<UsersController> logger, UserService us) : Controller
    {
        private readonly ILogger<UsersController> _logger = logger;
        private readonly UserService userService = us;
        
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Usuarios";

            IEnumerable<UserResponse>? usersData = null;
            try
            {
                usersData = await userService.GetUsers();
            }
            catch (UnauthorizedAccessException uae)
            {
                this._logger.LogDebug(uae.Message);
                throw;
            }
            catch (Exception err)
            {
                this._logger.LogDebug(err.Message);
                throw;
            }

            // verify if the users is null
            if(usersData == null)
            {
                ViewData["ErrorMessage"] = "No se pudo cargar los usuarios";
            }
            
            // return the view
            return View(usersData);
        }

        [Route("{userId}")]
        public async Task<IActionResult> ShowUser([FromRoute] int userId)
        {
            UserResponse? userData = null;
            try
            {
                userData = await userService.GetUserData(userId);
            }
            catch (UnauthorizedAccessException uae)
            {
                this._logger.LogDebug(uae.Message);
                throw;
            }
            catch (Exception err)
            {
                this._logger.LogDebug(err.Message);
                throw;
            }

            // verify if the user is null
            if(userData == null)
            {
                ViewBag.NotFoundMessage = "El usuario no se encuntra registrada en el sistema";
                return View("~/Views/Shared/NotFound.cshtml");   
            }
            
            // return the view
            ViewData["Title"] = $"Usuario {userData!.FirstName?.ToLower()} {userData!.LastName?.ToLower()}";
            return View(userData);
        }

    }
}