using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        private IWebHostEnvironment _environment; //used to find paths to save file

        [BindProperty][StringLength(50)]
        public string flName { get; set; }

        [BindProperty]
        public IFormFile Upload { get; set; } //holds uploaded file

        public bool isFloorPlan { get; set; } //stores if svg is saved already

        public FloorEditDisplayModel(IOptions<GenericClasses.ConnectionConfig> connectionConfig, IWebHostEnvironment environment)
        {
            _connectionConfig = connectionConfig;
            _environment = environment;
        }
        public void OnGet(string bdId, string flNo)
        {
            flName = ObjBuilding.GetFloorName(bdId, flNo, _connectionConfig.Value.ConnStr); //gets the floorname from its id
            isFloorPlan = System.IO.File.Exists(Path.Combine(_environment.ContentRootPath, "wwwroot/images", bdId + flNo + ".svg")); //checks to see if file is already saved on webserver
        }

        public void OnPostUpdate(string bdId, string flNo)
        {
            if (!String.IsNullOrEmpty(flName))
            {
                ObjBuilding.UpdateFloorName(bdId, flNo, flName, _connectionConfig.Value.ConnStr);
            } 
            else
            {
                ModelState.AddModelError(string.Empty, "Floor Name is Required");
            }
            flName = ObjBuilding.GetFloorName(bdId, flNo, _connectionConfig.Value.ConnStr);
            isFloorPlan = System.IO.File.Exists(Path.Combine(_environment.ContentRootPath, "wwwroot/images", bdId + flNo + ".svg"));
        }

        public void OnPostUploadAsync(string bdId, string flNo) //handles uploading file
        {
            if (!(Upload == null))
            {
                string[] extension = Upload.FileName.Split(".");
                if (extension[extension.Length - 1].ToLower() == "svg") //ensurs the uploaded file atleast is an .svg file
                {
                    string fileName = bdId + flNo + ".svg"; //ignore what the files name is, instead change it to  what the system requires
                    var file = Path.Combine(_environment.ContentRootPath, "wwwroot/images", fileName); //create the path to save it on
                    using (var fileStream = new FileStream(file, FileMode.Create)) 
                    {
                        Upload.CopyTo(fileStream); //do the actual write
                    }
                } else
                {
                    ModelState.AddModelError(string.Empty, "Floorplan must be a .svg file");
                }
            } else
            {
                ModelState.AddModelError(string.Empty, "Floorplan file is required");
            }
            flName = ObjBuilding.GetFloorName(bdId, flNo, _connectionConfig.Value.ConnStr);
            isFloorPlan = System.IO.File.Exists(Path.Combine(_environment.ContentRootPath, "wwwroot/images", bdId + flNo + ".svg"));
        }
    }
}
