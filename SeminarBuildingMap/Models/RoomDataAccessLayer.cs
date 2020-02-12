using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;

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

        public void InsertDardenRoom(string rmID, string rmCoords, string _connectionString)
        {
            rmCoords = rmCoords.Remove(rmCoords.Length - 1);
            string[] coordsArray = rmCoords.Split(";");
            if (coordsArray.Length != 4)
            {
                return;
            }
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@rmId", rmID);
                queryParameters.Add("@rmName", "Darden " + rmID);
                queryParameters.Add("@rmTopLeftPoint", coordsArray[0]);
                queryParameters.Add("@rmBottomLeftPoint", coordsArray[1]);
                queryParameters.Add("@rmBottomRightPoint", coordsArray[2]);
                queryParameters.Add("@rmTopRightPoint", coordsArray[3]);
                connection.Execute("up_InsertRoom_Darden", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }


    }
}
