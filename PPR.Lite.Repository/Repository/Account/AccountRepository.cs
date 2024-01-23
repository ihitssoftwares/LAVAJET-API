using PPR.Lite.Infrastructure.Helpers;
using PPR.Lite.Infrastructure.ISecurity.Abstract;
using PPR.Lite.Repository.IRepository;
using PPR.Lite.Repository.IRepository.Account;
using PPR.Lite.Shared.Account;
using PPR.Lite.Shared.DataBank.DTO;
using PPR.Lite.Shared.General;
//using PPR.Lite.Shared.Master;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text;

namespace PPR.Lite.Repository.Repository.Account
{
    public class AccountRepository : IAccountRepository
    {
        private ITokenRepository _tokenRepository;
        private readonly ISqlConnectionProvider _sql;
        private readonly IHash _hash;

        public AccountRepository(ITokenRepository tokenRepository, ISqlConnectionProvider sql, IHash hash)
        {
            _tokenRepository = tokenRepository;
            _sql = sql;
            _hash = hash;
        }

        public async Task<CustomSoftware> GetSoftwareLogo()
        {
            CustomSoftware CustomSoftware = new CustomSoftware();
            CustomSoftware.SoftwareLogo = new File();
            CustomSoftware.SoftwareName = new File();
            CustomSoftware.HRMS_SoftwareName = new File();
            try
            {

                var dataSet = await _sql.GetDataSetAsync("[PPR].[SP_GET_SOFTWARE_LOGO]");

                if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        if (row["LogoInBytes"].ToString() != null)
                        {
                            CustomSoftware.SoftwareLogo.fdata = (byte[])(row["LogoInBytes"]);
                        }
                        if (row["SoftwareNameInBytes"].ToString() != "")
                        {
                            CustomSoftware.SoftwareName.FileName = MakeSafe.ToSafeString(row["ESSPFilename"]);
                            CustomSoftware.SoftwareName.fdata = (byte[])(row["SoftwareNameInBytes"]);
                        }
                        if (row["HRMSSoftwareNameInBytes"].ToString() != "")
                        {
                            CustomSoftware.HRMS_SoftwareName.FileName = MakeSafe.ToSafeString(row["HRMSFilename"]);
                            CustomSoftware.HRMS_SoftwareName.fdata = (byte[])(row["HRMSSoftwareNameInBytes"]);
                        }
                    }
                }
                return CustomSoftware;
            }

            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }

        }

        public async Task<ResponseEntity<string>> Authenticate(Login credentials)
        {
            try
            {
                IDbDataParameter[] sqlParameters =
              {
                new SqlParameter("@UserName",credentials.UserCode),
                new SqlParameter("@CompanyName",credentials.CompanyCode)
            };
                var dt = await _sql.GetDataTableAsync("[HRMS].[SP_APP_USER_DETAILS]", sqlParameters, CommandType.StoredProcedure);
                var userdtls = PopulateAuthUserDetails(dt);
                //Check the user is Authenticated onre or not ,if Yes Generate Token

                //Test login hashing
                var loginAttempt = new LoginAttempt
                {
                    UserCode = credentials.UserCode,
                    Password = credentials.Password,
                    CompanyCode = credentials.CompanyCode
                };

                var currentUser = userdtls;
                credentials.CompanyName = userdtls.authUser.CompanyName;
                credentials.LoginStatus = userdtls.LoginStatus;
                var isAuth = _hash.ComparePasswordHash(credentials.Password, currentUser.authUser.PasswordHash, currentUser.authUser.PasswordSalt);
                if (!isAuth)
                {
                    loginAttempt.IsSuccessful = false;
                    loginAttempt.Message = "Invalid password";
                    return new ResponseEntity<string>("Invalid Credentials", false, "null");
                }
                if (credentials.software == "HRMS" && userdtls.authUser.UserType == "ESSP")
                    return new ResponseEntity<string>("Sorry.You Dont have permission to access the HRMS", false, "null");
                if (credentials.software == "ESSP" && userdtls.authUser.UserType == "HRMS")
                    return new ResponseEntity<string>("Sorry.You Dont have permission to access the ESSP", false, "null");

                else
                {

                    credentials.authUser = userdtls.authUser;
                    string jsonData = JsonConvert.SerializeObject(credentials);
                    return new ResponseEntity<string>("Logged in Successfully", true, jsonData);
                }

                
            }
            catch (Exception e)
            {
                return new ResponseEntity<string>("Invalid Credentials", false, "null");
            }
        }

        public async Task<ResponseEntity<string>> Login(Login credentials)
        {
            try
            {
                IDbDataParameter[] sqlParameters =
              {
                new SqlParameter("@UserName",credentials.UserCode),
                new SqlParameter("@CompanyName",credentials.CompanyCode),
                 new SqlParameter("@GroupCompanyCode",credentials.GroupCompanyCode)
            };
                var dt = await _sql.GetDataTableAsync("[PPR].[SP_APP_USER_LOGIN]", sqlParameters, CommandType.StoredProcedure);
                var userdtls = PopulateLoginDetails(dt);
                //Check the user is Authenticated onre or not ,if Yes Generate Token

                //Test login hashing
                var loginAttempt = new LoginAttempt
                {
                    UserCode = credentials.UserCode,
                    Password = credentials.Password,
                    CompanyCode = credentials.CompanyCode,
                    GroupCompanyCode = credentials.GroupCompanyCode
                };

                var currentUser = userdtls;
                credentials.CompanyName = userdtls.CompanyName;

                var token = "";
                var isAuth = _hash.ComparePasswordHash(credentials.Password, currentUser.authUser.PasswordHash, currentUser.authUser.PasswordSalt);

             

                if (!isAuth)
                {
                    loginAttempt.IsSuccessful = false;
                    loginAttempt.Message = "Invalid password";
                    return new ResponseEntity<string>("Invalid Credentials", false, "null");
                }
                if (credentials.software == "HRMS" && userdtls.authUser.UserType == "ESSP")
                    return new ResponseEntity<string>("Sorry.You Dont have permission to access the HRMS", false, "null");
                if (credentials.software == "ESSP" && userdtls.authUser.UserType == "HRMS")
                    return new ResponseEntity<string>("Sorry.You Dont have permission to access the ESSP", false, "null");

                //------------------Token Generation Starts----------------------------------------------------
                else
                {
                    UserBase user = new UserBase();

                    user = userdtls.authUser.User;            
                    var tokenSettings = new AppTokenSettings<UserBase>
                    {
                        IssuedAt = DateTime.Now,
                        Type = "LOGIN",
                        Data = user
                    };
                    token = _tokenRepository.GenerateToken(tokenSettings);
                    Console.WriteLine("Success");
                    credentials.Token = token;
                    credentials.authUser = userdtls.authUser;
                    string jsonData = JsonConvert.SerializeObject(credentials);
                    return new ResponseEntity<string>("Logged in Successfully", true, jsonData);
                }
                //------------------Token Generation ENDS----------------------------------------------------
            }
            catch (Exception e)
            {
                return new ResponseEntity<string>("Invalid Credentials", false, "null");
            }
        }

        //public async Task<ResponseEntity<JObject>> Mobile_Authenticate(Login mob_credentials)
        //{
        //    //# Mobile Login Authenticate    
        //    try
        //    {
        //        SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 250, Direction = ParameterDirection.Output };
        //        SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

        //        IDbDataParameter[] sqlParameters =
        //      {
        //        new SqlParameter("@UserName",mob_credentials.UserCode),
        //        new SqlParameter("@CompanyName",mob_credentials.CompanyCode),
        //        new SqlParameter("@DeviceId",mob_credentials.DeviceData.DeviceId),
        //        new SqlParameter("@DeviceModel",mob_credentials.DeviceData.DeviceModel),
        //        new SqlParameter("@AppVersion",mob_credentials.DeviceData.AppVersion),
        //        new SqlParameter("@FcmID",mob_credentials.DeviceData.FcmId),
        //          ResponseKey,ResponseStatus

        //    };
        //        var dt = await _sql.GetDataTableAsync("[MOBILE].[SP_MOBILE_LOGIN_USER_DETAILS]", sqlParameters, CommandType.StoredProcedure);
        //        var userdtls = PopulateMobileAuthUserDetails(dt);
        //        //Check the user is Authenticated onre or not ,if Yes Generate Token

        //        //Test login hashing
        //        var loginAttempt = new LoginAttempt
        //        {
        //            UserCode = mob_credentials.UserCode,
        //            Password = mob_credentials.Password,
        //            CompanyCode = mob_credentials.CompanyCode
        //        };
          
        //        var currentUser = userdtls;
        //        //mob_credentials.CompanyName = userdtls.CompanyName;
        //        var token = "";
        //        var isAuth = _hash.ComparePasswordHash(mob_credentials.Password, currentUser.authUser.PasswordHash, currentUser.authUser.PasswordSalt);
        //        if (!isAuth)
        //        {
        //            loginAttempt.IsSuccessful = false;
        //            loginAttempt.Message = "Invalid password";
        //            return new ResponseEntity<JObject>("Invalid Credentials", false, null);
        //        }
        //        //if (mob_credentials.software == "HRMS" && userdtls.authUser.UserType == "ESSP")
        //        //    return new ResponseEntity<JObject>("Sorry.You Dont have permission to access the HRMS", false, JsonObjectNull);
        //        //if (mob_credentials.software == "ESSP" && userdtls.authUser.UserType == "HRMS")
        //        //    return new ResponseEntity<JObject>("Sorry.You Dont have permission to access the ESSP", false, JsonObjectNull);

        //        if (Convert.ToBoolean(ResponseStatus.Value) == false)
        //        {
        //            loginAttempt.IsSuccessful = false;
        //            loginAttempt.Message = ResponseKey.Value.ToString();
        //            return new ResponseEntity<JObject>(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value), null);
        //        }
        //        //------------------Token Generation Starts----------------------------------------------------
        //        else
        //        {
        //            Login ObjResult = new Login();
        //            UserBase user = new UserBase();
        //            user = userdtls.authUser.User;
        //            var tokenSettings = new AppTokenSettings<UserBase>
        //            {
        //                IssuedAt = DateTime.Now,
        //                Type = "MOBILE_LOGIN",
        //                Data = user
        //            };
        //            token = _tokenRepository.GenerateToken(tokenSettings);
        //            Console.WriteLine("Success");                  
        //            ObjResult.authUser = userdtls.authUser;
        //            ObjResult.Token = token;
        //            //ObjResult.authUser.Token = token;
        //            JObject jsonData = new JObject();
        //            jsonData = JObject.Parse(JsonConvert.SerializeObject(ObjResult));                  
        //            return new ResponseEntity<JObject>("Logged in Successfully", true, jsonData);               
        //        }
        //        //------------------Token Generation ENDS----------------------------------------------------
        //    }
        //    catch (Exception e)
        //    {
        //        return new ResponseEntity<JObject>("Invalid Credentials", false, null);
        //    }
        //}


        //public async Task<ResponseEntity> MobileLogHistory(UserBase UserBase,Login model)
        //{
        //    try
        //    {
        //        SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 250, Direction = ParameterDirection.Output };
        //        SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

        //        IDbDataParameter[] sqlParameters =
        //        {
        //            new SqlParameter("@CompanyGI",UserBase.CompanyGuid),
        //            new SqlParameter("@UserGI",UserBase.UserGuid),
        //            new SqlParameter("@Software",model.software),
        //            new SqlParameter("@InOutStatus",model.INOUT),                 
        //            new SqlParameter("@DeviceId",model.DeviceData.DeviceId),
        //            new SqlParameter("@DeviceModel",model.DeviceData.DeviceModel),
        //            new SqlParameter("@AppVersion",model.DeviceData.AppVersion),
        //            new SqlParameter("@FcmId",model.DeviceData.FcmId),
        //            ResponseKey,ResponseStatus

        //    };
        //        await _sql.ExecuteNonQueryAsync("[MOBILE].[SP_MOBILE_LOGIN_HISTORY]", sqlParameters, CommandType.StoredProcedure);
        //        return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
        //    }
        //    catch (SqlException ex)
        //    {
        //        var ErrorMsg = ex.Message.ToString();
        //        throw new Exception(ErrorMsg, ex);
        //    }
        //}



        #region Private
        private Login PopulateAuthUserDetails(DataTable dt)
        {
            var item = new Login();
            item.authUser = new AuthUser();
            if ((dt.Rows?.Count ?? 0) > 0)
            {
                //item.User = new Shared.Generic.Admin.AppUser
                //{
                UserBase User = new UserBase()
                {
                    CompanyGuid = MakeSafe.ToSafeGuid(dt.Rows[0]["CompanyGuid"]),
                    UserGuid = MakeSafe.ToSafeGuid(dt.Rows[0]["UserGuid"])
                };
                item.authUser.EmployeeGI = MakeSafe.ToSafeGuid(dt.Rows[0]["EmployeeGI"]);
                item.authUser.EmployeeID = MakeSafe.ToSafeString(dt.Rows[0]["EmployeeID"]);
                item.authUser.EmployeeCode = MakeSafe.ToSafeInt32(dt.Rows[0]["EmployeeCode"]);
                item.authUser.ImageUrl = MakeSafe.ToSafeString(dt.Rows[0]["ImageUrl"]);
                item.authUser.Designation = MakeSafe.ToSafeString(dt.Rows[0]["Designation"]);
                item.authUser.Department = MakeSafe.ToSafeString(dt.Rows[0]["Department"]);
                item.authUser.CompanyName = MakeSafe.ToSafeString(dt.Rows[0]["CompanyName"]);
                item.authUser.IsActive = MakeSafe.ToSafeBool(dt.Rows[0]["IsActive"]);
                item.authUser.EmployeeName = MakeSafe.ToSafeString(dt.Rows[0]["FullName"]);
                item.authUser.User = User;
                item.authUser.PasswordHash = MakeSafe.ToSafeString(dt.Rows[0]["PasswordHash"]);
                item.authUser.PasswordSalt = MakeSafe.ToSafeString(dt.Rows[0]["PasswordSalt"]);
                item.authUser.UserRole = MakeSafe.ToSafeString(dt.Rows[0]["UserRole"]);
                item.authUser.UserRoleCode = MakeSafe.ToSafeInt32(dt.Rows[0]["UserRoleCode"]);
                item.authUser.IsNewLogin = MakeSafe.ToSafeBool(dt.Rows[0]["IsNewLogin"]);
                item.authUser.CompanyID = MakeSafe.ToSafeInt32(dt.Rows[0]["companyID"]);
                item.authUser.UserCode = MakeSafe.ToSafeInt32(dt.Rows[0]["userCode"]);
                
                item.authUser.DashBoardRoute = MakeSafe.ToSafeString(dt.Rows[0]["DashBoardRoute"]); 
                //item.MasterPermission = new Shared.Generic.Admin.Permission
                //{
                //    Approve = MakeSafe.ToSafeBool(dt.Rows[0]["Approve"]),
                //    Create = MakeSafe.ToSafeBool(dt.Rows[0]["Create"]),
                //    Delete = MakeSafe.ToSafeBool(dt.Rows[0]["Delete"]),
                //    Edit = MakeSafe.ToSafeBool(dt.Rows[0]["Edit"])
                //};
                //item.Role = new Shared.Generic.Admin.Role
                //{
                //    GI = MakeSafe.ToSafeGuid(dt.Rows[0]["RoleGI"]),
                //    Name = MakeSafe.ToSafeString(dt.Rows[0]["RoleName"])
                //};
                item.authUser.UserType = MakeSafe.ToSafeString(dt.Rows[0]["UserType"]);
                //item.DefaultDashboard = MakeSafe.ToSafeString(dt.Rows[0]["DefaultDashboard"]);
                item.LoginStatus = MakeSafe.ToSafeString(dt.Rows[0]["LoginStatus"]);
                return item;
            }
            return null;
        }

        private Login PopulateMobileAuthUserDetails(DataTable dt)
        {
            var item = new Login();
            item.authUser = new AuthUser();
            if ((dt.Rows?.Count ?? 0) > 0)
            {
                //item.User = new Shared.Generic.Admin.AppUser
                //{
                UserBase User = new UserBase()
                {
                    CompanyGuid = MakeSafe.ToSafeGuid(dt.Rows[0]["CompanyGuid"]),
                    UserGuid = MakeSafe.ToSafeGuid(dt.Rows[0]["UserGuid"])
                };
                item.authUser.EmployeeGI = MakeSafe.ToSafeGuid(dt.Rows[0]["EmployeeGI"]);
                item.authUser.EmployeeID = MakeSafe.ToSafeString(dt.Rows[0]["EmployeeID"]);
                item.authUser.EmployeeCode = MakeSafe.ToSafeInt32(dt.Rows[0]["EmployeeCode"]);
                item.authUser.ImageUrl = MakeSafe.ToSafeString(dt.Rows[0]["ImageUrl"]);
                item.authUser.Designation = MakeSafe.ToSafeString(dt.Rows[0]["Designation"]);
                item.authUser.Department = MakeSafe.ToSafeString(dt.Rows[0]["Department"]);
                item.authUser.CompanyName = MakeSafe.ToSafeString(dt.Rows[0]["CompanyName"]);
                item.authUser.IsActive = MakeSafe.ToSafeBool(dt.Rows[0]["IsActive"]);
                item.authUser.EmployeeName = MakeSafe.ToSafeString(dt.Rows[0]["FullName"]);
                item.authUser.User = User;
                item.authUser.PasswordHash = MakeSafe.ToSafeString(dt.Rows[0]["PasswordHash"]);
                item.authUser.PasswordSalt = MakeSafe.ToSafeString(dt.Rows[0]["PasswordSalt"]);
                item.authUser.UserRole = MakeSafe.ToSafeString(dt.Rows[0]["UserRole"]);
                item.authUser.UserRoleCode = MakeSafe.ToSafeInt32(dt.Rows[0]["UserRoleCode"]);
                item.authUser.IsNewLogin = MakeSafe.ToSafeBool(dt.Rows[0]["IsNewLogin"]);
                item.authUser.CompanyID = MakeSafe.ToSafeInt32(dt.Rows[0]["companyID"]);
                item.authUser.UserCode = MakeSafe.ToSafeInt32(dt.Rows[0]["userCode"]);

                item.authUser.DashBoardRoute = MakeSafe.ToSafeString(dt.Rows[0]["DashBoardRoute"]);
                //item.MasterPermission = new Shared.Generic.Admin.Permission
                //{
                //    Approve = MakeSafe.ToSafeBool(dt.Rows[0]["Approve"]),
                //    Create = MakeSafe.ToSafeBool(dt.Rows[0]["Create"]),
                //    Delete = MakeSafe.ToSafeBool(dt.Rows[0]["Delete"]),
                //    Edit = MakeSafe.ToSafeBool(dt.Rows[0]["Edit"])
                //};
                //item.Role = new Shared.Generic.Admin.Role
                //{
                //    GI = MakeSafe.ToSafeGuid(dt.Rows[0]["RoleGI"]),
                //    Name = MakeSafe.ToSafeString(dt.Rows[0]["RoleName"])
                //};
                item.authUser.UserType = MakeSafe.ToSafeString(dt.Rows[0]["UserType"]);
                //item.DefaultDashboard = MakeSafe.ToSafeString(dt.Rows[0]["DefaultDashboard"]);
                //item.LoginStatus = MakeSafe.ToSafeString(dt.Rows[0]["LoginStatus"]);
                return item;
            }
            return null;
        }
        private Login PopulateLoginDetails(DataTable dt)
        {
            var item = new Login();
            item.authUser = new AuthUser();
            if ((dt.Rows?.Count ?? 0) > 0)
            {
                //item.User = new Shared.Generic.Admin.AppUser
                //{
                UserBase User = new UserBase()
                {
                    GroupCompanyGI = MakeSafe.ToSafeGuid(dt.Rows[0]["GroupCompanyGI"]),
                    CompanyGuid = MakeSafe.ToSafeGuid(dt.Rows[0]["CompanyGuid"]),
                    UserGuid = MakeSafe.ToSafeGuid(dt.Rows[0]["UserGuid"])
                };
                item.authUser.CompanyName = MakeSafe.ToSafeString(dt.Rows[0]["CompanyName"]);
                item.authUser.EmployeeGI = MakeSafe.ToSafeGuid(dt.Rows[0]["EmployeeGI"]);
                item.authUser.EmployeeID = MakeSafe.ToSafeString(dt.Rows[0]["EmployeeID"]);
                item.authUser.EmployeeCode = MakeSafe.ToSafeInt32(dt.Rows[0]["EmployeeCode"]);
                item.authUser.ImageUrl = MakeSafe.ToSafeString(dt.Rows[0]["ImageUrl"]);
                item.authUser.Designation = MakeSafe.ToSafeString(dt.Rows[0]["Designation"]);
                item.authUser.Department = MakeSafe.ToSafeString(dt.Rows[0]["Department"]);
                item.authUser.GroupCompanyName = MakeSafe.ToSafeString(dt.Rows[0]["GroupCompanyName"]);
                item.authUser.GroupCompanyShortName = MakeSafe.ToSafeString(dt.Rows[0]["GroupCompanyShortName"]);
                item.authUser.IsActive = MakeSafe.ToSafeBool(dt.Rows[0]["IsActive"]);
                item.authUser.EmployeeName = MakeSafe.ToSafeString(dt.Rows[0]["FullName"]);
                item.authUser.User = User;
                item.authUser.PasswordHash = MakeSafe.ToSafeString(dt.Rows[0]["PasswordHash"]);
                item.authUser.PasswordSalt = MakeSafe.ToSafeString(dt.Rows[0]["PasswordSalt"]);
                item.authUser.UserRole = MakeSafe.ToSafeString(dt.Rows[0]["UserRole"]);
                item.authUser.UserRoleCode = MakeSafe.ToSafeInt32(dt.Rows[0]["UserRoleCode"]);
                item.authUser.IsNewLogin = MakeSafe.ToSafeBool(dt.Rows[0]["IsNewLogin"]);
                item.authUser.CompanyID = MakeSafe.ToSafeInt32(dt.Rows[0]["companyID"]);
                item.authUser.UserCode = MakeSafe.ToSafeInt32(dt.Rows[0]["userCode"]);

                item.authUser.DashBoardRoute = MakeSafe.ToSafeString(dt.Rows[0]["DashBoardRoute"]);
                //item.MasterPermission = new Shared.Generic.Admin.Permission
                //{
                //    Approve = MakeSafe.ToSafeBool(dt.Rows[0]["Approve"]),
                //    Create = MakeSafe.ToSafeBool(dt.Rows[0]["Create"]),
                //    Delete = MakeSafe.ToSafeBool(dt.Rows[0]["Delete"]),
                //    Edit = MakeSafe.ToSafeBool(dt.Rows[0]["Edit"])
                //};
                //item.Role = new Shared.Generic.Admin.Role
                //{
                //    GI = MakeSafe.ToSafeGuid(dt.Rows[0]["RoleGI"]),
                //    Name = MakeSafe.ToSafeString(dt.Rows[0]["RoleName"])
                //};
                item.authUser.UserType = MakeSafe.ToSafeString(dt.Rows[0]["UserType"]);
                //item.DefaultDashboard = MakeSafe.ToSafeString(dt.Rows[0]["DefaultDashboard"]);

                return item;
            }
            return null;
        }
        #endregion
        public async Task<ResponseEntity> LogHistory(UserBase UserBase, string CompanyCode, string UserCode, bool LoginSuccess,string Software, string InOutStatus, string MacAddress)
        {
            try
            {

                IDbDataParameter[] sqlParameters =
                {
                    new SqlParameter("@CompanyGI",UserBase.CompanyGuid),
                    new SqlParameter("@UserGI",UserBase.UserGuid),
                    new SqlParameter("@Software",Software),
                    new SqlParameter("@InOutStatus",InOutStatus),
                    new SqlParameter("@MacAddress",MacAddress),
                     new SqlParameter("@CompanyName",CompanyCode),
                      new SqlParameter("@UserName",UserCode),
                       new SqlParameter("@LoginSuccess",LoginSuccess)

            };
                await _sql.ExecuteNonQueryAsync("[SYSTEM].[SP_LOGIN_HISTORY]", sqlParameters, CommandType.StoredProcedure);
                return null;
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        //public async Task<UserRoleDetails> GetLogOutUserRoleDetails(UserBase user, int UserRoleCode)
        //{

        //    IDbDataParameter[] sqlParameters =
        //     {
        //        new SqlParameter("@CompanyGI",user.CompanyGuid),
        //        new SqlParameter("@UserGI",user.UserGuid),
        //        new SqlParameter("@UserRoleCode",UserRoleCode),
        //    };
        //    var ds = await _sql.GetDataSetAsync("[HRMS].[SP_LOGOUT_USER_ROLE_DETAILS]", sqlParameters);
        //    return PopulateUserDetails(ds);

        //}

        //private UserRoleDetails PopulateUserDetails(DataSet dataSet)
        //{
        //    UserRoleDetails model = new UserRoleDetails();
        //    List<Designation> DesignationList = new List<Designation>();
        //    List<Location> LocationList = new List<Location>();
        //    List<Region> RegionList = new List<Region>();
        //    List<ApplicableMenus> ApplicableMenus = new List<ApplicableMenus>();
        //    if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
        //    {
        //        foreach (DataRow row in dataSet.Tables[0].Rows)
        //        {
        //            var item = new Location();
        //            item.Region = new Region();
        //            item.Code = MakeSafe.ToSafeInt32(row["Code"]);
        //            item.Name = MakeSafe.ToSafeString(row["Name"]);
        //            item.Region.Name = MakeSafe.ToSafeString(row["Region"]);
        //            LocationList.Add(item);
        //        }
        //    }
        //    model.Location = LocationList;
        //    if ((dataSet.Tables[1]?.Rows?.Count ?? 0) > 0)
        //    {
        //        foreach (DataRow row in dataSet.Tables[1].Rows)
        //        {
        //            var item = new ApplicableMenus();
        //            item.Module = MakeSafe.ToSafeString(row["Module"]);
        //            item.Menus = MakeSafe.ToSafeString(row["MenuName"]);
        //            item.SubModule = MakeSafe.ToSafeString(row["SubModule"]);
        //            ApplicableMenus.Add(item);
        //        }
        //    }
        //    model.ApplicableMenus = ApplicableMenus;
        //    if ((dataSet.Tables[2]?.Rows?.Count ?? 0) > 0)
        //    {
        //        foreach (DataRow row in dataSet.Tables[2].Rows)
        //        {
        //            var item = new Designation();
        //            item.Code = MakeSafe.ToSafeInt32(row["Code"]);
        //            item.Name = MakeSafe.ToSafeString(row["Name"]);
        //            DesignationList.Add(item);
        //        }
        //    }
        //    model.Designation = DesignationList;
        //    if ((dataSet.Tables[3]?.Rows?.Count ?? 0) > 0)
        //    {
        //        foreach (DataRow row in dataSet.Tables[3].Rows)
        //        {
        //            var item = new Region();
        //            item.Code = MakeSafe.ToSafeInt32(row["Code"]);
        //            item.Name = MakeSafe.ToSafeString(row["Name"]);
        //            RegionList.Add(item);
        //        }
        //    }
        //    model.Region = RegionList;
        //    return model;
        //}

        public async Task<List<Dropdown>> GetGroupCompanyDropDown(Login credentials)
        {
            try
            {
                IDbDataParameter[] sqlParameters =
               {
                 new SqlParameter("@UserName",credentials.UserCode),
                new SqlParameter("@CompanyName",credentials.CompanyCode)
            };
                var dataTable = await _sql.GetDataTableAsync("[FIXED].[SP_GROUP_COMPANY_COMBOFILL]", sqlParameters, CommandType.StoredProcedure);
                return PopulateDropDownDetails(dataTable);
            }

            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        private List<Dropdown> PopulateDropDownDetails(DataTable dt)
        {

            List<Dropdown> DropList = new List<Dropdown>();
            foreach (DataRow dr in dt.Rows)
            {
                DropList.Add(new Dropdown
                {
                    Code = MakeSafe.ToSafeInt32(dr["Code"]),
                    Name = MakeSafe.ToSafeString(dr["Name"])
                });
            }
            return DropList;
        }
    }
}