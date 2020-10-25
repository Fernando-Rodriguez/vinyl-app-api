using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using VinylAppApi.Library.AuthorizationManager;
using VinylAppApi.Library.DbManager;
using VinylAppApi.Library.Managers.AuthorizationManager;
using VinylAppApi.Library.Managers.DataCoordinationManager;
using VinylAppApi.Library.Models.AuthorizationModels;
using VinylAppApi.Library.SpotifyApiManager;



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
                byte[] symmetricKey = Encoding.UTF8.GetBytes(Configuration.GetSection("ServerCredentials").ToString());

                var key =  new SymmetricSecurityKey(symmetricKey);

                opts.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = key
                };
            });


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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
