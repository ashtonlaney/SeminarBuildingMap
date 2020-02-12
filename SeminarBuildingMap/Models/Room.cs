using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;


namespace SeminarBuildingMap.Models
{
    public partial class Room
    {
        [Required][Display(Name = "Room Number")]
        public string rmId { get; set; }
        public string rmName { get; set; }
        public string rmTopLeftPoint { get; set; }
        public string rmBottomLeftPoint { get; set; }
        public string rmBottomRightPoint { get; set; }
        public string rmTopRightPoint { get; set; }

        public string Color { get; set; } = "Blue";

    }
}

