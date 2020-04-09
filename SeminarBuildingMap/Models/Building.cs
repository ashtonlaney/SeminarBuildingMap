using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeminarBuildingMap.Models
{
    //represents the smBuildings table in SQL, simply holds the ID and Name of a building
    public class Building
    {
        public string bdId { get; set; }
        public string bdName { get; set; }
    }
}
