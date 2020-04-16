using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace SeminarBuildingMap
{
    public class RoomScheduleModel : PageModel
    {
        readonly Models.RoomScheduleDataAccessLayer ObjSchedule = new Models.RoomScheduleDataAccessLayer();
        readonly Models.RoomDataAccessLayer ObjRoom = new Models.RoomDataAccessLayer();

        private readonly IOptions<GenericClasses.ConnectionConfig> _connectionConfig;

        public Models.Room RoomData { get; set; }

        public IQueryable<Models.SimpleSchedule> ScheduleData { get; set; }

        public RoomScheduleModel(IOptions<GenericClasses.ConnectionConfig> connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }

        public void OnGet(int Room)
        {
          RoomData = ObjRoom.GetRoomInfo(Room, _connectionConfig.Value.ConnStr);
          ScheduleData = ObjSchedule.GetRoomSchedule(Room , _connectionConfig.Value.ConnStr);
        }
    }
}