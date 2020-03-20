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

    }
}
