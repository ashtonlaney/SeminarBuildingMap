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
    [Authorize(Roles="Admin")]
    public class BuildingEditSearchModel : PageModel
    {
        readonly Models.BuildingDataAccessLayer ObjBuilding = new Models.BuildingDataAccessLayer();

        private readonly IOptions<GenericClasses.ConnectionConfig> _connectionConfig;

        [BindProperty]
        public string bdId { get; set; }

        public IQueryable<Models.Building> BuildingData { get; set; }

        public BuildingEditSearchModel(IOptions<GenericClasses.ConnectionConfig> connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }

        public void OnGet()
        {
            BuildingData = ObjBuilding.GetAllBuildings(_connectionConfig.Value.ConnStr);
        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(bdId))
            {
                ObjBuilding.AddBuilding(bdId, _connectionConfig.Value.ConnStr);
                return RedirectToPage("./BuildingEditDisplay/" + bdId);
            } else

            {
                return Page();
            }
        }

    }
}
