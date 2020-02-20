using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SeminarBuildingMap.GenericClasses;
using SeminarBuildingMap.Models;

namespace SeminarBuildingMap
{
    public class AddRoomsToMapModel : PageModel
    {

        private readonly IOptions<ConnectionConfig> _connectionConfig;

        readonly RoomDataAccessLayer objRoom = new RoomDataAccessLayer();

        public IEnumerable<Room> Rooms { get; set; }

        public AddRoomsToMapModel(IOptions<ConnectionConfig> connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }

        public void OnGet()
        {
            Rooms = objRoom.GetDardenRooms(_connectionConfig.Value.ConnStr);
        }

        public JsonResult OnGetAdd(String RoomNumber, String Coords)
        {
            objRoom.InsertDardenRoom(RoomNumber, Coords, _connectionConfig.Value.ConnStr);
            return new JsonResult("[success]");
        }
    }
}