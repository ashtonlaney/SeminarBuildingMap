using System;
using System.Collections.Generic;
using System.Linq;
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

        [BindProperty(SupportsGet = true)]
        public string building { get; set; }

        [BindProperty(SupportsGet = true)]
        public string floor { get; set; }
        
        [BindProperty]
        public string floorName { get; set; }
        [BindProperty]
        public string buildingName { get; set; }

        readonly RoomDataAccessLayer objRoom = new RoomDataAccessLayer(); //for creating rooms on map
        readonly BuildingDataAccessLayer objBuilding = new BuildingDataAccessLayer(); //get building list
        readonly RoomScheduleDataAccessLayer objSchedule = new RoomScheduleDataAccessLayer(); //for getting schedules for ajax call

        public IEnumerable<Room> Rooms { get; set; } //list of rooms to use on front end for geoJSON

        public IEnumerable<Building> Buildings { get; set; }

        public IEnumerable<Floor> Floors { get; set; }

        public IndexModel(IOptions<ConnectionConfig> connectionConfig) //init model
        {
            _connectionConfig = connectionConfig;
        }

        public void OnGet() //normal get request, loads rooms onto map
        {
            if (String.IsNullOrEmpty(building))
            {
                building = "Darden";
            }
            if (String.IsNullOrEmpty(floor))
            {
                floor = "1";
            }
            Buildings = objBuilding.GetAllBuildings(_connectionConfig.Value.ConnStr);
            Rooms = objRoom.GetSelectedRooms(building, floor, _connectionConfig.Value.ConnStr);
            Floors = objBuilding.GetBuildingFloors(building, _connectionConfig.Value.ConnStr);
            floorName = "";
            for(int i = 0; i < Floors.Count(); i++)
            {
                if(Floors.ElementAt(i).flNo == floor)
                {
                    floorName = Floors.ElementAt(i).flName;
                }
            }
            for (int i = 0; i < Buildings.Count(); i++)
            {
                if (Buildings.ElementAt(i).bdId.ToLower() == building.ToLower())
                {
                    buildingName = Buildings.ElementAt(i).bdName;
                }
            }
        }

        public JsonResult OnGetInfo(int rmId) //made to respond to ajax call for room info. provides todays schedule of room in json reach via /?handler=Info&Building=<building>&RoomNumber=<roomnumber>
        {
            string[] test = { "hello", "hi" };
            var ScheduleList = objSchedule.GetRoomSchedule_Today(rmId, _connectionConfig.Value.ConnStr);//gets list of bookings in room today
            Console.WriteLine(ScheduleList);
            return new JsonResult(ScheduleList); //return JSON serialized value for front end
        }

    }
} 
