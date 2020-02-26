using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;


namespace SeminarBuildingMap.Models
{
    public partial class Room
    {
        static readonly string Switch = "Yellow";
        static readonly string NotAvailable = "Red";
        static readonly string Available = "Green";

        [Required][Display(Name = "Room Number")]
        public string rmNo { get; set; }
        public string bdId { get; set; }
        public string rmId { get; set; }
        public string rmName { get; set; }
        public string rmTopLeftPoint { get; set; }
        public string rmBottomLeftPoint { get; set; }
        public string rmBottomRightPoint { get; set; }
        public string rmTopRightPoint { get; set; }
        public string isAvailable { get; set; }
        public string Color { 
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
        public string rmFloorNo { get; set; }
        public string rmType { get; set; }

    }
}

