using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeminarBuildingMap.Models
{
    //Represents a RoomSchedule, aka an availability or event within a room
    public class RoomSchedule
    {

        //empty constructor for dapper to use   
        public RoomSchedule() { }

        //the value is internally stored as a TimeSpan, but HTML inputs return datetimes
        public DateTime EndTime
        {
            get
            {
                return new DateTime(avEndTime.Ticks); //when pulling the value convert to datetime
            }

            set
            {
                avEndTime = TimeSpan.FromTicks(value.Ticks); //set the internal TimeSpan
            }

        }

        //same as above
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

        //the av prefix is used for events which have ev as well
        public int avId { get; set; }
        public string avName { get; set; }
        [Display(Name = "Start Time")]
        public TimeSpan avStartTime { get; set; }
        [Display(Name = "End Time")]
        public TimeSpan avEndTime { get; set; }
        public string avDay { get; set; }
        public DateTime evDate { get;set; }
        public int rmId { get; set; }

    }
}
