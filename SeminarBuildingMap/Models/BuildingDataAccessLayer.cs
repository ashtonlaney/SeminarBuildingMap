using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SeminarBuildingMap.Models
{
    public class BuildingDataAccessLayer
    {
        public IQueryable<Building> GetAllBuildings(string _connectionString)
        {
            IEnumerable<Building> buildingList = new List<Building>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                buildingList = connection.Query<Building>("up_GetBuildings", commandType: System.Data.CommandType.StoredProcedure);
            }
            return buildingList.AsQueryable();
        }

        public IQueryable<Floor> GetBuildingFloors(string bdId, string _connectionString)
        {
            IEnumerable<Floor> buildingList = new List<Floor>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@bdId", bdId);
                buildingList = connection.Query<Floor>("up_GetBuildingFloors", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            return buildingList.AsQueryable();
        }

        public void AddBuilding(string bdId, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@bdId", bdId);
                connection.Execute("up_AddBuilding", queryParameters ,commandType: System.Data.CommandType.StoredProcedure);
            }
        }

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

        public void ClearOwnedBuildings(string username, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@username", username);
                connection.Execute("up_ClearOwnedBuildings", queryParameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

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
