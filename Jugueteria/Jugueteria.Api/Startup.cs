using FluentValidation.AspNetCore;
using Jugueteria.Models.Segurity;
using Jugueteria.Models.StaticDictionary;
using Jugueteria.Persistence;
using Jugueteria.Service.Repositories;
using Jugueteria.Service.Repositories.ToysRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Text;

namespace Jugueteria.Api
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

            services.AddControllers().AddNewtonsoftJson(options =>  // evita la referencia circular
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            })

            .AddFluentValidation(s =>  // registra el validador para todas las clases que hereden de AbstractValidator
            {
                s.RegisterValidatorsFromAssemblyContaining<Users>();
                s.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Jugueteria.Api", Version = "v1" });
            });

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            var builder = services.AddIdentityCore<Users>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services).AddDefaultTokenProviders();
            identityBuilder.AddRoles<IdentityRole>();
            identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Users, IdentityRole>>();
            identityBuilder.AddEntityFrameworkStores<ApplicationDbContext>();
            identityBuilder.AddSignInManager<SignInManager<Users>>();

            //jwt
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SD.ApiKey));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,                    
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

            //Serilog
            services.AddSingleton<Serilog.ILogger>(options =>
            {
                var connString = Configuration["Serilog:DefaultConnection"];
                var tableName = Configuration["Serilog:TableName"];
                return new LoggerConfiguration()
                .WriteTo.MSSqlServer(
                    connectionString: connString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = tableName,
                        SchemaName = "dbo",
                        AutoCreateSqlTable = true
                    }).CreateLogger();
            });


            //services
            services.AddScoped<IIdentityRepo, IdentityRepo>();
            services.AddScoped<IToysRepository, ToysRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jugueteria.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
