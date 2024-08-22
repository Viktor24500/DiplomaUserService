using Microsoft.Extensions.DependencyInjection;
using SystemUserService.BusinessLogic.Services;
using SystemUserService.BusinessLogic.Services.Interfaces;

namespace SystemUserService.BusinessLogic.Configuration
{
    public static class BussinessLogicServiceCollection
    {
        public static void AddBussinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IRoleService, RoleService>();
        }
    }
}
