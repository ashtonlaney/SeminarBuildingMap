using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using SeminarBuildingMap.GenericClasses;
using SeminarBuildingMap.Models;

namespace SeminarBuildingMap.Pages
{
    public class IndexModel : PageModel
    {

        private readonly IOptions<ConnectionConfig> _connectionConfig; //brings connection strings into this page

        readonly RoomDataAccessLayer objRoom = new RoomDataAccessLayer(); //for creating rooms on map
        readonly RoomScheduleDataAccessLayer objSchedule = new RoomScheduleDataAccessLayer(); //for getting schedules for ajax call

        public IEnumerable<Room> Rooms { get; set; } //list of rooms to use on front end for geoJSON

        public IndexModel(IOptions<ConnectionConfig> connectionConfig) //init model
        {
            _connectionConfig = connectionConfig;
        }

        public void OnGet() //normal get request, loads rooms onto map
        {
            Rooms = objRoom.GetSelectedRooms("Darden", "2", _connectionConfig.Value.ConnStr);
        }

        public JsonResult OnGetInfo(String Building, String RoomNumber) //made to respond to ajax call for room info. provides todays schedule of room in json reach via /?handler=Info&Building=<building>&RoomNumber=<roomnumber>
        {
            string[] test = { "hello", "hi" };
            var ScheduleList = objSchedule.GetRoomSchedule_Today(RoomNumber, _connectionConfig.Value.ConnStr);//gets list of bookings in room today
            Console.WriteLine(ScheduleList);
            return new JsonResult(test); //return JSON serialized value for front end
        }

    }
} 
