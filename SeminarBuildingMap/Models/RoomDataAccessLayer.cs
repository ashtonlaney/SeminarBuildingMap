using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SeminarBuildingMap.Areas.Identity.Data;

namespace SeminarBuildingMap.Models
{
    public class RoomDataAccessLayer
    {
        //returns all rooms in a building/floor
        public IEnumerable<Room> GetSelectedRooms(string bdId, string flNo, string _connectionString)
        {
            IEnumerable<Room> rooms = new List<Room>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@bdId", bdId);
                queryParameters.Add("@flNo", flNo);
                //creates a list of the "Room" object, the variables in this class must correspond to sql column names or this will not work
                rooms = connection.Query<Room>("up_GetSelectedRooms", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            return rooms;
        }

        //does the same as above, but without the expensive availability calculation, used for admin room editor
        public IEnumerable<Room> GetSelectedRooms_NoAvailability(string bdId, string flNo, string _connectionString)
        {
            IEnumerable<Room> rooms = new List<Room>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@bdId", bdId);
                queryParameters.Add("@flNo", flNo);
                //creates a list of the "Room" object, the variables in this class must correspond to sql column names or this will not work
                rooms = connection.Query<Room>("up_GetSelectedRooms_NoAvailability", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            return rooms;
        }

        //adds a new room to specified building/floor
        public bool AddRoom(string rmNo, string bdId, string flNo, string rmCoords, string _connectionString)
        {
            rmCoords = rmCoords.Remove(rmCoords.Length - 1); //gets rid of the ending semicolon
            string[] coordsArray = rmCoords.Split(";"); //split the 4 coordinates into an array
            if (coordsArray.Length != 4) //makes sure exactly four corners are specified
            {
                return false; //returns success handling so user can be told if the add was successful
            }
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //this is how you add SQL parameters, this splits the coords entered into the 4 corners of an object
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@rmNo", rmNo);
                queryParameters.Add("@bdId", bdId);
                queryParameters.Add("@flNo", flNo);
                queryParameters.Add("@rmTopLeftPoint", coordsArray[0]);
                queryParameters.Add("@rmBottomLeftPoint", coordsArray[1]);
                queryParameters.Add("@rmBottomRightPoint", coordsArray[2]);
                queryParameters.Add("@rmTopRightPoint", coordsArray[3]);
                connection.Execute("up_AddRoom", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            return true;
        }
        
        //updates room Number
        public void UpdateRoom(string rmId, string rmNo, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //this is how you add SQL parameters, this splits the coords entered into the 4 corners of an object
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@rmId", rmId);
                queryParameters.Add("@rmNo", rmNo);
                connection.Execute("up_UpdateRoom", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        //delets a room
        public void DeleteRoom(string rmId, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //this is how you add SQL parameters, this splits the coords entered into the 4 corners of an object
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@rmId", rmId);
                connection.Execute("up_DeleteRoom", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        //return rooms a user can edit, this is different depending on if a user is an Admin, Manager, or Faculty member (admin has all, manager looks for their buildings, faculty gets specific rooms)
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

        //returns Room object which contains info about specified room
        public Room GetRoomInfo(int rmId, string _connectionString)
        {
            Room room;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                room = connection.QuerySingle<Room>("up_GetRoomInfo", new { rmId }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return room;
        }

        //Authentication function, it checks that a user can edit a room before doing so
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
                } else if(result == rmId.ToString()) //the function returns the requested roomId if successful
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //updates a room's name
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

        //updates a room's name and type, this is the version of the above function, that is available to Admins
        public void UpdateRoomNameType(int rmId, string rmName, string rmType, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@rmId", rmId);
                queryParameters.Add("@rmName", rmName);
                queryParameters.Add("@rmType", rmType);
                int result = connection.Execute("up_UpdateRoomNameType", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        //gets all users with permissions over a room
        public IQueryable<SeminarBuildingMapUser> GetOwnedUsers(int rmId, string _connectionString)
        {
            IEnumerable<SeminarBuildingMapUser> schedule = new List<SeminarBuildingMapUser>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@rmId", rmId);

                schedule = connection.Query<SeminarBuildingMapUser>("up_GetRoomOwnedUsers", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            return schedule.AsQueryable();
        }

        //assign a room to the specified user
        public void AddOwnedRooms(int rmId, string username, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@rmId", rmId);
                queryParameters.Add("@username", username);

                connection.Execute("up_AddOwnedRooms", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        //delete an assigned room from a user
        public void DeleteOwnedRooms(int rmId, string username, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@rmId", rmId);
                queryParameters.Add("@username", username);

                connection.Execute("up_DeleteOwnedRooms", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        //gets all rooms in a building, used for the public schedule viewer
        public IQueryable<Room> GetBuildingRooms(string bdId, string _connectionString)
        {
            IEnumerable<Room> roomList = new List<Room>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@bdId", bdId);
                roomList = connection.Query<Room>("up_GetBuildingRooms", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            return roomList.AsQueryable();
        }
        
        //get all users that can be modified (non admins)
        public IQueryable<SeminarBuildingMapUser> GetSubusers(string _connectionString)
        {
            IEnumerable<SeminarBuildingMapUser> userList = new List<SeminarBuildingMapUser>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                userList = connection.Query<SeminarBuildingMapUser>("up_GetSubusers", commandType: System.Data.CommandType.StoredProcedure);
            }
            return userList.AsQueryable();
        }

    }
}
