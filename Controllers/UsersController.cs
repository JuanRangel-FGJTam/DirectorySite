using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using DirectorySite.Data;
using DirectorySite.Models;
using DirectorySite.Services;
using DirectorySite.Exceptions;

namespace DirectorySite.Controllers
{

    [Auth]
    [Admin]
    [Route("[controller]")]
    public class UsersController(ILogger<UsersController> logger, UserService us, CatalogService cs) : Controller
    {
        private readonly ILogger<UsersController> _logger = logger;
        private readonly UserService userService = us;
        private readonly CatalogService catalogService = cs;
        
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
            
            ViewBag.Roles = await catalogService.GetRoles();
            
            // return the view
            ViewData["Title"] = $"Usuario {userData!.FirstName?.ToLower()} {userData!.LastName?.ToLower()}";
            return View(userData);
        }

        [Route("{userId}/edit")]
        public async Task<IActionResult> EditUser([FromRoute] int userId)
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

            var rolesAvaliables = await this.catalogService.GetRoles();

            // verify if the user is null
            if(userData == null)
            {
                ViewBag.NotFoundMessage = "El usuario no se encuntra registrada en el sistema";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            // prepare the updateRequest models
            ViewBag.UserUpdateGeneralsRequest = new UserUpdateGeneralsRequest {
                FirstName = userData.FirstName?.Trim(),
                LastName = userData.LastName?.Trim(),
                Email = userData.Email?.Trim(),
            };
            ViewBag.UserUpdateCredentialsRequest = new UserUpdateCredentialsRequest();
            ViewBag.UpdateRolesRequest = new UpdateRolesRequest {
                Roles = (userData.UserRoles??[]).Select( item => item.RoleId).ToList(),
                RolesAvailables = rolesAvaliables!.Select( item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(item.Name, item.Id.ToString()))
            };
            
            // return the view
            ViewData["Title"] = $"Editando {userData!.FirstName?.ToLower()} {userData!.LastName?.ToLower()}";
            return View(userData);
        }


        [HttpPatch]
        [Route("{userId}/generals")]
        [Consumes(MediaTypeNames.Application.FormUrlEncoded)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> UpdateGeneralData([FromRoute] int userId, [FromForm] UserUpdateGeneralsRequest request)
        {
            // TODO: Validate the request

            // * attempt to get the user data
            UserResponse? userData = null;
            try
            {
                userData = await userService.GetUserData(userId);
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch(Exception err)
            {
                this._logger.LogError(err, "Fail at get the data of the user '{personId}'", userId);
                return Conflict( new {
                    Message = "Error al obtene los datos del usuario."
                });
            }

            // * update the person
            try
            {
                var response = await userService.UpdateUserGenerals(userId, request);
                if(response == 1)
                {
                    return Ok();
                }else
                {
                    return Conflict();
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Conflict();
            }
            catch (ArgumentException)
            {
                return Conflict();
            }
            catch (InvalidDataException)
            {
                return Conflict();
            }
            
        }


        [HttpPatch]
        [Route("{userId}/credentials")]
        [Consumes(MediaTypeNames.Application.FormUrlEncoded)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> UpdateCredentials([FromRoute] int userId, [FromForm] UserUpdateCredentialsRequest request)
        {
            // TODO: Validate the request
            
            // * attempt to get the user data
            UserResponse? userData = null;
            try
            {
                userData = await userService.GetUserData(userId);
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch(Exception err)
            {
                this._logger.LogError(err, "Fail at get the data of the user '{personId}'", userId);
                return Conflict( new {
                    Message = "Error al obtene los datos del usuario."
                });
            }


            // * update the person
            try
            {
                var response = await userService.UpdateCredentials(userId, request);
                if(response == 1)
                {
                    return Ok();
                }else
                {
                    return Conflict();
                }
            }
            catch(ArgumentException ve)
            {
                return UnprocessableEntity( new {
                    ve.Message
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Conflict();
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        [HttpPatch]
        [Route("{userId}/roles")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> UpdateUserRoles([FromRoute] int userId, [FromForm] int roleId, [FromForm] int value)
        {
            // * attempt to get the user data
            UserResponse? userData = null;
            try
            {
                userData = await userService.GetUserData(userId);
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch(Exception err)
            {
                this._logger.LogError(err, "Fail at get the data of the user '{personId}'", userId);
                return Conflict( new {
                    Message = "Error al obtene los datos del usuario."
                });
            }
            
            this._logger.LogInformation("{role} a {value}", roleId, value);
            // * update the person
            try
            {
                var response = await userService.AttachRole(userId, roleId, value == 1);
                if(response == 1)
                {
                    return Ok();
                }else
                {
                    return Conflict();
                }
            }
            catch (UnauthorizedAccessException ua)
            {
                return Conflict(ua.Message);
            }
            catch (Exception err)
            {
                return Conflict(err.Message);
            }
        }


        [HttpGet("/new")]
        public IActionResult NewUser()
        {
            var request = new NewUserRequest();
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> StoreUser(NewUserRequest request)
        {

            // TODO: Validate request
            bool valid = true;
            if(!valid)
            {
                return UnprocessableEntity();
            }


            // * store the new user
            try
            {
                var user = await this.userService.StoreNewUser(request);
            }
            catch (Exception e)
            {
                // Add a custom error message
                ViewBag.ErrorMessage = e.Message;
                return View("~/Views/Users/NewUser.cshtml");
            }

            return RedirectToAction("Index");
        }
    }
}