using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using SeminarBuildingMap.GenericClasses;

namespace SeminarBuildingMap.Areas.Admin.Pages
{
    [Authorize(Roles="Faculty, Manager, Admin")]
    public class OwnedRoomDisplayModel : PageModel
    {

        private readonly IOptions<ConnectionConfig> _connectionConfig;

        readonly Models.RoomDataAccessLayer ObjRoom = new Models.RoomDataAccessLayer();

        [BindProperty]
        public Models.Room lclRoom { get; set; }

        public OwnedRoomDisplayModel(IOptions<ConnectionConfig> connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }

        public void OnGet(int id)
        {
            lclRoom = ObjRoom.GetRoomInfo(id, _connectionConfig.Value.ConnStr);
        }

        public void OnPost(int id)
        {

            if (ModelState.IsValid && ObjRoom.UserCanEditRoom(User.Identity.Name, id, _connectionConfig.Value.ConnStr))
            {
                if (User.IsInRole("Admin") && (lclRoom.rmType == "Classroom" || lclRoom.rmType == "Office"))
                {
                    ObjRoom.UpdateRoomNameType(id, lclRoom.rmName, lclRoom.rmType, _connectionConfig.Value.ConnStr);
                }
                else
                {
                    ObjRoom.UpdateRoomName(id, lclRoom.rmName, _connectionConfig.Value.ConnStr);
                }
            } 
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Request");
            }
            lclRoom = ObjRoom.GetRoomInfo(id, _connectionConfig.Value.ConnStr);
        }
    }
}
