using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SeminarBuildingMap.Models
{
    public class RoomScheduleDataAccessLayer
    {
        //this function takes in a room and returns all events/availabilites for that room on the current date
        public List<RoomSchedule> GetRoomSchedule_Today(string rmId, string _connectionString)
        {
            List<RoomSchedule> schedule = new List<RoomSchedule>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@RmId", rmId);

                schedule = connection.Query<RoomSchedule>("up_GetRoomSchedule_Today", queryParameters ,commandType: System.Data.CommandType.StoredProcedure).AsList();
            }
            return schedule;
        }

        public IQueryable<RoomSchedule> GetRoomSchedule(int rmId, string _connectionString)
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

        public void InsertRoomAvailability(RoomSchedule availability, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@rmId", availability.rmId);
                queryParameters.Add("@avName", availability.avName);
                queryParameters.Add("@avDay", availability.avDay);
                queryParameters.Add("@avStartTime", availability.UstStartTime);
                queryParameters.Add("@avEndTime", availability.UstEndTime);

                connection.Execute("up_InsertAvailability", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public void EditRoomAvailability(RoomSchedule availability, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@rmId", availability.rmId);
                queryParameters.Add("@avName", availability.avName);
                queryParameters.Add("@avStartTime", availability.UstStartTime);
                queryParameters.Add("@avEndTime", availability.UstEndTime);

                connection.Execute("up_EditAvailability", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

    }
}
