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
    [Authorize(Roles="Faculty,Manager,Admin")]
    public class OwnedRoomSearchModel : PageModel
    {
        readonly Models.RoomDataAccessLayer ObjRoom = new Models.RoomDataAccessLayer();

        private readonly IOptions<GenericClasses.ConnectionConfig> _connectionConfig;

        public IQueryable<Models.Room> RoomData { get; set; } 
        public OwnedRoomSearchModel(IOptions<GenericClasses.ConnectionConfig> connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }
        public void OnGet() //lists a users rooms they own. how this is determined is based upon their role
        {
            string userRole;
            if (User.IsInRole("Admin"))
            {
                userRole = "Admin";
            }
            else if (User.IsInRole("Manager"))
            {
                userRole = "Manager";
            }
            else
            {
                userRole = "Faculty";
            }
                RoomData = ObjRoom.GetOwnedRooms(User.Identity.Name, userRole, _connectionConfig.Value.ConnStr);
        }
    }
}


