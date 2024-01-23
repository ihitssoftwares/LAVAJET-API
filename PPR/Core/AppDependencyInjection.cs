using Microsoft.Extensions.DependencyInjection;
using ppr.Lite.Repository.IRepository.Master;
using PPR.Lite.Infrastructure.ISecurity.Abstract;
using PPR.Lite.Infrastructure.Security;
using PPR.Lite.Repository.IRepository;
using PPR.Lite.Repository.IRepository.Account;
using PPR.Lite.Repository.IRepository.General;
using PPR.Lite.Repository.IRepository.Master;
using PPR.Lite.Repository.IRepository.Report;
using PPR.Lite.Repository.Repository;
using PPR.Lite.Repository.Repository.Account;
using PPR.Lite.Repository.Repository.General;
using PPR.Lite.Repository.Repository.Master;
using PPR.Lite.Repository.Repository.Report;

namespace PPR.Lite.Api.Core
{
    public static class AppDependencyInjection
    {
        public static void AddDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddTransient<ISqlConnectionProvider, SqlConnectionProvider>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddScoped<IHash, Hash>();
            services.AddTransient<IPasswordSettingsRepository, PasswordSettingsRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();
            services.AddScoped<IPassword, Password>();

            #region General 
            services.AddTransient<IDropdownRepository, DropdownRepository>();
            services.AddTransient<ICommonRepository, CommonRepository>();
            #endregion
            #region MASTER
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IGradeRepository, GradeRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<IDesignationRepository, DesignationRepository>();
            services.AddTransient<IJobRoleRepository, JobRoleRepository>();
            services.AddTransient<IReviewPeriodRepository, ReviewPeriodRepository>();
            services.AddTransient<IFactorRepository, FactorRepository>();
            services.AddTransient<IRatingPaternRepository, RatingPaternRepository>();
            services.AddTransient<IFactorGroupRepository, FactorGroupRepository>();
            services.AddTransient<IJobRoleVsFactorsRepository, JobRoleVsFactorsRepository>();
 
            services.AddTransient<IAppraisalEmployeeAuthoritySettingsRepository, AppraisalEmployeeAuthoritySettingsRepository>();
            services.AddTransient<IPerformanceScheduleRepository, PerformanceScheduleRepository>();
            services.AddTransient<IMenuRepository, MenuRepository>();
            services.AddTransient<IPPRSelfRepository, PPRSelfRepository>();

            #endregion
            #region LavaJetReport
            services.AddTransient<ILavaJetReportRepository, LavaJetReportRepository>();
            #endregion
        }
    }
}
