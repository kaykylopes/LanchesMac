using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanchesMac.Data
{
    public static class SeeData
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider,
                                            IConfiguration Configuration)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            //define os perfis em um array

            string[] roleNames = { "Admin", "Menber" };
            IdentityResult roleResul;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResul = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var poweruser = new IdentityUser
            {
                //obtem o nome e o email do arquivo de configuração
                UserName = Configuration.GetSection("UserSettings")["UserName"],
                Email = Configuration.GetSection("UserSettings")["UserEmail"]
            };
            //obtem a senha do arquivo de configuração
            string userPassword = Configuration.GetSection("UserSettings")["UserPassword"];

            //verifica se existe um usuário com o email informado
            var user = await UserManager.FindByEmailAsync(Configuration.GetSection("UserSettings")["UserEmail"]);

            if (user == null)
            {
                //cria o super usuário com os dados informados
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    // atribui o usuário ao perfil Admin
                    await UserManager.AddToRoleAsync(poweruser, "Admin");
                }
            }
        }
    }
}
