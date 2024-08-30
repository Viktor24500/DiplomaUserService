using Microsoft.Extensions.DependencyInjection;
using SystemUserService.DataAccess.Repositories;
using SystemUserService.DataAccess.Repositories.Intefaces;

namespace SystemUserService.DataAccess.Configuration
{
    public static class DataAccessServicesCollection
    {
        public static void AddDataAccessServices(this IServiceCollection services)
        {
            services.AddScoped<IPermissionsRepository, PermissionsRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IRolePermissionsRepository, RolePermissionsRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IUserRolePermissionsRepository, IUserRolePermissionsRepository>();
        }
    }
}
