using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SeminarBuildingMap.Models
{
    //Deals with all touching the database using Dapper for room schedules (events/availability)
    public class RoomScheduleDataAccessLayer
    {
        //this function takes in a room and returns all events/availabilites for that room on the current date
        public List<SimpleSchedule> GetRoomSchedule_Today(int rmId, string _connectionString)
        {
            List<SimpleSchedule> schedule = new List<SimpleSchedule>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@RmId", rmId);

                schedule = connection.Query<SimpleSchedule>("up_GetRoomSchedule_Today", queryParameters ,commandType: System.Data.CommandType.StoredProcedure).AsList();
            }
            return schedule;
        }

        public IQueryable<SimpleSchedule> GetRoomSchedule(int rmId, string _connectionString)
        {
            IEnumerable<SimpleSchedule> schedule = new List<SimpleSchedule>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@RmId", rmId);

                schedule = connection.Query<SimpleSchedule>("up_GetRoomSchedule", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            return schedule.AsQueryable();
        }

        //returns availability list for room
        public IQueryable<RoomSchedule> GetRoomAvailability(int rmId, string _connectionString)
        {
            IEnumerable<RoomSchedule> schedule = new List<RoomSchedule>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@rmId", rmId);

                schedule = connection.Query<RoomSchedule>("up_GetRoomAvailability", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            return schedule.AsQueryable();
        }
        
        //add new availability for room
        public void InsertRoomAvailability(RoomSchedule availability, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@rmId", availability.rmId);
                queryParameters.Add("@avName", availability.avName);
                queryParameters.Add("@avDay", availability.avDay);
                queryParameters.Add("@avStartTime", availability.avStartTime);
                queryParameters.Add("@avEndTime", availability.avEndTime);

                connection.Execute("up_InsertAvailability", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }


        //change an existing availability
        public void EditRoomAvailability(RoomSchedule availability, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@avId", availability.avId);
                queryParameters.Add("@avName", availability.avName);
                queryParameters.Add("@avStartTime", availability.avStartTime);
                queryParameters.Add("@avEndTime", availability.avEndTime);

                connection.Execute("up_EditAvailability", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        //delete existing availability
        public void DeleteRoomAvailability(int avId, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@avId", avId);
                connection.Execute("up_DeleteAvailability", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        //
        //the below are the same but for events instead of availability
        //

        public IQueryable<RoomSchedule> GetRoomEvents(int rmId, string _connectionString)
        {
            IEnumerable<RoomSchedule> schedule = new List<RoomSchedule>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@rmId", rmId);

                schedule = connection.Query<RoomSchedule>("up_GetRoomEvents", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            return schedule.AsQueryable();
        }

        public void InsertRoomEvent(RoomSchedule roomEvent, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@rmId", roomEvent.rmId);
                queryParameters.Add("@evName", roomEvent.avName);
                queryParameters.Add("@evDate", roomEvent.evDate.ToShortDateString());
                queryParameters.Add("@evStartTime", roomEvent.avStartTime);
                queryParameters.Add("@evEndTime", roomEvent.avEndTime);

                connection.Execute("up_InsertEvent", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public void EditRoomEvent(RoomSchedule roomEvent, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@evId", roomEvent.avId);
                queryParameters.Add("@evName", roomEvent.avName);
                queryParameters.Add("@evDate", roomEvent.evDate);
                queryParameters.Add("@evStartTime", roomEvent.avStartTime);
                queryParameters.Add("@evEndTime", roomEvent.avEndTime);

                connection.Execute("up_EditEvent", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public void DeleteRoomEvent(int evId, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@evId", evId);
                connection.Execute("up_DeleteEvent", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

    }
}
