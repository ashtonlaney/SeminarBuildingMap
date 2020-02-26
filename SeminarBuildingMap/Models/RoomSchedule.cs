using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeminarBuildingMap.Models
{
    public class RoomSchedule
    {
        //manual constructor, since we have to handle changing the sql time type to a string representation
        /*public RoomSchedule(int? avId, string avName, string avDay, TimeSpan avStartTime, TimeSpan avEndTime)
        {
            this.avId = avId;
            this.avName = avName;
            this.avDay = avDay;

            //time stuff
            TimeZoneInfo easternTime = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var dt = new DateTime(avStartTime.Ticks);
            this.avStartTime = TimeZoneInfo.ConvertTimeFromUtc(dt, easternTime).ToShortTimeString();
            
            dt = new DateTime(avEndTime.Ticks);
            this.avEndTime = TimeZoneInfo.ConvertTimeFromUtc(dt, easternTime).ToShortTimeString();
        }*/

        /*public RoomSchedule(int avId, string avName, string avDay, TimeSpan avStartTime, TimeSpan avEndTime, bool convertToUtc)
        {
            this.avId = avId;
            this.avName = avName;
            this.avDay = avDay;

            TimeZoneInfo easternTime = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var dt = new DateTime(avStartTime.Ticks);
            this.avStartTime = TimeZoneInfo.ConvertTimeToUtc(dt, easternTime).ToShortTimeString();

            dt = new DateTime(avEndTime.Ticks);
            this.avEndTime = TimeZoneInfo.ConvertTimeToUtc(dt, easternTime).ToShortTimeString();
        }*/

        public void convertToUtc()
        {
            TimeZoneInfo easternTime = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var dt = new DateTime(TimeSpan.Parse(EstStartTime).Ticks);
            UstStartTime = TimeZoneInfo.ConvertTimeToUtc(dt, easternTime).ToShortTimeString();

            dt = new DateTime(TimeSpan.Parse(EstEndTime).Ticks);
            UstEndTime = TimeZoneInfo.ConvertTimeToUtc(dt, easternTime).ToShortTimeString();
        }

        public void convertToEst()
        {
            TimeZoneInfo easternTime = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var dt = new DateTime(avStartTime.Ticks);
            this.EstStartTime = TimeZoneInfo.ConvertTimeFromUtc(dt, easternTime).ToShortTimeString();

            dt = new DateTime(avEndTime.Ticks);
            this.EstEndTime = TimeZoneInfo.ConvertTimeFromUtc(dt, easternTime).ToShortTimeString();
        }


        public int avId { get; set; }
        public string avName { get; set; }
        public TimeSpan avStartTime { get; set; }
        public TimeSpan avEndTime { get; set; }
        public string avDay { get; set; }
        public int avRoom { get; set; }

        public string EstStartTime { get; set; }
        public string EstEndTime { get; set; }

        public string UstStartTime { get; set; }

        public string UstEndTime { get; set; }
    }
}
