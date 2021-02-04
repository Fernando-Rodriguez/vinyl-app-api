using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using VinylAppApi.Authorization.AuthorizationManager;
using VinylAppApi.DataAccess.DbManager;
using VinylAppApi.DataAccess.DataCoordinationManager;
using VinylAppApi.Shared.Models.AuthorizationModels;
using VinylAppApi.SpotifyHandler.SpotifyApiManager;

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
            services.AddTransient<IDbAccess, DbAccess>();
            services.AddTransient<ISpotifyRequest, SpotifyRequest>();
            services.AddTransient<IMatchUpData, MatchUpData>();
            services.AddSingleton<ITokenManager, TokenManager>();
            services.AddTransient<IAuthContainerModel, JwtContainerModel>();
            services.AddTransient<IAuthService, JwtService>();
            services.AddTransient<IAuthorizationVerification, AuthorizationVerification>();
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
