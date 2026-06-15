using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Recycle.Data;
using Recycle.Data.Models;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Recycle
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            
            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
           
            builder.Services.AddDbContext<Data.Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddIdentity<Data.Models.User, IdentityRole>()
                .AddEntityFrameworkStores<Data.Context>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["jwt:Issuer"],
        ValidAudience = builder.Configuration["jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"])),

         RoleClaimType = ClaimTypes.Role
    };
});


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });
            builder.Services.AddScoped<Interfaces.IOtp, Repos.OtpRepo>();
            builder.Services.AddScoped<Services.OtpServices>();
                builder.Services.AddScoped<Interfaces.ITransaction, Repos.TransactionRepo>();
            builder.Services.AddScoped<Services.TransactionServices>();
            builder.Services.AddScoped<Context>();
            builder.Services.AddScoped<Interfaces.IMachinecs, Repos.MachineRepo>();
            builder.Services.AddScoped<Services.MachineServices>();
                builder.Services.AddScoped<Services.UserServices>();
            
            builder.Services.AddScoped<Helper.ViewMachine>();

            builder.Services.AddAuthorization();

            var app = builder.Build();

            

           


           
            
            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
               
                

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

          
            app.MapControllers();

            app.Run();
        }
    }
}
