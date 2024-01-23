using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.IRepository.General
{
    public interface IMenuRepository
    {
        public Task<MenuEntity> GetMenu(UserBase user, string LoginFrom);
        public Task<MenuEntity> GetModuleList(UserBase user, string LoginFrom);
        public Task<MenuEntity> GetModuleVsMenuList(UserBase user, int ModuleCode);
        public Task<MenuItem> GetMenuDetails(UserBase user, int MenuCode);
    }
}
