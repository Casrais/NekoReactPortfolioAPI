using CosmosAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Cosmos;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using CosmosAPI.Services;
using Microsoft.AspNetCore.Http;
using System.Web.Http;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using CosmosAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PieroDeTomi.EntityFrameworkCore.Identity.Cosmos.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Swashbuckle.Swagger;


namespace CosmosAPI
{
//[Microsoft.AspNetCore.Authorization.AuthorizeAttribute]
    public class Startup
    {

        public static CosmosClient cosmosClient;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        private static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(string DatabaseName, string Container, string Account, string Key, string Conn)
        {

            //string databaseName = configurationSection.GetSection("DatabaseName").Value;
            string databaseName = DatabaseName;
            string containerName = Container;
            string account = Account;
            string key = Key;
            string Connection = Conn;
            //Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
            
            List<(string, string)> containersToInitialize = new List<(string, string)>
    {           (databaseName, "Category"),
                (databaseName, "Creator"),
                (databaseName, "Files"),
                (databaseName, "Identity"),
                (databaseName, "Identity_DeviceFlowCodes"),
                (databaseName, "Identity_PersistedGrant"),
                (databaseName, "Identity_Tokens"),
                (databaseName, "Identity_UserRoles"),
                (databaseName, "Identity_Logins"),
                (databaseName, "Item"),
                (databaseName, "Medium"),
                (databaseName, "Post"),
                (databaseName, "User"),
                (databaseName, "UserComment")
            };
            Microsoft.Azure.Cosmos.CosmosClient client = await CosmosClient.CreateAndInitializeAsync(Connection, containersToInitialize);
            
            CosmosDbService cosmosDbService = new CosmosDbService(client, databaseName, containerName);
            Microsoft.Azure.Cosmos.DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            return cosmosDbService;
        }

        

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddIdentityCore<Identity>().AddEntityFrameworkStores<CosmosAPIDataContext>().AddSignInManager<SignInManager<Identity>>();

            //services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                        {
                            Delay= TimeSpan.FromSeconds(2),
                            MaxDelay = TimeSpan.FromSeconds(16),
                            MaxRetries = 5
                         }
            };
            var secretclient = new SecretClient(new Uri(Environment.GetEnvironmentVariable("VaultUri")), new DefaultAzureCredential(), options);

            KeyVaultSecret secretDatabaseName = secretclient.GetSecret("DatabaseName");
            KeyVaultSecret secretCosmosKey = secretclient.GetSecret("CosmosKey");
            KeyVaultSecret secretCosmosAccount = secretclient.GetSecret("NekoCosmosAccount");
            KeyVaultSecret secretCosmosConnection = secretclient.GetSecret("NekoCosmosConnection");
            KeyVaultSecret secretAuth0Domain = secretclient.GetSecret("NekoAuthDomain");
            KeyVaultSecret secretAuth0Audience = secretclient.GetSecret("NekoAuthAudience");
            KeyVaultSecret secretTokenKey = secretclient.GetSecret("NekoTokenKey");

            string databaseName = secretDatabaseName.Value;
            string containerName = Configuration.GetSection("ContainerName").Value;
            string account = secretCosmosAccount.Value;
            string key = secretCosmosKey.Value;
            string connection = secretCosmosConnection.Value;
            string authDomain = secretAuth0Domain.Value;
            string authAudience = secretAuth0Audience.Value;
            string tokenKey = secretTokenKey.Value;


            services.AddEntityFrameworkCosmos();

            //cosmosClient = new CosmosClient(Configuration.GetSection("CosmosAccount").Value,Configuration.GetSection("CosmosKey").Value, new CosmosClientOptions() { ApplicationName = "NekoPortfolio" });
            services.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync(databaseName, containerName, account, key, connection).GetAwaiter().GetResult());
            //services.AddDbContext<CosmosAPIDataContext>();

            services.AddCosmosIdentity<CosmosAPIDataContext, Identity, IdentityRole>(
                 // Auth provider standard configuration (e.g.: account confirmation, password requirements, etc.)
                  options => { // User settings
                      options.User.RequireUniqueEmail = true;
                      options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                      // Password settings
                      options.Password.RequireDigit = true;
                      options.Password.RequiredLength = 8;
                      options.Password.RequireLowercase = true;
                      options.Password.RequireNonAlphanumeric = false;
                      options.Password.RequireUppercase = true;

                      // Lockout settings
                      options.Lockout.AllowedForNewUsers = true;
                      options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                      options.Lockout.MaxFailedAccessAttempts = 5;
                  },
                
                  // Cosmos DB configuration options
                  options => options.UseCosmos(
                      account,
                      key,
                      databaseName
                  ),
                
                  // If true, AddDefaultTokenProviders() method will be called on the IdentityBuilder instance
                  addDefaultTokenProviders: true
                );


            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"]));
            string domain = $"{authDomain}";

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddIdentityServerJwt()
                .AddJwtBearer(options =>
    {
        options.Authority = authDomain;
        options.Audience = authAudience;
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = authDomain,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            ValidAudience = authAudience,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // Override the default clock skew of 5 mins
        };
    }
    );


            //services.AddIdentity<Identity, IdentityRole>().AddEntityFrameworkStores<CosmosAPIDataContext>();
            //services.AddIdentityServer().AddApiAuthorization<IdentityUser, CosmosAPIDataContext>();
            services.AddIdentityServer().AddApiAuthorization<Identity, CosmosAPIDataContext>();
            //services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityDbContext>();


            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.Cookie.Name = "NekoReactPortfolioCookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Identity/Account/Login";
                //ReturnUrlParameter = requires;
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = "https://nekoreactportfolio.azurewebsites.net/";
                options.SlidingExpiration = true;
            });

            //services.AddScoped<TokenService>();
            services.AddTransient(serviceProvider => new TokenService(Configuration, tokenKey, authDomain, authAudience));

            //services.AddAuthorization(options =>
            //{
            //    options.FallbackPolicy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build();
            //});


            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:messages", policy => policy.Requirements.Add(new HasScopeRequirement("read:messages", domain)));
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();



            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("https://localhost:44300");
                });
            });


            // uncomment this if you want to use authentication with swagger
 //           services.AddSwaggerGen(c =>
 //           {
 //               c.SwaggerDoc("v1", new OpenApiInfo { Title = "CosmosAPI", Version = "v1" });
 //               c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
 //               {
 //                   Name = "Authorization",
 //                   Type = SecuritySchemeType.ApiKey,
 //                   Scheme = "Bearer",
 //                   BearerFormat = "JWT",
 //                   In = ParameterLocation.Header,
 //                   Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
 //               });
 //               c.AddSecurityRequirement(new OpenApiSecurityRequirement
 //{
 //    {
 //          new OpenApiSecurityScheme
 //            {
 //                Reference = new OpenApiReference
 //                {
 //                    Type = ReferenceType.SecurityScheme,
 //                    Id = "Bearer"
 //                }
 //            },
 //            new string[] {}
 //    }
 //});
 //           });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CosmosAPI v1"));
            }

            

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }


}
