using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharpsenStreamBackend.Database;
using SharpsenStreamBackend.Jwt;
using SharpsenStreamBackend.Resources;
using SharpsenStreamBackend.Resources.Interfaces;
using SharpsenStreamBackend.StreamChat;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SharpsenStreamBackend
{
    public class Startup
    {
        StreamChatServer _chatServer;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string corsName = "WebApiCors";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SharpsenStreamBackend", Version = "v1" });
            });
            services.AddSwaggerDocument();
            services.AddCors(options =>
            {
                options.AddPolicy(corsName,
                        builder => {
                            builder.WithOrigins("http://localhost:4200", "https://localhost:4200");
                            builder.AllowCredentials();
                        });
            });
            services.AddSingleton<DbController>();
            services.AddSingleton<IStreamResource, StreamResource>();
            services.AddSingleton<IUserResource, UserResource>();
            services.AddSingleton<IJwtAuthManager, JwtAuthManager>();

            services.AddSingleton<ChatRooms>();
            services.AddSingleton<StreamChatServer>();

            var config = new Config();

            services.AddSingleton(config);
            /*services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.Cookie.HttpOnly = false;
                  options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                  options.Cookie.SameSite = SameSiteMode.None;
                  options.Cookie.Name = "SharpsenStreamCookie";
              });*/
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = config.config.jwt.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.config.jwt.Secret)),
                    ValidAudience = config.config.jwt.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _chatServer = app.ApplicationServices.GetService<StreamChatServer>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SharpsenStreamBackend v1"));
            }

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.ContentRootPath, "ServerFiles")),
                RequestPath = "/ServerFiles",
                EnableDirectoryBrowsing = true
            });

            app.UseWebSockets();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(corsName);

            app.UseCookiePolicy();


            app.UseAuthentication();

            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        var socketFinishedTcs = new TaskCompletionSource<object>();
                        _chatServer.handleUser(webSocket, socketFinishedTcs);
                        await socketFinishedTcs.Task;
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }

            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
