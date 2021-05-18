using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using VinylAppApi.Domain.Services.AuthorizationService;
using VinylAppApi.Domain.Services.AlbumService.DataCoordinationManager;
using VinylAppApi.Domain.Models.AuthorizationModels;
using VinylAppApi.Domain.Services.MusicInformationService;
using VinylAppApi.Helpers;
using VinylAppApi.Domain.Repository;
using VinylAppApi.Domain.Services.UserService;
using VinylAppApi.Domain.Services.AlbumService;
using VinylAppApi.Domain.Services.GroupService;
using VinylAppApi.Domain.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VinylAppApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services
                .AddAuthentication("OAuth")
                .AddCookie(config =>
                {
                    config.Cookie.HttpOnly = true;
                    config.Cookie.Name = "vinyl_app";
                })
                .AddJwtBearer("OAuth", opts =>
                {
                    byte[] symmetricKey = Encoding.UTF8
                    .GetBytes(Configuration.GetSection("ServerCredentials").ToString());

                    var key =  new SymmetricSecurityKey(symmetricKey);

                    opts.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        IssuerSigningKey = key
                    };

                    opts.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["_bearer"];
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddSwaggerGen();
            services.AddControllers();
            services.AddScoped<ISpotifyRequest, SpotifyRequest>();
            services.AddScoped<IMatchUpData, MatchUpData>();
            services.AddScoped<IAuthContainerModel, JwtContainerModel>();
            services.AddScoped<IAuthService, JwtService>();
            services.AddScoped<IUserTokenHelper, UserTokenHelper>();
            services.AddScoped(typeof(IMongoRepo<>), typeof(MongoRepo<>));
            services.AddSingleton<ITokenManager, TokenManager>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var cookiePolicy = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
                Secure = CookieSecurePolicy.Always,
                CheckConsentNeeded = context => true,  
            };

            app.UseCookiePolicy(cookiePolicy);

            app.UseSwagger();

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                config.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            // Managed by cloud service.
            // app.UseCors("AllowAnyOrigin");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
