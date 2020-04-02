using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace SeminarBuildingMap
{
    public class RoomListModel : PageModel
    {
        readonly Models.RoomDataAccessLayer ObjRoom = new Models.RoomDataAccessLayer();

        private readonly IOptions<GenericClasses.ConnectionConfig> _connectionConfig;

        public IQueryable<Models.Room> BuildingData { get; set; }

        public RoomListModel(IOptions<GenericClasses.ConnectionConfig> connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }

        public void OnGet()
        {
            //Needs to be able to change buildings. Currently it shows only dardens rooms.
            BuildingData = ObjRoom.GetBuildingRooms("DARDEN", _connectionConfig.Value.ConnStr);
        }
    }
}