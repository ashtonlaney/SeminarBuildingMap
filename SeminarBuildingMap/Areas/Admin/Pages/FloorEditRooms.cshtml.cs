using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string coords { get; set; } //coords to add for room

        [BindProperty][StringLength(10)]
        public string rmNo { get; set; } //room # to edit
        [BindProperty][StringLength(10)]
        public string newRmNo { get; set; } //room # of new room to add

        [BindProperty]
        public string rmId { get; set; } //rmId to edit

        private readonly IOptions<ConnectionConfig> _connectionConfig;

        readonly RoomDataAccessLayer objRoom = new RoomDataAccessLayer();

        public IEnumerable<Room> Rooms { get; set; } //list of rooms to add to map

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
            if (ModelState.IsValid && !String.IsNullOrEmpty(newRmNo))
            {
               bool success = objRoom.AddRoom(newRmNo, bdId, flNo, coords, _connectionConfig.Value.ConnStr); //add room, returns true if it successfully adds (mainly doesn't due to bad coord input)
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, "Please supply exactly 4 points");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Room Number is required for new rooms");
            }
            Rooms = objRoom.GetSelectedRooms(bdId, flNo, _connectionConfig.Value.ConnStr);
            coords = "";
            rmNo = "";
            newRmNo = "";
            rmId = "";
        }

        public void OnPostUpdate(string bdId, string flNo) //updates room # of a rmId
        {
            if (ModelState.IsValid && !String.IsNullOrEmpty(rmId) && !String.IsNullOrEmpty(rmNo) && int.TryParse(rmId, out _))
            {
                objRoom.UpdateRoom(rmId, rmNo, _connectionConfig.Value.ConnStr);
            }
            else
            {              
                ModelState.AddModelError(string.Empty, "A Room must be selected and a room number must be provided to update a room");
            }
            Rooms = objRoom.GetSelectedRooms(bdId, flNo, _connectionConfig.Value.ConnStr);
            coords = "";
            rmNo = "";
            newRmNo = "";
            rmId = "";
        }

        public void OnPostDelete(string bdId, string flNo) //delete selected room based on id
        {
            if (ModelState.IsValid && !String.IsNullOrEmpty(rmId) && int.TryParse(rmId, out _)) //makes sure rmId is a valid int, the out _ throws away the conversion of the tryParse
            {
                objRoom.DeleteRoom(rmId, _connectionConfig.Value.ConnStr);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "A Room must be selected in order to delete");
            }
            Rooms = objRoom.GetSelectedRooms(bdId, flNo, _connectionConfig.Value.ConnStr);
            coords = "";
            rmNo = "";
            newRmNo = "";
            rmId = "";
        }
    }
}