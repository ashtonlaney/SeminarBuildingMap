using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace SeminarBuildingMap.Models
{
    public class RoomDataAccessLayer
    {
        public IEnumerable<Room> GetDardenRooms(string _connectionString)
        {
            IEnumerable<Room> rooms = new List<Room>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                rooms = connection.Query<Room>("up_GetRooms_Darden", commandType: System.Data.CommandType.StoredProcedure);
            }
            return rooms;
        }
    }
}
