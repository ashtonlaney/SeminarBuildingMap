using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace SeminarBuildingMap.Pages
{
    public class BuildingListModel : PageModel
    {

        readonly Models.BuildingDataAccessLayer ObjBuilding = new Models.BuildingDataAccessLayer();

        private readonly IOptions<GenericClasses.ConnectionConfig> _connectionConfig;

        public IQueryable<Models.Building> BuildingData { get; set; }

        public BuildingListModel(IOptions<GenericClasses.ConnectionConfig> connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }

        public void OnGet()
        {
            BuildingData = ObjBuilding.GetAllBuildings(_connectionConfig.Value.ConnStr);
        }

    }
}
