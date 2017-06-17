using System;
using System.Linq;
using Library.Core.Models;
using Library.Repo;
using Library.Web.Code;
// using Library.Web.Data;
using Library.Web.Models;
using Library.Web.Models.AccountViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Web.Extensions
{
    public static class IdentityExtensions
    {
        public static async void EnsureRolesCreated(this IServiceScope serviceScope)
        {
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var context = serviceScope.ServiceProvider.GetRequiredService<LibraryDbContext>();
            if (context.AllMigrationsApplied())
            {                
                foreach (var role in Roles.All)
                {
                    if (!await roleManager.RoleExistsAsync(role.ToUpper()))
                    {
                        var result = await roleManager.CreateAsync(new Role { Name = role });
                    }
                }
            }
            
        }
        public static async void EnsureUsersCreated(this IServiceScope serviceScope) {
            var context = serviceScope.ServiceProvider.GetRequiredService<LibraryDbContext>();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
            if (context.AllMigrationsApplied())
            {
                foreach(var user in Users.All) {                    
                    if(await userManager.FindByNameAsync(user.UserName) == null){
                        var result = await userManager.CreateAsync(user, "Pa$$word@31");
                        if(user.UserName.Equals("08138238095") || user.UserName.Equals("08064028176")) continue;
                        await userManager.AddToRoleAsync(user, Roles.Member);  
                    }                    
                }
                var person = await userManager.FindByNameAsync("08138238095")  ;
                await userManager.AddToRoleAsync(person, Roles.SuperAdministrator);

                person = await userManager.FindByNameAsync("08064028176")  ;
                await userManager.AddToRoleAsync(person, Roles.Admin);

            }

            
        }

        public static User MapToUser(this CreateUserViewModel model) {
            var user = new User 
            { 
                UserName = model.PhoneNumber, 
                PhoneNumber = model.PhoneNumber, 
                PhoneNumberConfirmed= true, 
                FirstName = model.FirstName, 
                LastName = model.LastName, 
                ChangePasswordFirstTimeLogin = model.ChangePasswordFirstTimeLogin, 
                Email = model.Email 
            };
            return user;
        }
        public static IQueryable<UserViewModel> MapToUserViewModel(this IQueryable<User> user) {
            var model = user.Select( u => new UserViewModel {
                UserId = u.Id,
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,                
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
                InsertedAt = u.InsertedAt
            });            
            return model;
        }
        public static IQueryable<RoleViewModel> MapToRoleViewModel(this IQueryable<Role> roles) {
            var roleViewModel = roles.Select(r => new RoleViewModel {
                Id = r.Id,
                Name = r.Name
            });
            return roleViewModel;
        }
        
    }
}