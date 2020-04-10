using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace SeminarBuildingMap.Areas.Admin.Pages
{
    //only admins can access this page
    [Authorize(Roles = "Admin")]
    public class BuildingEditDisplayModel : PageModel
    {
        readonly Models.BuildingDataAccessLayer ObjBuilding = new Models.BuildingDataAccessLayer(); //grabs an instance of BuildingDataAccess in order to communicate with SQL

        private readonly IOptions<GenericClasses.ConnectionConfig> _connectionConfig; 

        [BindProperty][StringLength(1)] //limit the string to a single character
        public string flNo { get; set; }

        [BindProperty][Display(Name ="Building Name")][StringLength(50)]
        public string bdName { get; set; }

        public IQueryable<Models.Floor> FloorData { get; set; } //holds the data for the floor datagrid
         
        public BuildingEditDisplayModel(IOptions<GenericClasses.ConnectionConfig> connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }

        public void OnGet(string bdId) //sets the floor datagrid and the current building name textbox
        {
            FloorData = ObjBuilding.GetBuildingFloors(bdId, _connectionConfig.Value.ConnStr);
            bdName = ObjBuilding.GetBuildingName(bdId, _connectionConfig.Value.ConnStr);
        }

        public void OnPostUpdate(string bdId)
        {
            if (!String.IsNullOrEmpty(bdName)) //checks that a building name is supplied
            {
                ObjBuilding.UpdateBuilding(bdId, bdName, _connectionConfig.Value.ConnStr); //updates the building name
            } else
            {
                ModelState.AddModelError(string.Empty, "Building Name is required");
            }
            bdName = ObjBuilding.GetBuildingName(bdId, _connectionConfig.Value.ConnStr); //reload the data
            FloorData = ObjBuilding.GetBuildingFloors(bdId, _connectionConfig.Value.ConnStr);
        }

        public void OnPostAdd(string bdId)
        {
            if (!String.IsNullOrEmpty(flNo)) //floor must have a #
            {
                ObjBuilding.AddFloor(bdId, flNo, _connectionConfig.Value.ConnStr); //add new floor
            } else
            {
                ModelState.AddModelError(string.Empty, "Floor Number is required");
            }
            bdName = ObjBuilding.GetBuildingName(bdId, _connectionConfig.Value.ConnStr);
            FloorData = ObjBuilding.GetBuildingFloors(bdId, _connectionConfig.Value.ConnStr);
        }
    }
}
