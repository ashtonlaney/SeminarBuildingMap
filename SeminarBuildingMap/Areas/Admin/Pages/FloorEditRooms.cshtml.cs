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

        [BindProperty]
        public string coords { get; set; }

        [BindProperty]
        public string rmNo { get; set; }
        [BindProperty]
        public string newRmNo { get; set; }

        [BindProperty]
        public string rmId { get; set; }

        private readonly IOptions<ConnectionConfig> _connectionConfig;

        readonly RoomDataAccessLayer objRoom = new RoomDataAccessLayer();

        public IEnumerable<Room> Rooms { get; set; }

        public FloorEditDisplayModel(IOptions<ConnectionConfig> connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }

        public void OnGet(string bdId, string flNo)
        {
            Rooms = objRoom.GetSelectedRooms(bdId, flNo, _connectionConfig.Value.ConnStr);
        }

        public void OnPostAdd(string bdId, string flNo)
        {
            if (!String.IsNullOrEmpty(newRmNo))
            {
                objRoom.AddRoom(newRmNo, bdId, flNo, coords, _connectionConfig.Value.ConnStr);
            }
            Rooms = objRoom.GetSelectedRooms(bdId, flNo, _connectionConfig.Value.ConnStr);
            coords = "";
            rmNo = "";
            newRmNo = "";
            rmId = "";
        }

        public void OnPostUpdate(string bdId, string flNo)
        {
            if (!String.IsNullOrEmpty(rmId))
            {
                objRoom.UpdateRoom(rmId, rmNo, _connectionConfig.Value.ConnStr);
            }
            Rooms = objRoom.GetSelectedRooms(bdId, flNo, _connectionConfig.Value.ConnStr);
        }

        public void OnPostDelete(string bdId, string flNo)
        {
            objRoom.DeleteRoom(rmId, _connectionConfig.Value.ConnStr);
            Rooms = objRoom.GetSelectedRooms(bdId, flNo, _connectionConfig.Value.ConnStr);
        }
    }
}