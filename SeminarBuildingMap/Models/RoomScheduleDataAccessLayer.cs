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
    }
}
