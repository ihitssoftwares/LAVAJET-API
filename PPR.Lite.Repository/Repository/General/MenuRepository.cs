using PPR.Lite.Infrastructure.Helpers;
using PPR.Lite.Repository.IRepository;
using PPR.Lite.Repository.IRepository.General;
using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.Repository.General
{
    public class MenuRepository:IMenuRepository
    {
        private readonly ISqlConnectionProvider _sql;

        public MenuRepository(ISqlConnectionProvider sql)
        {
            _sql = sql;
        }
        #region Public Properties
        public async Task<MenuEntity> GetMenu(UserBase user, string LoginFrom)
        {
            IDbDataParameter[] sqlParameters =
            {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),
                 new SqlParameter("@LoginFrom",LoginFrom)
            };
            var ds = await _sql.GetDataSetAsync("[HRMS].[SP_MENU_LIST]", sqlParameters);
            return PopulateMenuList(ds);
        }

        public async Task<MenuEntity> GetModuleList(UserBase user, string LoginFrom)
        {
            IDbDataParameter[] sqlParameters =
            {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),
                 new SqlParameter("@LoginFrom",LoginFrom)
            };
            var ds = await _sql.GetDataSetAsync("[HRMS].[SP_MODULE_LIST]", sqlParameters);
            return PopulateModuleList(ds);
        }
        public async Task<MenuEntity> GetModuleVsMenuList(UserBase user, int ModuleCode)
        {
            IDbDataParameter[] sqlParameters =
            {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),
                 new SqlParameter("@ModuleCode",ModuleCode)
            };
            var ds = await _sql.GetDataSetAsync("[HRMS].[SP_MODULE_VS_MENU_LIST]", sqlParameters);
            return PopulateModuleVsMenuList(ds);
        }
        #endregion

        #region Private Properties

        private MenuEntity PopulateMenuList(DataSet ds)
        {
            var details = new MenuEntity();

            List<ModuleItem> module = new List<ModuleItem>();
            if ((ds.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ModuleItem item = new ModuleItem();

                    item.ModuleCode = MakeSafe.ToSafeInt32(row["ModuleCode"]);
                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.Active = MakeSafe.ToSafeInt32(row["Active"]);
                    item.DisplayOrder = MakeSafe.ToSafeInt32(row["DisplayOrder"]);
                    item.Class = MakeSafe.ToSafeString(row["Class"]);
                    item.ModuleKey = MakeSafe.ToSafeString(row["ModuleKey"]);
                    item.Route = MakeSafe.ToSafeString(row["Route"]);
                    item.CompanyCode = MakeSafe.ToSafeInt32(row["CompanyCode"]);

                    module.Add(item);
                }
                details.Module = module;
            }
            List<SubModuleItem> submodule = new List<SubModuleItem>();
            if ((ds.Tables[1]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    SubModuleItem item = new SubModuleItem();
                    item.SubModuleCode = MakeSafe.ToSafeInt32(row["SubModuleCode"]);
                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.ModuleCode = MakeSafe.ToSafeInt32(row["ModuleCode"]);
                    item.Active = MakeSafe.ToSafeInt32(row["Active"]);
                    item.DisplayOrder = MakeSafe.ToSafeInt32(row["DisplayOrder"]);
                    item.Class = MakeSafe.ToSafeString(row["Class"]);
                    item.SubModuleKey = MakeSafe.ToSafeString(row["SubModuleKey"]);
                    item.CompanyCode = MakeSafe.ToSafeInt32(row["CompanyCode"]);
                    submodule.Add(item);
                }
                details.SubModule = submodule;
            }
            List<MenuItem> menus = new List<MenuItem>();
            if ((ds.Tables[2]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in ds.Tables[2].Rows)
                {
                    MenuItem item = new MenuItem();

                    item.MenuCode = MakeSafe.ToSafeInt32(row["MenuCode"]);
                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.SubModuleCode = MakeSafe.ToSafeInt32(row["SubModuleCode"]);

                    item.Area = MakeSafe.ToSafeString(row["Area"]);
                    item.Controller = MakeSafe.ToSafeString(row["Controller"]);
                    item.Class = MakeSafe.ToSafeString(row["Class"]);
                    item.Action = MakeSafe.ToSafeString(row["Action"]);
                    item.Active = MakeSafe.ToSafeInt32(row["Active"]);
                    item.FormType = MakeSafe.ToSafeString(row["FormType"]);

                    item.MenuKey = MakeSafe.ToSafeString(row["MenuKey"]);
                    item.Route = MakeSafe.ToSafeString(row["Route"]);
                    item.IsHrmsMenu = MakeSafe.ToSafeInt32(row["IsPPRMenu"]);
                    item.IsEsspMenu = MakeSafe.ToSafeInt32(row["IsEsspMenu"]);
                    item.IsStaticMenu = MakeSafe.ToSafeInt32(row["IsStaticMenu"]);
                    item.CompanyCode = MakeSafe.ToSafeInt32(row["CompanyCode"]);

                    menus.Add(item);
                }
                details.Menu = menus;
            }
            return details;

        }
        private MenuEntity PopulateModuleList(DataSet ds)
        {
            var details = new MenuEntity();

            List<ModuleItem> module = new List<ModuleItem>();
            if ((ds.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ModuleItem item = new ModuleItem();

                    item.ModuleCode = MakeSafe.ToSafeInt32(row["ModuleCode"]);
                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.Active = MakeSafe.ToSafeInt32(row["Active"]);
                    item.DisplayOrder = MakeSafe.ToSafeInt32(row["DisplayOrder"]);
                    item.Class = MakeSafe.ToSafeString(row["Class"]);
                    item.ModuleKey = MakeSafe.ToSafeString(row["ModuleKey"]);
                    item.Route = MakeSafe.ToSafeString(row["Route"]);
                    item.CompanyCode = MakeSafe.ToSafeInt32(row["CompanyCode"]);

                    module.Add(item);
                }
                details.Module = module;
            }
            return details;
        }

        private MenuEntity PopulateModuleVsMenuList(DataSet ds)
        {
            var details = new MenuEntity();

            List<SubModuleItem> submodule = new List<SubModuleItem>();
            if ((ds.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SubModuleItem item = new SubModuleItem();
                    item.SubModuleCode = MakeSafe.ToSafeInt32(row["SubModuleCode"]);
                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.ModuleCode = MakeSafe.ToSafeInt32(row["ModuleCode"]);
                    item.Active = MakeSafe.ToSafeInt32(row["Active"]);
                    item.DisplayOrder = MakeSafe.ToSafeInt32(row["DisplayOrder"]);
                    item.Class = MakeSafe.ToSafeString(row["Class"]);
                    item.SubModuleKey = MakeSafe.ToSafeString(row["SubModuleKey"]);
                    item.CompanyCode = MakeSafe.ToSafeInt32(row["CompanyCode"]);
                    item.ModuleName = MakeSafe.ToSafeString(row["ModuleName"]);
                    item.ModuleClass = MakeSafe.ToSafeString(row["ModuleClass"]);
                    submodule.Add(item);
                }
                details.SubModule = submodule;
            }
            List<MenuItem> menus = new List<MenuItem>();
            if ((ds.Tables[1]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    MenuItem item = new MenuItem();

                    item.MenuCode = MakeSafe.ToSafeInt32(row["MenuCode"]);
                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.SubModuleCode = MakeSafe.ToSafeInt32(row["SubModuleCode"]);

                    item.Area = MakeSafe.ToSafeString(row["Area"]);
                    item.Controller = MakeSafe.ToSafeString(row["Controller"]);
                    item.Class = MakeSafe.ToSafeString(row["Class"]);
                    item.Action = MakeSafe.ToSafeString(row["Action"]);
                    item.Active = MakeSafe.ToSafeInt32(row["Active"]);
                    item.FormType = MakeSafe.ToSafeString(row["FormType"]);

                    item.MenuKey = MakeSafe.ToSafeString(row["MenuKey"]);
                    item.Route = MakeSafe.ToSafeString(row["Route"]);
                    item.IsHrmsMenu = MakeSafe.ToSafeInt32(row["IsHrmsMenu"]);
                    item.IsEsspMenu = MakeSafe.ToSafeInt32(row["IsEsspMenu"]);
                    item.IsStaticMenu = MakeSafe.ToSafeInt32(row["IsStaticMenu"]);
                    item.CompanyCode = MakeSafe.ToSafeInt32(row["CompanyCode"]);

                    menus.Add(item);
                }
                details.Menu = menus;
            }
            return details;

        }
        #endregion

        #region Menu Search
        public async Task<MenuItem> GetMenuDetails(UserBase user, int MenuCode)
        {
            IDbDataParameter[] sqlParameters =
            {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),
                new SqlParameter("@MenuCode",MenuCode)
            };
            var ds = await _sql.GetDataSetAsync("[HRMS].[SP_MENU_FILTER]", sqlParameters);
            return PopulateMenuDetails(ds);
        }
        private MenuItem PopulateMenuDetails(DataSet ds)
        {
            var model = new MenuItem();
            if ((ds.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                model.MenuCode = MakeSafe.ToSafeInt32(ds.Tables[0].Rows[0]["MenuCode"]);
                model.Controller = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["Controller"]);
                model.Action = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["Action"]);
                model.Area = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["Area"]);
                model.Name = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["Name"]);
                model.ModuleCode = MakeSafe.ToSafeInt32(ds.Tables[0].Rows[0]["ModuleCode"]);
                model.SubModuleCode = MakeSafe.ToSafeInt32(ds.Tables[0].Rows[0]["SubModuleCode"]);
            }
            return model;
        }
        #endregion
    }
}
