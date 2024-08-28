using Microsoft.Extensions.DependencyInjection;
using SystemUserService.BusinessLogic.Services;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Validators;

namespace SystemUserService.BusinessLogic.Configuration
{
    public static class BussinessLogicServiceCollection
    {
        public static void AddBussinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<PasswordChecks>();
        }
    }
}
