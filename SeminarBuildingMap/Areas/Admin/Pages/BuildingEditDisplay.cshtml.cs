using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace SeminarBuildingMap.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class BuildingEditDisplayModel : PageModel
    {
        readonly Models.BuildingDataAccessLayer ObjBuilding = new Models.BuildingDataAccessLayer();

        private readonly IOptions<GenericClasses.ConnectionConfig> _connectionConfig;

        [BindProperty]
        public string flNo { get; set; }

        public IQueryable<Models.Floor> FloorData { get; set; }
        public BuildingEditDisplayModel(IOptions<GenericClasses.ConnectionConfig> connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }

        public void OnGet(string bdId)
        {
            FloorData = ObjBuilding.GetBuildingFloors(bdId, _connectionConfig.Value.ConnStr);
        }

        public void OnPost(string bdId)
        {
            ObjBuilding.AddFloor(bdId, flNo, _connectionConfig.Value.ConnStr);
            FloorData = ObjBuilding.GetBuildingFloors(bdId, _connectionConfig.Value.ConnStr);
        }
    }
}
