using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SeminarBuildingMap.Models
{
    //Holds functions that interact with SQL stored procedures concerning the Building Management Functionality of the project 
    public class BuildingDataAccessLayer
    {
        //This function gets all buildings on campus
        //(Comments will become more sparse the further down you go, as functionality is repeated and only needs explained the first time it is used)
        public IQueryable<Building> GetAllBuildings(string _connectionString) //connection string for the SeminarMap DB
        {
            IEnumerable<Building> buildingList = new List<Building>(); //This will function as a list structure to hold returned buildings
            using (SqlConnection connection = new SqlConnection(_connectionString)) //connects to SQL with given connection string
            {
                buildingList = connection.Query<Building>("up_GetBuildings", commandType: System.Data.CommandType.StoredProcedure); //Calls the up_GetBuilding Stored Procedure. Query<Building> returns a list of type Buildings, built from the entities in the DB
            }
            return buildingList.AsQueryable(); //converting the Enumerable to a Queryable Interface is necessary in order to bind the buildings into a datagrid
        }

        //Returns floors of an inputted building
        public IQueryable<Floor> GetBuildingFloors(string bdId, string _connectionString) //takes in ID of building that rooms are desired for
        {
            IEnumerable<Floor> buildingList = new List<Floor>(); 
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters(); //this is how we insert our function parameters as SQL parameters
                queryParameters.Add("@bdId", bdId); //sql parameters are denoted as strings with @ prefaced, as they are declared in SQL
                //you can see the queryParameters object has been passed into the Query function as an additional argument
                buildingList = connection.Query<Floor>("up_GetBuildingFloors", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            return buildingList.AsQueryable();
        }

        //Returns the name of a building based upon its ID 
        public string GetBuildingName(string bdId, string _connectionString)
        {
            string bdName = ""; //initialize string to return at end
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@bdId", bdId);
                //QuerySingle<String> will return the first row (only one will exist since bdId is a primary key and since only one column is being SELECTed we can store the single value as a simple string
                bdName = connection.QuerySingle<string>("up_GetBuildingName", queryParameters ,commandType: System.Data.CommandType.StoredProcedure);
            }
            return bdName;
        }

        //Same as above but with a floors name
        public string GetFloorName(string bdId, string flNo, string _connectionString)
        {
            string flName = "";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@bdId", bdId); //simply add multiple SQL parameters, since flNo is only unique within a building
                queryParameters.Add("@flNo", flNo); 
                flName = connection.QuerySingle<string>("up_GetFloorName", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            return flName;
        }

        //Takes in information to update floor name
        public void UpdateFloorName(string bdId, string flNo, string flName, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@bdId", bdId);
                queryParameters.Add("@flNo", flNo);
                queryParameters.Add("@flName", flName);
                //Execute is called for Delete, Update, Insert, etc where nothing needs to be outputted from SQL
                connection.Execute("up_UpdateFloorName", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        //Add new building
        public void AddBuilding(string bdId, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@bdId", bdId);
                connection.Execute("up_AddBuilding", queryParameters ,commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        //Add new floor
        public void AddFloor(string bdId, string flNo, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@bdId", bdId);
                queryParameters.Add("@flNo", flNo);
                connection.Execute("up_AddFloor", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        //Update Building Name
        public void UpdateBuilding(string bdId, string bdName, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@bdId", bdId);
                queryParameters.Add("@bdName", bdName);
                connection.Execute("up_UpdateBuilding", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        //Get Managers assigned buildings
        public IEnumerable<string> GetOwnedBuildings(string username, string _connectionString)
        {
            IEnumerable<string> buildingList = new List<string>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@username", username);
                buildingList = connection.Query<string>("up_GetOwnedBuildings", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            return buildingList;
        }

        //Get rid of a users assigned buildings
        public void ClearOwnedBuildings(string username, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@username", username);
                connection.Execute("up_ClearOwnedBuildings", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        //Give a user a new assigned building
        public void AddOwnedBuilding(string username, string bdId, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@username", username);
                queryParameters.Add("@bdId", bdId);
                connection.Execute("up_AddOwnedBuilding", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

    }
}
