using System;
using DeliveryApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DeliveryApp.Services;
using DeliveryApp.Util;
using DeliveryApp.Repository;
using DeliveryApp.Service;
using Newtonsoft.Json;
using Polly;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Reflection;
using System.IO;
using DeliveryApp.Controller;

namespace DeliveryApp
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
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DatabaseContext>(opt => opt.UseNpgsql(connectionString));

            // services
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<ISecurityUtil, SecurityUtil>();

            services
                .AddScoped<IAdditionalFeeRules, ClearSkyFee>()
                .AddScoped<IAdditionalFeeRules, FewCloudsFee>()
                .AddScoped<IAdditionalFeeRules, ScatteredCloudsSkyFee>()
                .AddScoped<IAdditionalFeeRules, BrokenCloudsSkyFee>()
                .AddScoped<IAdditionalFeeRules, ShowerRainFee>()
                .AddScoped<IAdditionalFeeRules, RainFee>()
                .AddScoped<IAdditionalFeeRules, ThunderstormFee>()
                .AddScoped<IAdditionalFeeRules, SnowFee>()
                .AddScoped<IAdditionalFeeRules, MistFee>();

            // http services
            services.AddHttpClient<IOpenRouteService, OpenRouteService>(c => {
                 c.BaseAddress = new Uri("https://api.openrouteservice.org/v2/directions/driving-car");
            }).AddTransientHttpErrorPolicy(p => 
                p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(500)));

            services.AddHttpClient<IOpenWatherMapService, OpenWatherMapService>(c => {
                c.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/weather");
            }).AddTransientHttpErrorPolicy(p => 
                p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(500)));

            services.AddHttpClient<IOpenCageDataService, OpenCageDataService>(c => {
                c.BaseAddress = new Uri("https://api.opencagedata.com/");
            }).AddTransientHttpErrorPolicy(p => 
                p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(500)));

            // repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDeliveryRepository, DeliveryRepository>();


            services.AddHttpContextAccessor();

            services.AddCors();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter()))
                .AddNewtonsoftJson(options => {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    })
                .AddJsonOptions(options => 
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            var key = Encoding.ASCII.GetBytes(Constants.JWT_SECRET);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerGen(c => {

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Delivery App",
                    Description = "An API to make deliveries",
                    Contact = new OpenApiContact
                    {
                        Name = "Emanuel Cerqueira",
                        Email = "emanuelcerqueira07@gmail.com",
                        Url = new Uri("https://github.com/emanuelcerqueira"),
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header, 
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey 
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { 
                    new OpenApiSecurityScheme
                    { 
                    Reference = new OpenApiReference
                    { 
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer" 
                    } 
                    },
                    new string[] { } 
                    } 
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }
    }
}
