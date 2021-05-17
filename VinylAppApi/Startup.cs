using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using VinylAppApi.Domain.Services.AuthorizationService;
using VinylAppApi.Domain.Services.AlbumService.DataCoordinationManager;
using VinylAppApi.Shared.Models.AuthorizationModels;
using VinylAppApi.Domain.Services.MusicInformationService;
using VinylAppApi.Helpers;
using VinylAppApi.Domain.Repository;
using VinylAppApi.Domain.Services.UserService;
using VinylAppApi.Domain.Services.AlbumService;
using VinylAppApi.Domain.Services.GroupService;

namespace VinylAppApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("OAuth").AddJwtBearer("OAuth", opts =>
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
            });

            services.AddSwaggerGen();
            services.AddControllers();
            services.AddScoped<ISpotifyRequest, SpotifyRequest>();
            services.AddScoped<IMatchUpData, MatchUpData>();
            services.AddScoped<IAuthContainerModel, JwtContainerModel>();
            services.AddScoped<IAuthService, JwtService>();
            services.AddScoped<IAuthorizationVerification, AuthorizationVerification>();
            services.AddScoped<IUserTokenHelper, UserTokenHelper>();
            services.AddScoped(typeof(IMongoRepo<>), typeof(MongoRepo<>));
            services.AddSingleton<ITokenManager, TokenManager>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IGroupService, GroupService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors("AllowAnyOrigin");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
