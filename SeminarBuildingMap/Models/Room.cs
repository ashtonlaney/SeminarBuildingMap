using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;


namespace SeminarBuildingMap.Models
{
 
    //Represents a specific room
    public partial class Room
    {
        static readonly string Switch = "Yellow"; //these define what color to represent each room status as on the map
        static readonly string NotAvailable = "Red";
        static readonly string Available = "Green";

        //makes a rmNo mandatory and sets to display it as "Room Number" when referenced as a label
        [Required][Display(Name = "Room Number")]
        public string rmNo { get; set; }
        public string bdId { get; set; }
        public string rmId { get; set; }
        public string rmName { get; set; }
        public string rmTopLeftPoint { get; set; } //these are the 4 points on the map for the room
        public string rmBottomLeftPoint { get; set; }
        public string rmBottomRightPoint { get; set; }
        public string rmTopRightPoint { get; set; }
        public string isAvailable { get; set; } //this is calculated in SQL and lists whether the room is available at the current time
        public string Color { //when color is called it looks at if the room is listed as available and returns a color based on this
            get
            {
                if (string.IsNullOrEmpty(isAvailable))
                {
                    return NotAvailable;
                }
                return isAvailable switch
                {
                    "SWITCH" => Switch,
                    "NOT" => NotAvailable,
                    "AVAIL" => Available,
                    _ => NotAvailable,
                };
            }
        }
        public string flNo { get; set; }
        public string rmType { get; set; }

    }
}

