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
    public class EditEventModel : PageModel
    {
        private readonly IOptions<ConnectionConfig> _connectionConfig;

        readonly Models.RoomDataAccessLayer ObjRoom = new Models.RoomDataAccessLayer();

        readonly Models.RoomScheduleDataAccessLayer ObjSchedule = new Models.RoomScheduleDataAccessLayer();

        [BindProperty]
        public Models.RoomSchedule lclNewEvent { get; set; }
        public IQueryable<Models.RoomSchedule> lclSchedule { get; set; }

        public EditEventModel(IOptions<ConnectionConfig> connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }
        public void OnGet(int id)
        {
            lclSchedule = ObjSchedule.GetRoomEvents(id, _connectionConfig.Value.ConnStr);
        }

        public void OnPostSave(int id)
        {
            lclNewEvent.rmId = id;
            if (ModelState.IsValid && ObjRoom.UserCanEditRoom(User.Identity.Name, id, _connectionConfig.Value.ConnStr))
            {
                if (lclNewEvent.avId == 0)
                {
                    ObjSchedule.InsertRoomEvent(lclNewEvent, _connectionConfig.Value.ConnStr);
                }
                else
                {
                    ObjSchedule.EditRoomEvent(lclNewEvent, _connectionConfig.Value.ConnStr);
                }
            } else
            {
                ModelState.AddModelError(string.Empty, "Error: Invalid Request");
            }
            lclSchedule = ObjSchedule.GetRoomEvents(id, _connectionConfig.Value.ConnStr);
            lclNewEvent = new Models.RoomSchedule();
        }

        public void OnPostDelete(int id)
        {
            if (ModelState.IsValid && ObjRoom.UserCanEditRoom(User.Identity.Name, id, _connectionConfig.Value.ConnStr))
            {
                if (lclNewEvent.avId != 0)
                {
                    ObjSchedule.DeleteRoomEvent(lclNewEvent.avId, _connectionConfig.Value.ConnStr);
                }
            } else
            {
                ModelState.AddModelError(string.Empty, "Error: Invalid Request");
            }
            lclSchedule = ObjSchedule.GetRoomEvents(id, _connectionConfig.Value.ConnStr);
            lclNewEvent = new Models.RoomSchedule();
        }
    }
}
