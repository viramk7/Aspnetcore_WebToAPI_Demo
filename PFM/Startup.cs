using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PFM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Polly;
using System.Net.Http;
using System.Net;
using PFM.Models.InputDtos;
using PFM.Models.OutputDtos;
using System.Security.Claims;
using PFM.Models.Dtos;

namespace PFM
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
            services.AddHttpClient<HttpClient>();
            services.AddHttpClient<HttpClientHelper>()
                .AddPolicyHandler((provider, request) =>
                {
                    return
                        Policy
                            .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.Unauthorized)
                            .RetryAsync(1, async (response, retryCount, context) =>
                            {
                                var client = provider.GetRequiredService<HttpClientHelper>();
                                var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();

                                var uri = "api/auth/refresh-token";
                                var refreshData = new RefreshTokenInputDto
                                {
                                    Email = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value,
                                    RefreshToken = httpContextAccessor.HttpContext.Session.GetString("refresh_token")
                                };
                                var newTokens = 
                                await client.Post<RefreshTokenInputDto, LoginDto>(uri, refreshData);

                                httpContextAccessor.HttpContext.Session.SetString("access_token", newTokens.AccessToken.Token);
                                httpContextAccessor.HttpContext.Session.SetString("refresh_token", newTokens.RefreshToken);

                                // refresh auth token.
                            });
                });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromDays(10);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddTransient<IHttpClientHelper, HttpClientHelper>();
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IAuthService, AuthService>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o => o.LoginPath = "/Authentication/Index");

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
