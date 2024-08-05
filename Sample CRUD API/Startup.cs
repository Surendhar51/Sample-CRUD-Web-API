using DATA.DbModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using REPOSITORY.IRepository;
using REPOSITORY.Repository;
using System.Text;
using UTILITY;
using static System.Net.WebRequestMethods;

namespace Sample_CRUD_API
{
    public class Startup
    {
        public static WebApplication InitApp(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();
            Configure(app);
            return app;
        }
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration["DemoDb"];
            builder.Services.AddDbContext<Context>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
            builder.Services.AddTransient<SettingVariables>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("api", new OpenApiInfo()
                {
                    Description = "Sample CURD API With Swagger",
                    Title = "Sample CURD API",
                    Version = "1"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement{
         {
             new OpenApiSecurityScheme{
                 Reference=new OpenApiReference{
                     Type=ReferenceType.SecurityScheme,
                     Id="Bearer"
                 }
             },
             new string[]{ }
         }
     });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowHeaders",
                    policy =>
                    {
                        policy.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials();
                    });
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "http://testapi.com",
                    ValidAudience =" http://testapi.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("pintusharmaqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqweqwe")),
                    ClockSkew = TimeSpan.Zero
                };

            });


        }
        public static void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options => options.SwaggerEndpoint("api/swagger.json", "Sample CURD"));
            }
            app.UseCors("AllowHeaders");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
               app.MapControllers();

            app.Run();
        }
    }
}
