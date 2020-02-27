using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SeminarBuildingMap.Models
{
    public class RoomDataAccessLayer
    {
        //returns all rooms in darden, will be changed to be dynamic later
        public IEnumerable<Room> GetSelectedRooms(string bdId, string rmFloorNo, string _connectionString)
        {
            IEnumerable<Room> rooms = new List<Room>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@bdId", bdId);
                queryParameters.Add("@rmFloorNo", rmFloorNo);
                //creates a list of the "Room" object, the variables in this class must correspond to sql column names or this will not work
                rooms = connection.Query<Room>("up_GetSelectedRooms", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            return rooms;
        }

        //adds a new room to darden, this will be changed when an eventual admin page for this is created
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
                //this is how you add SQL parameters, this splits the coords entered into the 4 corners of an object
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

        //return rooms owned by user
        public IQueryable<Room> GetOwnedRooms(string username, string userRole, string _connectionString)
        {
            IEnumerable<Room> roomList = new List<Room>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@username", username);
                queryParameters.Add("@userRole", userRole);
                roomList = connection.Query<Room>("up_GetOwnedRooms", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            return roomList.AsQueryable();
        }

        public Room GetRoomInfo(int rmId, string _connectionString)
        {
            Room room;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                room = connection.QuerySingle<Room>("up_GetRoomInfo", new { rmId }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return room;
        }

        public bool UserCanEditRoom(string UserName, int rmId, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@rmId", rmId);
                queryParameters.Add("@UserName", UserName);
                string result = connection.ExecuteScalar<string>("up_UserCanEditRoom", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                if (string.IsNullOrEmpty(result))
                {
                    return false;
                } else if(result == rmId.ToString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void UpdateRoomName(int rmId, string rmName, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@rmId", rmId);
                queryParameters.Add("@rmName", rmName);
                int result = connection.Execute("up_UpdateRoomName", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

    }
}
