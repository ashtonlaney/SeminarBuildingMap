using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeminarBuildingMap.Models
{
    public class RoomSchedule
    {
   
        public RoomSchedule() { }

        public DateTime EndTime
        {
            get
            {
                return new DateTime(avEndTime.Ticks);
            }

            set
            {
                avEndTime = TimeSpan.FromTicks(value.Ticks);
            }

        }

        public DateTime StartTime
        {
            get
            {
                return new DateTime(avStartTime.Ticks);
            }
            set
            {
                avStartTime = TimeSpan.FromTicks(value.Ticks);
            }
        }

        public int avId { get; set; }
        public string avName { get; set; }
        public TimeSpan avStartTime { get; set; }
        public TimeSpan avEndTime { get; set; }
        public string avDay { get; set; }
        public DateTime evDate { get;set; }
        public int rmId { get; set; }

    }
}
