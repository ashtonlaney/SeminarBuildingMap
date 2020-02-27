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

        public IQueryable<Models.RoomSchedule> lclSchedule { get; set; }

        public EditAvailabilityModel(IOptions<ConnectionConfig> connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }

        public void OnGet(int id)
        {
            lclSchedule = ObjSchedule.GetRoomSchedule(id, _connectionConfig.Value.ConnStr);
            foreach (Models.RoomSchedule availability in lclSchedule)
            {
                availability.convertToEst();
            }
        }

        public void OnPost(int id)
        {
            if (ModelState.IsValid && ObjRoom.UserCanEditRoom(User.Identity.Name, id, _connectionConfig.Value.ConnStr))
            {
                lclNewAvailability.convertToUtc();
                lclNewAvailability.rmId = id;
                if (lclNewAvailability.avId == 0)
                {
                    ObjSchedule.InsertRoomAvailability(lclNewAvailability, _connectionConfig.Value.ConnStr);
                }
                else
                {
                    ObjSchedule.EditRoomAvailability(lclNewAvailability, _connectionConfig.Value.ConnStr);
                }
            }
            lclSchedule = ObjSchedule.GetRoomSchedule(id, _connectionConfig.Value.ConnStr);
            foreach (Models.RoomSchedule availability in lclSchedule)
            {
                availability.convertToEst();
            }
        }
    }
}
