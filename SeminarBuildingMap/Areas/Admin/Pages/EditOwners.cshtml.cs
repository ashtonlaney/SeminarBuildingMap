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
    //only Admins and Managers access this page
    [Authorize(Roles="Admin,Manager")]
    public class EditOwnersModel : PageModel
    {

        private readonly UserManager<SeminarBuildingMapUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOptions<ConnectionConfig> _connectionConfig;
        private readonly Models.RoomDataAccessLayer ObjRoom = new Models.RoomDataAccessLayer();
        public IQueryable<SeminarBuildingMapUser> allUsers { get; set; }
        public IQueryable<SeminarBuildingMapUser> ownedUsers { get; set; }

        public string RoomName { get; set; }

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

        public void OnGet(int id) //display all faculty and managers in the datagrid
        {
            allUsers = _userManager.GetUsersInRoleAsync("Manager").Result.ToList().AsQueryable();
            allUsers = allUsers.Union(_userManager.GetUsersInRoleAsync("Faculty").Result.ToList().AsQueryable()); //union the datasets together so it can list all editable users
            ownedUsers = ObjRoom.GetOwnedUsers(id, _connectionConfig.Value.ConnStr);
            RoomName = ObjRoom.GetRoomInfo(id, _connectionConfig.Value.ConnStr).rmName;

        }

        public void OnPostSave(int id)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(UsersToAdd) && ObjRoom.UserCanEditRoom(User.Identity.Name, id, _connectionConfig.Value.ConnStr)) //check user has permissions to edit room
            {
                var users = UsersToAdd.Split(","); //split user box on comma
                foreach (string item in users) //iterate through each user
                {
                    var email = item.Trim();
                    if (!string.IsNullOrEmpty(email)) { 
                        var user = _userManager.FindByEmailAsync(email).Result; //find the user based on entered email
                        if (user != null && !_userManager.IsInRoleAsync(user, "Admin").Result) //make sure an admin isn't entered and that a valid user is selected
                        {
                            ObjRoom.AddOwnedRooms(id, email, _connectionConfig.Value.ConnStr);  //authorize the user to edit the selected room                            
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
            RoomName = ObjRoom.GetRoomInfo(id, _connectionConfig.Value.ConnStr).rmName;
            UsersToAdd = "";
            UsersToDelete = "";
            if(ModelState.ErrorCount == 0)
            {
                ModelState.Clear();
            }
        }
        public void OnPostDelete(int id) //delete user from room's authorized list
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(UsersToDelete) && ObjRoom.UserCanEditRoom(User.Identity.Name, id, _connectionConfig.Value.ConnStr))
            {
                var users = UsersToDelete.Split(",");
                foreach (string item in users)
                {
                    var email = item.Trim();
                    if (!string.IsNullOrEmpty(email))
                    {
                        ObjRoom.DeleteOwnedRooms(id, email, _connectionConfig.Value.ConnStr); //call delete function for user
                    }
                }
            } else
            {
                ModelState.AddModelError(string.Empty, "Please enter at least 1 user to delete");
            }
            allUsers = _userManager.GetUsersInRoleAsync("Manager").Result.ToList().AsQueryable();
            allUsers = allUsers.Union(_userManager.GetUsersInRoleAsync("Faculty").Result.ToList().AsQueryable());
            ownedUsers = ObjRoom.GetOwnedUsers(id, _connectionConfig.Value.ConnStr);
            RoomName = ObjRoom.GetRoomInfo(id, _connectionConfig.Value.ConnStr).rmName;
            UsersToAdd = "";
            UsersToDelete = "";
            if (ModelState.ErrorCount == 0)
            {
                ModelState.Clear();
            }
        }
    }
}
