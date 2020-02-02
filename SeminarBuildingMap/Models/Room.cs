using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;


namespace SeminarBuildingMap.Models
{
    public partial class Room
    {
        [Required]
        public string Building { get; set; }
        [Required][Display(Name = "Room Number")]
        public string RoomNumber { get; set; }
        public String Coordinates { get; set; }
        public string Color { get; set; } = "Blue";

    }
}
