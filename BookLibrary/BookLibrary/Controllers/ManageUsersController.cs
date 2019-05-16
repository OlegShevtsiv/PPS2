using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Models;
using BookLibrary.ViewModels.ManageUsers;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Filters;

namespace BookLibrary.Controllers
{
    [Authorize(Roles = "user admin")]
    public class ManageUsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICommentService _commentService;
        public ManageUsersController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ICommentService commentService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _commentService = commentService;
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Error");
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, LoginName = user.UserName };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.UserName = model.LoginName;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return RedirectToAction("Error");
                }
            }
            foreach (var c in _commentService.Get(new CommentFilterByOwnerId { OwnerId = id }))
            {
                _commentService.Remove(c.Id);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditUserRoles(string userId)
        {
            // get user
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // gets user`s roles list
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return RedirectToAction("Error");
        }
        [HttpPost]
        public async Task<IActionResult> EditUserRoles(string userId, List<string> roles)
        {
            // gets user
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // gets user`s roles list
                var userRoles = await _userManager.GetRolesAsync(user);
                // gets all roles
                var allRoles = _roleManager.Roles.ToList();
                // gets roles list, that was added 
                var addedRoles = roles.Except(userRoles);
                // gets roles that was deleted
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("Index");
            }

            return RedirectToAction("Error");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}