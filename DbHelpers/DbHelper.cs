using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Teatro.Data;
using Teatro.Exceptions;
using Teatro.Models.Entities;

namespace Teatro.Helpers
{
    public static class DbHelper
    {
        public static async Task InitializeDatabaseAsync<T>(WebApplication app) where T : DbContext
        {
            try
            {
                IServiceProvider services = app.Services;

                await RunMigrationAsync<T>(services);
                await SeedRoles(services);
                await SeedAdmin(services);
            }
            catch
            {
                throw;
            }
        }

        private static async Task RunMigrationAsync<T>(IServiceProvider services) where T : DbContext
        {
            try
            {
                using var scope = services.CreateAsyncScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<T>();

                await dbContext.Database.MigrateAsync();
            }
            catch
            {
                throw;
            }
        }

        private static async Task SeedRoles(IServiceProvider services)
        {
            try
            {
                using var scope = services.CreateAsyncScope();

                RoleManager<ApplicationRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

                bool superAdminRoleExists = await roleManager.RoleExistsAsync(StringConstants.AdminRole);

                ApplicationRole? superAdminRole = null;
                ApplicationRole? userRole = null;

                if (!superAdminRoleExists)
                {
                    superAdminRole = new ApplicationRole()
                    {
                        Name = StringConstants.AdminRole,
                        Description = "Amministratore del sistema",
                        Active = true
                    };

                    IdentityResult superAdminRoleCreated = await roleManager.CreateAsync(superAdminRole);

                    if (!superAdminRoleCreated.Succeeded)
                    {
                        throw new DbInitializationException("Errore durante la creazione del ruolo Amministratore");
                    }
                }

                bool userRoleExists = await roleManager.RoleExistsAsync(StringConstants.UserRole);

                if (!userRoleExists)
                {
                    userRole = new ApplicationRole()
                    {
                        Name = StringConstants.UserRole,
                        Description = "Utente standard",
                        Active = true
                    };

                    IdentityResult userRoleCreated = await roleManager.CreateAsync(userRole);

                    if (!userRoleCreated.Succeeded)
                    {
                        if (superAdminRole != null)
                        {
                            await roleManager.DeleteAsync(superAdminRole);
                        }
                        throw new DbInitializationException("Errore durante la creazione del ruolo Utente");
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private static async Task SeedAdmin(IServiceProvider services)
        {
            try
            {
                using var scope = services.CreateAsyncScope();

                UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                ApplicationUser? existingSuperAdmin = await userManager.FindByEmailAsync(StringConstants.AdminEmail);

                if (existingSuperAdmin == null)
                {
                    ApplicationUser superAdmin = new ApplicationUser()
                    {
                        Active = true,
                        DataDiNascita = new DateOnly(1990, 1, 1),
                        Nome = "Admin",
                        Cognome = "Sistema",
                        Email = StringConstants.AdminEmail,
                        UserName = StringConstants.AdminEmail,
                        DataCreazione = DateTime.UtcNow,
                        EmailConfirmed = true
                    };

                    IdentityResult userCreated = await userManager.CreateAsync(superAdmin, StringConstants.AdminPassword);

                    if (!userCreated.Succeeded)
                    {
                        throw new DbInitializationException("Errore durante la creazione dell'utente Admin");
                    }

                    IdentityResult roleAssigned = await userManager.AddToRoleAsync(superAdmin, StringConstants.AdminRole);

                    if (!roleAssigned.Succeeded)
                    {
                        await userManager.DeleteAsync(superAdmin);
                        throw new DbInitializationException("Errore durante l'assegnamento del ruolo all'utente");
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}