using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using SeminarBuildingMap.Areas.Identity.Data;
using SeminarBuildingMap.GenericClasses;
using SeminarBuildingMap.Models;

namespace SeminarBuildingMap.Areas.Admin
{
    [Authorize(Roles = "Admin")]
    public class SubuserDisplayModel : PageModel
    {
        private readonly IOptions<ConnectionConfig> _connectionConfig;
        private readonly UserManager<SeminarBuildingMapUser> _userManager;
        private readonly Models.BuildingDataAccessLayer ObjBuilding = new Models.BuildingDataAccessLayer();
        public SelectList buildings { get; set; }

        [BindProperty]
        public string Email { get; set; }

        public List<String> AvailableRoles { get; set; }
        [BindProperty]
        public string SelectedRole { get; set; }

        [BindProperty]
        public List<String> SelectedBuildings { get; set; }

        public SubuserDisplayModel(IOptions<GenericClasses.ConnectionConfig> connectionConfig, UserManager<SeminarBuildingMapUser> userManager)
        {
            _connectionConfig = connectionConfig;
            _userManager = userManager;
        }
        public async Task OnGetAsync(string id)
        {
            SeminarBuildingMapUser user = await _userManager.FindByIdAsync(id);
            Email = user.UserName;
            List<Building> tempBuilding = ObjBuilding.GetAllBuildings(_connectionConfig.Value.ConnStr).ToList();
            buildings = new SelectList(tempBuilding, nameof(Building.bdId), nameof(Building.bdName));
            SelectedBuildings = ObjBuilding.GetOwnedBuildings(user.UserName, _connectionConfig.Value.ConnStr).ToList();

            AvailableRoles = new List<String>();
            AvailableRoles.Add("Admin");
            AvailableRoles.Add("Manager");
            AvailableRoles.Add("Faculty");
            if(await _userManager.IsInRoleAsync(user, "Admin"))
            {
                SelectedRole = "Admin";
            }
            else if(await _userManager.IsInRoleAsync(user, "Manager"))
            {
                SelectedRole = "Manager";
            }
            else
            {
                SelectedRole = "Faculty";
            }
        }

        

        public async Task<PageResult> OnPostSaveAsync(string id)
        {

            AvailableRoles = new List<String>();
            AvailableRoles.Add("Admin");
            AvailableRoles.Add("Manager");
            AvailableRoles.Add("Faculty");

            SeminarBuildingMapUser user = await _userManager.FindByIdAsync(id);
            if(!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                if (AvailableRoles.Contains(SelectedRole) && !await _userManager.IsInRoleAsync(user, SelectedRole))
                {
                    IList<String> roles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles);
                    await _userManager.AddToRoleAsync(user, SelectedRole);
                }

                ObjBuilding.ClearOwnedBuildings(user.UserName, _connectionConfig.Value.ConnStr);
                foreach(string bdId in SelectedBuildings)
                {
                    ObjBuilding.AddOwnedBuilding(user.UserName, bdId, _connectionConfig.Value.ConnStr);
                }
            }
            List<Building> tempBuilding = ObjBuilding.GetAllBuildings(_connectionConfig.Value.ConnStr).ToList();
            buildings = new SelectList(tempBuilding, nameof(Building.bdId), nameof(Building.bdName));
            SelectedBuildings = ObjBuilding.GetOwnedBuildings(user.UserName, _connectionConfig.Value.ConnStr).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            SeminarBuildingMapUser user = await _userManager.FindByIdAsync(id);
            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
               await _userManager.DeleteAsync(user);
               return RedirectToPage("SubuserSearch");
            }
            return Page();
        }


    }
}
