using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using SeminarBuildingMap.GenericClasses;

namespace SeminarBuildingMap.Areas.Admin.Pages
{
    public class EditAvailabilityModel : PageModel
    {
        private readonly IOptions<ConnectionConfig> _connectionConfig;

        readonly Models.RoomDataAccessLayer ObjRoom = new Models.RoomDataAccessLayer();

        readonly Models.RoomScheduleDataAccessLayer ObjSchedule = new Models.RoomScheduleDataAccessLayer();

        [BindProperty]
        public Models.RoomSchedule lclNewAvailability { get; set; }
        [BindProperty]
        public List<String> checkboxDays { get; set; }
        public IQueryable<Models.RoomSchedule> lclSchedule { get; set; }

        public EditAvailabilityModel(IOptions<ConnectionConfig> connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }

        public void OnGet(int id)
        {
            lclSchedule = ObjSchedule.GetRoomAvailability(id, _connectionConfig.Value.ConnStr);
        }

        public void OnPostSave(int id)
        {
            lclNewAvailability.rmId = id;
            if (ModelState.IsValid && ObjRoom.UserCanEditRoom(User.Identity.Name, id, _connectionConfig.Value.ConnStr))
            {
                if (lclNewAvailability.avId == 0)
                {
                    var validDays = new List<String> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                    if(checkboxDays.Count == 0)
                    {
                        ModelState.AddModelError(String.Empty, "Atleast one day must be selected");
                    }
                    foreach (string day in checkboxDays)
                    {
                        if (validDays.Contains(day))
                        {
                            lclNewAvailability.avDay = day;
                            ObjSchedule.InsertRoomAvailability(lclNewAvailability, _connectionConfig.Value.ConnStr);
                        }
                    }
                }
                else
                {
                    ObjSchedule.EditRoomAvailability(lclNewAvailability, _connectionConfig.Value.ConnStr);
                }
            } else
            {
                ModelState.AddModelError(String.Empty, "Error: Invalid Request");
            }
            lclSchedule = ObjSchedule.GetRoomAvailability(id, _connectionConfig.Value.ConnStr);
            lclNewAvailability = new Models.RoomSchedule();
        }

        public void OnPostDelete(int id)
        {
            if(ModelState.IsValid && ObjRoom.UserCanEditRoom(User.Identity.Name, id, _connectionConfig.Value.ConnStr))
            {
                if (lclNewAvailability.avId != 0)
                {
                    ObjSchedule.DeleteRoomAvailability(lclNewAvailability.avId, _connectionConfig.Value.ConnStr);
                } else
                {
                    ModelState.AddModelError(string.Empty, "An availability record must be selected to delete");
                }
            } else
            {
                ModelState.AddModelError(string.Empty, "Error: Invalid Request");
            }
            lclSchedule = ObjSchedule.GetRoomAvailability(id, _connectionConfig.Value.ConnStr);
            lclNewAvailability = new Models.RoomSchedule();
        }
    }

}
