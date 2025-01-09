
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyDoctorApp.Configuration;
using MyDoctorApp.Data;
using MyDoctorApp.Helpers;
using MyDoctorApp.Repositories;
using MyDoctorApp.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using System.Reflection;
using System.Text;

namespace MyDoctorApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<MyDoctorAppDbContext>(options => options.UseSqlServer(connString));

            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });

            builder.Services.AddScoped<IApplicationService, ApplicationService>();
            builder.Services.AddRepositories();

            builder.Services.AddScoped(provider =>
                new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new MapperConfig());
                })
                .CreateMapper());

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtSettings = builder.Configuration.GetSection("Authentication");
                options.IncludeErrorDetails = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidIssuer = "https://codingfactory.aueb.gr",

                    ValidateAudience = false,
                    ValidAudience = "https://api.codingfactory.aueb.gr",

                    ValidateLifetime = true, 

                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(jwtSettings["SecretKey"]!))

                };
            });

            builder.Services.AddCors(options => {
                options.AddPolicy("AllowAll",
                    b => b.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin());
            });

            builder.Services.AddCors(options => {
                options.AddPolicy("AngularClient",
                    b => b.WithOrigins("http://localhost:4200") // Assuming Angular runs on localhost:4200
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            // Add services to the container.
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);


                options.SwaggerDoc("v1", new OpenApiInfo { Title = "MyDoctor API", Version = "v1" });
                options.IncludeXmlComments(xmlPath);
                options.SupportNonNullableReferenceTypes();
                options.OperationFilter<AuthorizeOperationFilter>();
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        BearerFormat = "JWT"
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
