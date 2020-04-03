using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SeminarBuildingMap.GenericClasses;
using SeminarBuildingMap.Models;

namespace SeminarBuildingMap
{
    [Authorize(Roles="Admin")]
    public class FloorEditDisplayModel : PageModel
    {

        private readonly IOptions<ConnectionConfig> _connectionConfig;

        readonly RoomDataAccessLayer objRoom = new RoomDataAccessLayer();

        public IEnumerable<Room> Rooms { get; set; }

        public FloorEditDisplayModel(IOptions<ConnectionConfig> connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }

        public void OnGet(string bdId, string flId)
        {
            Rooms = objRoom.GetSelectedRooms(bdId, flId, _connectionConfig.Value.ConnStr);
        }

        public JsonResult OnGetAdd(string RoomNumber, string Coords)
        {
            objRoom.InsertDardenRoom(RoomNumber, Coords, _connectionConfig.Value.ConnStr);
            return new JsonResult("[success]");
        }
    }
}