using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeminarBuildingMap.Models
{
    //represents the smBuildings_Floors table in SQL, simply holds the building, floor number, and floor name
    public class Floor
    {
        public string bdId { get; set; }
        public string flNo { get; set; }
        public string flName { get; set; }
    }
}
