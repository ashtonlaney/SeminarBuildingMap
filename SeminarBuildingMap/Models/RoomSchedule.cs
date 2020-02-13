using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeminarBuildingMap.Models
{
    public class RoomSchedule
    {
        //manual constructor, since we have to handle changing the sql time type to a string representation
        public RoomSchedule(string avId, string avName, TimeSpan avStartTime, TimeSpan avEndTime)
        {
            this.avId = avId;
            this.avName = avName;

            //time stuff
            TimeZoneInfo easternTime = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var dt = new DateTime(avStartTime.Ticks);
            this.avStartTime = TimeZoneInfo.ConvertTimeFromUtc(dt, easternTime).ToShortTimeString();
            
            dt = new DateTime(avEndTime.Ticks);
            this.avEndTime = TimeZoneInfo.ConvertTimeFromUtc(dt, easternTime).ToShortTimeString();
        }

        public string avId { get; set; }
        public string avName { get; set; }
        public string avStartTime { get; set; }
        public string avEndTime { get; set; }
    }
}
