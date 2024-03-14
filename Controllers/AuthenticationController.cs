using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DirectorySite.Models.ViewModel;
using DirectorySite.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectorySite.Controllers
{
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly AuthenticationService authenticationService;

        public AuthenticationController(ILogger<AuthenticationController> logger, AuthenticationService authenticationService )
        {
            _logger = logger;
            this.authenticationService = authenticationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login( LoginRequest loginRequest)
        {
            var token = authenticationService.Authenticate( loginRequest, out string errorMessage);
            if( token == null){
                // Display error message or redirect back to login page
                loginRequest.ErrorMessage = errorMessage;
                return View("Index", loginRequest );
            }

            // Store token securely, e.g., in session
            HttpContext.Session.SetString( "JWTToken", token);
            
            // Redirect to a secure page
            return RedirectToAction("Index", "Home");

        }
        
    }
}