using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Text;
using PSAM.Services.IServices;
using PSAM.Services;
using PSAM.Entities;
using PSAM.Repositories;
using PSAM.Repositories.IRepositories;
using WebGame.Exceptions;

namespace PSAM
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            byte[] bytes = Encoding.ASCII.GetBytes("e09f27a6f041a1e13a4b5dc7e3d20ebc");
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Authorisation for services
            builder.Services.AddAuthorization(options =>
            {
                // options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, nameof(RoleType.moderator)));
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddJwtBearer(options =>
             {
                 options.RequireHttpsMetadata = false;
                 options.SaveToken = true;
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(bytes),
                     ValidateIssuer = false,
                     ValidateAudience = false
                 };
             });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Example API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Description = "Please insert JWT token into field"
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
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // JSON serializer
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
               options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
               .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            // Register IHttpContextAccessor before AuthService
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Register services
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<ILikeService, LikeService>();
            builder.Services.AddScoped<ICommentService, CommentService>();

            // Link with database
            builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(@"Server=(localdb)\MSSqlLocalDb;Database=psamDb;Trusted_Connection=True;"));
            builder.Services.AddAutoMapper(typeof(Program));

            // Register repositories
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<ITechnologyRepository, TechnologyRepository>();
            builder.Services.AddScoped<ISubscribersRepository, SubscribersRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<ILikeRepository, LikeRepository>();


            // Add Cors
            builder.Services.AddCors(opt => { opt.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); });

            var app = builder.Build();

            // Enable CORS
            app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

