using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace SeminarBuildingMap.Areas.Admin.Pages
{
    public class FloorEditDisplayModel : PageModel
    {

        readonly Models.BuildingDataAccessLayer ObjBuilding = new Models.BuildingDataAccessLayer();

        private readonly IOptions<GenericClasses.ConnectionConfig> _connectionConfig;

        private IWebHostEnvironment _environment;

        [BindProperty]
        public string flName { get; set; }

        [BindProperty]
        public IFormFile Upload { get; set; }

        public bool isFloorPlan { get; set; }

        public FloorEditDisplayModel(IOptions<GenericClasses.ConnectionConfig> connectionConfig, IWebHostEnvironment environment)
        {
            _connectionConfig = connectionConfig;
            _environment = environment;
        }
        public void OnGet(string bdId, string flNo)
        {
            flName = ObjBuilding.GetFloorName(bdId, flNo, _connectionConfig.Value.ConnStr);
            isFloorPlan = System.IO.File.Exists(Path.Combine(_environment.ContentRootPath, "wwwroot/images", bdId + flNo + ".svg"));
        }

        public void OnPostUpdate(string bdId, string flNo)
        {
            if (!String.IsNullOrEmpty(flName))
            {
                ObjBuilding.UpdateFloorName(bdId, flNo, flName, _connectionConfig.Value.ConnStr);
            }
        }

        public void OnPostUploadAsync(string bdId, string flNo)
        {
            string[] extension = Upload.FileName.Split(".");
            if (extension[extension.Length - 1].ToLower() == "svg") {
                string fileName = bdId + flNo + ".svg";
                var file = Path.Combine(_environment.ContentRootPath, "wwwroot/images", fileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    Upload.CopyTo(fileStream);
                }
            }
        }
    }
}
