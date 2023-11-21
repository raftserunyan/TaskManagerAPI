using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TaskManager.API.Extensions;
using TaskManager.Core.Extensions;
using TaskManager.Data.DAO;
using TaskManager.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

#region Services
// ---------------------- Add services to the container --------------------- //
// -------------------------------------------------------------------------- //

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EipqLibrary.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] { }
                    }
                });
    c.CustomSchemaIds(x => x.FullName);
});

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

// Configure DbContext
builder.Services.AddDbContext<TaskManagerDbContext>(
        opts => opts.UseSqlServer(builder.Configuration["ConnectionStrings"]),
        ServiceLifetime.Scoped
       );

// Configure Identity
builder.Services.AddIdentity<User, IdentityRole<int>>(o => o.User.RequireUniqueEmail = true)
                .AddEntityFrameworkStores<TaskManagerDbContext>()
                .AddDefaultTokenProviders();

// Configure authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = false,
    ValidateAudience = false,
    ClockSkew = TimeSpan.Zero
};
builder.Services.AddSingleton(tokenValidationParameters);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = tokenValidationParameters;
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.RegisterServices();

var app = builder.Build();
#endregion

#region Pipeline
//--------------------- Configure the HTTP request pipeline  --------------------- //
// ------------------------------------------------------------------------------- //
app.UseCors("MyPolicy");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomExceptionHandling();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion