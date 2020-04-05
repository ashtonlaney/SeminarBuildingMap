using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using SeminarBuildingMap.Areas.Identity.Data;
using SeminarBuildingMap.GenericClasses;

namespace SeminarBuildingMap.Areas.Admin.Pages
{
    [Authorize(Roles="Admin,Manager")]
    public class EditOwnersModel : PageModel
    {

        private readonly UserManager<SeminarBuildingMapUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOptions<ConnectionConfig> _connectionConfig;
        private readonly Models.RoomDataAccessLayer ObjRoom = new Models.RoomDataAccessLayer();
        public IQueryable<SeminarBuildingMapUser> allUsers { get; set; }
        public IQueryable<SeminarBuildingMapUser> ownedUsers { get; set; }
        [BindProperty]
        public string UsersToAdd { get; set; }
        [BindProperty]
        public string UsersToDelete { get; set; }
        public EditOwnersModel(UserManager<SeminarBuildingMapUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<ConnectionConfig> connectionConfig)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _connectionConfig = connectionConfig;
        }

        public void OnGet(int id)
        {
            allUsers = _userManager.GetUsersInRoleAsync("Manager").Result.ToList().AsQueryable();
            allUsers = allUsers.Union(_userManager.GetUsersInRoleAsync("Faculty").Result.ToList().AsQueryable());
            ownedUsers = ObjRoom.GetOwnedUsers(id, _connectionConfig.Value.ConnStr);
           
        }

        public void OnPostSave(int id)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(UsersToAdd) && ObjRoom.UserCanEditRoom(User.Identity.Name, id, _connectionConfig.Value.ConnStr))
            {
                var users = UsersToAdd.Split(",");
                foreach (string item in users)
                {
                    var email = item.Trim();
                    if (!string.IsNullOrEmpty(email)) { 
                        var user = _userManager.FindByEmailAsync(email).Result;
                        if (user != null && !_userManager.IsInRoleAsync(user, "Admin").Result)
                        {
                            ObjRoom.AddOwnedRooms(id, email, _connectionConfig.Value.ConnStr);
                        } 
                        else
                        {
                            ModelState.AddModelError(string.Empty, "User: \"" + item + "\" is either not a valid user or an Admin and cannot be added");
                        }
                    }
                }
            } else
            {
                ModelState.AddModelError(string.Empty, "Please enter at least 1 user to add");
            }
            allUsers = _userManager.GetUsersInRoleAsync("Manager").Result.ToList().AsQueryable();
            allUsers = allUsers.Union(_userManager.GetUsersInRoleAsync("Faculty").Result.ToList().AsQueryable());
            ownedUsers = ObjRoom.GetOwnedUsers(id, _connectionConfig.Value.ConnStr);
        }
        public void OnPostDelete(int id)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(UsersToDelete) && ObjRoom.UserCanEditRoom(User.Identity.Name, id, _connectionConfig.Value.ConnStr))
            {
                var users = UsersToDelete.Split(",");
                foreach (string item in users)
                {
                    var email = item.Trim();
                    if (!string.IsNullOrEmpty(email))
                    {
                        ObjRoom.DeleteOwnedRooms(id, email, _connectionConfig.Value.ConnStr);
                    } else
                    {
                        ModelState.AddModelError(string.Empty, "Error deleting supplied users, please check for proper input");
                    }
                }
            } else
            {
                ModelState.AddModelError(string.Empty, "Please enter at least 1 user to delete");
            }
            allUsers = _userManager.GetUsersInRoleAsync("Manager").Result.ToList().AsQueryable();
            allUsers = allUsers.Union(_userManager.GetUsersInRoleAsync("Faculty").Result.ToList().AsQueryable());
            ownedUsers = ObjRoom.GetOwnedUsers(id, _connectionConfig.Value.ConnStr);
        }
    }
}
