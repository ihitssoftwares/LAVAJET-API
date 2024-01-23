using PPR.Lite.Infrastructure.Helpers;
using PPR.Lite.Infrastructure.ISecurity.Abstract;
using PPR.Lite.Repository.IRepository;
using PPR.Lite.Repository.IRepository.Account;
using PPR.Lite.Shared.Account;
using PPR.Lite.Shared.DataBank.DTO;
using PPR.Lite.Shared.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.Repository.Account
{
    public class PasswordSettingsRepository : IPasswordSettingsRepository
    {
        private readonly ISqlConnectionProvider _sql;
        private readonly IHash _hash;
        public PasswordSettingsRepository(ISqlConnectionProvider sql, IHash hash)
        {
            _sql = sql;
            _hash = hash;
        }
        public async Task<PasswordPolicySettings> GetPasswordPolicySettings(UserBase userBase)
        {
            try
            {
                IDbDataParameter[] sqlparameters =
           {
                 new SqlParameter("@CompanyGI",userBase.CompanyGuid),
                new SqlParameter("@UserGuid",userBase.UserGuid),

            };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_PASSWORD_SETTINGS_DETAILS]", sqlparameters);


                return PopulatePasswordPolicySettings(ds);
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        public async Task<PasswordPolicySettings> GetForgetPasswordPasswordPolicySettings(String CompanyID)
        {
            try
            {
                IDbDataParameter[] sqlparameters =
           {
                 new SqlParameter("@CompanyID",CompanyID),
            };
                var ds = await _sql.GetDataSetAsync("[HRMS].[SP_FORGET_PASSWORD_PASSWORD_SETTINGS_DETAILS]", sqlparameters);


                return PopulatePasswordPolicySettings(ds);
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        private PasswordPolicySettings PopulatePasswordPolicySettings(DataSet dataSet)
        {
            PasswordPolicySettings Model = new PasswordPolicySettings();



            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                Model.MinimumLength = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["MinimumLength"]);
                Model.MaximumLength = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["MaximumLength"]);
                Model.MinimumUppercaseLetters = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["MinimumUppercaseLetters"]);
                Model.MinimumLowercaseLetters = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["MinimumLowercaseLetters"]);
                Model.MinimumSpecialCharacter = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["MinimumSpecialCharacter"]);
                Model.MinimumNumericValues = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["MinimumNumericValues"]);
                Model.PasswordExpiryDays = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["PasswordExpiryDays"]);
                Model.EnforcePasswordHistory = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["EnforcePasswordHistory"]);
            }
            return Model;
        }
        public async Task<ResponseEntity> PasswordPolicySettingsSave(PasswordPolicySettings model)
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
                await _sql.ExecuteNonQueryAsync("[HRMS].[SP_PASSWORD_SETTINGS_SAVE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        public async Task<ResponseEntity> ForgetPasswordGenerateOTP(string CompanyID, string EmployeeID, DateTime? DOB)
        {
            try
            {

                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 100, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };
                IDbDataParameter[] sqlParameters =
                {

                    new SqlParameter("@CompanyID",CompanyID),
                    new SqlParameter("@EmployeeID",EmployeeID),
                     new SqlParameter("@DOB",DOB),
            ResponseKey,ResponseStatus
            };
                await _sql.ExecuteNonQueryAsync("[HRMS].[SP_FORGET_PASSWORD_GENERATE_OTP]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        public async Task<ResponseEntity> ForgetPasswordVerifyOTP(string CompanyID, string EmployeeID, string OTP)
        {
            try
            {

                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };
                IDbDataParameter[] sqlParameters =
                {

                    new SqlParameter("@CompanyID",CompanyID),
                    new SqlParameter("@EmployeeID",EmployeeID),
                     new SqlParameter("@OTP",OTP),
            ResponseKey,ResponseStatus
            };
                await _sql.ExecuteNonQueryAsync("[HRMS].[SP_FORGET_PASSWORD_VERIFY_OTP]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        public async Task<ResponseEntity> ChangePasswordSave(ChangePassword model)
        {
            try
            {
                string hash_N; string salt_N;
                _hash.CreatePasswordHash(model.NewPassword, out hash_N, out salt_N);
                model.PasswordHash_NewPassword = hash_N;
                model.PasswordSalt_NewPassword = salt_N;

                if (_hash.ComparePasswordHash(model.ConfirmPassword, model.PasswordHash_NewPassword, model.PasswordSalt_NewPassword))
                {
                    model.PasswordHash_ConfirmPassword = model.PasswordHash_NewPassword;
                    model.PasswordSalt_ConfirmPassword = model.PasswordSalt_NewPassword;
                }

                IDbDataParameter[] sqlparameters =
          {
                 new SqlParameter("@CompanyGI",model.User.CompanyGuid),
                  new SqlParameter("@UserGI",model.User.UserGuid)
            };
                var ds = await _sql.GetDataSetAsync("[HRMS].[SP_PASSWORD_HASHSALT_GET]", sqlparameters);

                ChangePassword model2 = new ChangePassword();
                model2 = PopulateHashSalt(ds);
                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };
                ResponseEntity response = new ResponseEntity("", false);
                if (_hash.ComparePasswordHash(model.EnterCurrentPassword, model2.PasswordHash_CurrentPassword, model2.PasswordSalt_CurrentPassword))
                {

                    IDbDataParameter[] sqlParameters =
                    {
            new SqlParameter("@JSONDATA",JsonHelper.ToJson(model)),
            ResponseKey,ResponseStatus
            };
                    await _sql.ExecuteNonQueryAsync("[HRMS].[SP_CHANGE_PASSWORD_SAVE]", sqlParameters, CommandType.StoredProcedure);
                    response.Message = ResponseKey.Value.ToString();
                    response.Success = Convert.ToBoolean(ResponseStatus.Value);
                }
                else
                {
                    response.Message = "Invalid Current Password";
                    response.Success = false;
                }
                return response;
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        private ChangePassword PopulateHashSalt(DataSet dataSet)
        {
            ChangePassword Model = new ChangePassword();



            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                Model.PasswordHash_CurrentPassword = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Hash"]);
                Model.PasswordSalt_CurrentPassword = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Salt"]);

            }
            return Model;
        }
        public async Task<ResponseEntity> ForgetPasswordSave(ForgetPassword model)
        {
            try
            {
                string hash_N; string salt_N;
                _hash.CreatePasswordHash(model.NewPassword, out hash_N, out salt_N);
                model.PasswordHash_NewPassword = hash_N;
                model.PasswordSalt_NewPassword = salt_N;

                if (_hash.ComparePasswordHash(model.ConfirmPassword, model.PasswordHash_NewPassword, model.PasswordSalt_NewPassword))
                {
                    model.PasswordHash_ConfirmPassword = model.PasswordHash_NewPassword;
                    model.PasswordSalt_ConfirmPassword = model.PasswordSalt_NewPassword;
                }
                //string hash_C; string salt_C;
                //_hash.CreatePasswordHash(model.ConfirmPassword, out hash_C, out salt_C);
                //model.PasswordHash_ConfirmPassword = hash_C;
                //model.PasswordSalt_ConfirmPassword = salt_C;


                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };
                IDbDataParameter[] sqlParameters =
                {
            new SqlParameter("@JSONDATA",JsonHelper.ToJson(model)),
            ResponseKey,ResponseStatus
            };
                await _sql.ExecuteNonQueryAsync("[HRMS].[SP_FORGET_CHANGE_PASSWORD_SAVE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        #region GenerateDOB_PasswordForEmployees_Indata Porting
        public async Task<ResponseEntity> Schedule_PasswordGeneneration(int EmployeeCode, string DOB)
        {
            try 
            { 
            //genrate hash and salt
            SchedulePasswordGenertion model = new SchedulePasswordGenertion();
            model.EmployeeCode = EmployeeCode;
            string hash_N; string salt_N;
            _hash.CreatePasswordHash(DOB ,out hash_N, out salt_N);
             model.HashPassword = hash_N;
             model.SaltPassword = salt_N;


                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };
             
                IDbDataParameter[] sqlparameters =
            {
                 new SqlParameter("@JSONDATA",JsonHelper.ToJson(model)),
                 ResponseStatus,ResponseKey


            };    

                await _sql.ExecuteNonQueryAsync("[HRMS].[SP_SCHEDULE_PASSWORD_GENERATION_SAVE]", sqlparameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }

            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
    }
}



            #endregion
        }
}
