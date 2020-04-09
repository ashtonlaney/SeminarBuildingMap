using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeminarBuildingMap.Models
{
    //used for when the schedule isn't being edited but just returned
    //this lightweight version is important because its returned over AJAX when a room is selected and it returns dates in an incredibly odd format if not stored as string literals
    public class SimpleSchedule
    {
        public SimpleSchedule(string avId, string avName, TimeSpan avStartTime, TimeSpan avEndTime, string avDay)
        {
            this.avId = avId;
            this.avName = avName;
            this.avDay = avDay;

            this.avStartTime = new DateTime(avStartTime.Ticks).ToShortTimeString();
            this.avEndTime = new DateTime(avEndTime.Ticks).ToShortTimeString();
        }

        public string avId { get; set; }
        public string avName { get; set; }
        public string avStartTime { get; set; }
        public string avEndTime { get; set; }
        public string avDay { get; set; }
        
    }
}
