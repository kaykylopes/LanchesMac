using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LanchesMac
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaulConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                  .AddEntityFrameworkStores<AppDbContext>()
                  .AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "/Home/AcessDanied");


            services.AddTransient<ICategoriaRepository, CategoriaRepository>();
            services.AddTransient<ILancheRepository, LancheRepository>();
            services.AddTransient<IPedidoRepository, PedidoRepository>();


             
            //fornece uma instancia de HttpContextAcessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddHttpContextAccessor();

            //cria um objeto Scoped, ou seja um objeto que esta associado a requisição
            //isso significa que se duas pessoas solicitarem o objeto CarrinhoCompra ao  mesmo tempo
            //elas vão obter instâncias diferentes
            services.AddScoped(sp => CarrinhoCompra.GetCarrinho(sp));
            services.AddMvc();
            //
            services.AddMemoryCache();
            services.AddSession();

           /* services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            */


            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts();
            }

            /*app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();*/

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();




            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{Controller=Admin}/{Action=Index}/{id?}");

                routes.MapRoute(
                    name: "filtrarPorCategoria",
                    template: "Lanche/{action}/{categoria}",
                    defaults: new {Controller="Lanche", action="List"}); 

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
