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
    public class OwnedRoomDisplayModel : PageModel
    {

        private readonly IOptions<ConnectionConfig> _connectionConfig;

        readonly Models.RoomDataAccessLayer ObjRoom = new Models.RoomDataAccessLayer();

        readonly Models.RoomScheduleDataAccessLayer ObjSchedule = new Models.RoomScheduleDataAccessLayer();

        public Models.Room lclRoom { get; set; }

        public IQueryable<Models.RoomSchedule> lclSchedule { get; set; }

        public OwnedRoomDisplayModel(IOptions<ConnectionConfig> connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }

        public void OnGet(int id)
        {
            lclRoom = ObjRoom.GetRoomInfo(id, _connectionConfig.Value.ConnStr);
            lclSchedule = ObjSchedule.GetRoomSchedule(lclRoom.rmNo, _connectionConfig.Value.ConnStr);
        }
    }
}
