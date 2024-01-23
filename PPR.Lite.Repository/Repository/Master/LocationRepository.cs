using PPR.Lite.Infrastructure.Helpers;
using PPR.Lite.Repository.IRepository;
using PPR.Lite.Repository.IRepository.Master;
using PPR.Lite.Shared.DataBank.Generic.Master;
using PPR.Lite.Shared.General;
using PPR.Lite.Shared.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.Repository.Master
{
         public  class LocationRepository: ILocationRepository
         {
       
            private readonly ISqlConnectionProvider _sql;
            public LocationRepository(ISqlConnectionProvider sql)
            {
                _sql = sql;
            }

            public async Task<Location> GetList(UserBase user, string TabIndex)
        {
            try
            {

                IDbDataParameter[] sqlparameters =
                  {
                 new SqlParameter("@CompanyGI",user.CompanyGuid),
                 new SqlParameter("@UserGuid",user.UserGuid),
                 new SqlParameter("@Type",TabIndex)

            };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_LOCATION_LIST]", sqlparameters);

                if (TabIndex == "APPROVAL_PENDING")
                    return LocationPendingList(ds);
                else
                    return LocationList(ds);
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }


        private Location LocationPendingList(DataSet dataSet)
        {

            Location model = new Location();
            List<Location> List = new List<Location>();
            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    model.ActiveCount = MakeSafe.ToSafeInt32(row["ActiveCount"]);
                    model.InactiveCount = MakeSafe.ToSafeInt32(row["InactiveCount"]);
                    model.PendingCount = MakeSafe.ToSafeInt32(row["PendingCount"]);

                }
            }
            if ((dataSet.Tables[1]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[1].Rows)
                {
                    var item = new Location();
                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.ShortName = MakeSafe.ToSafeString(row["ShortName"]);
                    item.Mode = MakeSafe.ToSafeString(row["EntryMode"]);
                    item.GI = MakeSafe.ToSafeGuid(row["LocationGI"]);
                    item.LogGI = MakeSafe.ToSafeGuid(row["LogGI"]);
                    item.MakerChecker = MakeSafe.ToSafeBool(row["MakerChecker"]);
                    item.EditActive = MakeSafe.ToSafeBool(row["EditActive"]);

                    List.Add(item);
                }
                model.LocationList = List;

            }

            return model;

        }

        private Location LocationList(DataSet dataSet)
        {
            Location model = new Location();
            List<Location> List = new List<Location>();
            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    model.ActiveCount = MakeSafe.ToSafeInt32(row["ActiveCount"]);
                    model.InactiveCount = MakeSafe.ToSafeInt32(row["InactiveCount"]);
                    model.PendingCount = MakeSafe.ToSafeInt32(row["PendingCount"]);

                }
            }

            if ((dataSet.Tables[1]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[1].Rows)
                {
                    var item = new Location();

                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.ShortName = MakeSafe.ToSafeString(row["ShortName"]);
                    item.GI = MakeSafe.ToSafeGuid(row["LocationGI"]);
                    item.Code = MakeSafe.ToSafeInt32(row["LocationCode"]);
                    item.MakerChecker = MakeSafe.ToSafeBool(row["MakerChecker"]);
                    item.EditActive = MakeSafe.ToSafeBool(row["EditActive"]);
                    List.Add(item);
                }
                model.LocationList = List;
            }

            return model;

        }

        public async Task<Location> GetLocationDetails(UserBase user, Guid LocationGI, Guid LogGI, string Type)
        {

            IDbDataParameter[] sqlParameters =
             {
                 new SqlParameter("@CompanyGI",user.CompanyGuid),
                 new SqlParameter("@LocationGI",LocationGI),
                 new SqlParameter("@LogGI",LogGI),
                 new SqlParameter("@Type",Type),

             };
            var ds = await _sql.GetDataSetAsync("[PPR].[SP_LOCATION_DETAILS]", sqlParameters);
            return GetLocation(ds);

        }
        private Location GetLocation(DataSet dataSet)
        {
            int Tablecount = dataSet.Tables.Count;
            Location model = new Location();
           
            model.Country = new Country();
            model.State = new State();
            model.District = new District();
            model.Region = new Region();
            model.LocationType = new LocationType();
            model.PTgroup = new PTGroup();
            model.ESIgroup = new ESIGroup();
            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {


                model.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Name"]);
                model.ShortName = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["ShortName"]);
                model.Active = MakeSafe.ToSafeBool(dataSet.Tables[0].Rows[0]["Active"]);
                model.DoorNumAndBuilding = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Address1"]);
                model.Street = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Address2"]);
                model.Area = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Address3"]);
                model.LocationType.Code = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["LocationTypeCode"]);
                model.LocationType.Name= MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["LocationTypeName"]);
                model.Region.Code = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["RegionCode"]);
                model.Region.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["RegionName"]);
                model.Country.Code = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["CountryCode"]);
                model.Country.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["CountryName"]);
                model.State.Code = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["StateCode"]);
                model.State.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["StateName"]);
                model.District.Code = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["DistrictCode"]);
                model.District.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["DistrictName"]);
                model.LandLineNum = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Phone"]);
                model.EmailId = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Email"]);
                model.WebAdderss = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["WebAddress"]);
                model.Latitude = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Latitude"]);
                model.Longitude = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Longitude"]);
                model.GeoAdderss = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["GeoAddress"]);
                model.PTgroup.Code = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["PtGroupCode"]);
                model.PTgroup.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["PtGroupName"]);
                model.ESIgroup.Code = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["EsiGroupCode"]);
                model.ESIgroup.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["EsiGroupName"]);
                model.Description = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Remarks"]);
                model.GI = MakeSafe.ToSafeGuid(dataSet.Tables[0].Rows[0]["LocationGI"]);
                model.LogGI = MakeSafe.ToSafeGuid(dataSet.Tables[0].Rows[0]["LogGI"]);
                model.Code = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["LocationCode"]);
            }

            //if (Tablecount == 2)
            //{

            //    if ((dataSet.Tables[1]?.Rows?.Count ?? 0) > 0)
            //    {

            //        model.GI = MakeSafe.ToSafeGuid(dataSet.Tables[1].Rows[0]["LocationGI"]);
            //    }
            //}
            return model;
        }

        async Task<ResponseEntity> ILocationRepository.Save(Location input)
        {
            try
            {

                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

                IDbDataParameter[] sqlParameters =
                {
            new SqlParameter("@JSONDATA",JsonHelper.ToJson(input)),
            ResponseKey,ResponseStatus
            };
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_LOCATION_SAVE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
            public async Task<ResponseEntity> ApproveOrReject(Location model)
            {

                try
                {

                    SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                    SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

                    IDbDataParameter[] sqlParameters =
                    {
            new SqlParameter("@JSONDATA",JsonHelper.ToJson(model)),
            ResponseKey,ResponseStatus
            };
                    await _sql.ExecuteNonQueryAsync("[PPR].[SP_LOCATION_APPROVE]", sqlParameters, CommandType.StoredProcedure);
                    return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
                }
                catch (SqlException ex)
                {
                    var ErrorMsg = ex.Message.ToString();
                    throw new Exception(ErrorMsg, ex);
                }

            }
        public async Task<ResponseEntity> Delete(UserBase user, Location model)
        {
            try
            {
                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

                IDbDataParameter[] sqlParameters =
                {

                    new SqlParameter("@CompanyGI",user.CompanyGuid),
                    new SqlParameter("@UserGI",user.UserGuid),
                    new SqlParameter("@LocationGI",model.GI),
                    ResponseKey,ResponseStatus
                };
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_LOCATION_DELETE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));

            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

    }
}
