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

        [BindProperty(SupportsGet = true)] //allows the bind property to be set on get requests, security vulnerability if updating anything, but this is simply a SELECT parameter
        public string building { get; set; } //input building

        [BindProperty(SupportsGet = true)]
        public string floor { get; set; } //input floor
        
        [BindProperty]
        public string floorName { get; set; } //name of floor to display
        [BindProperty]
        public string buildingName { get; set; } //name of building to display

        readonly RoomDataAccessLayer objRoom = new RoomDataAccessLayer(); //for creating rooms on map
        readonly BuildingDataAccessLayer objBuilding = new BuildingDataAccessLayer(); //get building list
        readonly RoomScheduleDataAccessLayer objSchedule = new RoomScheduleDataAccessLayer(); //for getting schedules for ajax call

        public IEnumerable<Room> Rooms { get; set; } //list of rooms to use on front end for geoJSON

        public IEnumerable<Building> Buildings { get; set; } //list of buildings for drop down

        public IEnumerable<Floor> Floors { get; set; } //list of floors in selected building for drop down

        public IndexModel(IOptions<ConnectionConfig> connectionConfig) //init model
        {
            _connectionConfig = connectionConfig;
        }

        public void OnGet() //normal get request, loads rooms onto map
        {
            if (String.IsNullOrEmpty(building)) //default to Darden if no room is selected
            {
                building = "Darden";
            }
            if (String.IsNullOrEmpty(floor)) //seems safe to default to a 1st floor of a building
            {
                floor = "1";
            }
            Buildings = objBuilding.GetAllBuildings(_connectionConfig.Value.ConnStr); //fetch building list
            Rooms = objRoom.GetSelectedRooms(building, floor, _connectionConfig.Value.ConnStr); //fetch rooms in building to display on map
            Floors = objBuilding.GetBuildingFloors(building, _connectionConfig.Value.ConnStr); //fetch floors for dropdown
            floorName = "";
            for(int i = 0; i < Floors.Count(); i++)
            {
                if(Floors.ElementAt(i).flNo == floor)//set the floorname for the selected floor
                {
                    floorName = Floors.ElementAt(i).flName; 
                }
            }
            for (int i = 0; i < Buildings.Count(); i++)
            {
                if (Buildings.ElementAt(i).bdId.ToLower() == building.ToLower()) //set the buildingname for the selected building
                {
                    buildingName = Buildings.ElementAt(i).bdName;
                }
            }
        }

        public JsonResult OnGetInfo(int rmId) //made to respond to ajax call for room info. provides todays schedule of room in json reach via /?handler=Info&RmId=<RmId>
        {
            var ScheduleList = objSchedule.GetRoomSchedule_Today(rmId, _connectionConfig.Value.ConnStr);//gets list of bookings in room today
            return new JsonResult(ScheduleList); //return JSON serialized value for front end
        }

    }
} 
