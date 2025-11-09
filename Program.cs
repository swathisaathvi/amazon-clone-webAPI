using amazonCloneWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using amazonCloneWebAPI.Handler;
using amazonCloneWebAPI.ProudctServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAuthorizationHandler, CustomRolesAuthorizationHandler>();
//builder.Services.AddAuthorization(options => 
//    {
//        options.AddPolicy("DevelopmentPolicy", policy =>{
//            policy.Requirements.Add(new CustomRolesRequirement(new[] {"Admin","User"}));
//        });
//        options.AddPolicy("AdminToCreateProduct", policy =>{
//            policy.Requirements.Add(new CustomRolesRequirement(new[] {"Admin"}));
//        });
//    }
//);

//Authorization (no roles/policies, just require authentication)
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost6379";
    options.InstanceName = "RedisDemo";
});

var authenticationType = builder.Configuration.GetValue<string>("Authentication:Type");
switch (authenticationType)
{
    case "JWT":
        ConfigureJwtAuthentication(builder.Services);
        break;
    case "IdentityServer":
        ConfigureIdentityServerAuthentication(builder.Services);
        break;
    case "Basic":
        ConfigureBasicAuthentication(builder.Services);
        break;
    default: throw new InvalidOperationException(
        "Invalid Authentication type specified in appsettings.");
}

////This is for JWT Authentication
//var _authKey = builder.Configuration.GetValue<string>("JwtSettings:securityKey");
//builder.Services.AddAuthentication(item => {
//    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(item => {
//    item.RequireHttpsMetadata = true;
//    item.SaveToken= true;
//    item.TokenValidationParameters = new TokenValidationParameters(){
//        ValidateIssuerSigningKey =true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authKey)),
//        ValidateIssuer = false,
//        ValidateAudience = false
//    };
//});

////This is for Identity Server Authentication
//var configIdentity = builder.Services.AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme);
//configIdentity.AddJwtBearer(options => {
//    options.Authority = "https://localhost:5000";
//    options.Audience = "amazonCloneWebAPI";
//    options.RequireHttpsMetadata = false;
//});

////This is for Basic Authentication
//builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddDbContext<AmazonCloneContext>(options=>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("constring"));
});

builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();

var autoMapper = new MapperConfiguration(item=> item.AddProfile(new AutoMapperHandler()));
IMapper mapper = autoMapper.CreateMapper();
builder.Services.AddSingleton(mapper);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext()
.CreateLogger();
builder.Logging.AddSerilog(logger);

var _jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(_jwtSettings);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureJwtAuthentication(IServiceCollection services)
{
    var _authKey = builder.Configuration.GetValue<string>("JwtSettings:securityKey");
    services.AddAuthentication(item =>
    {
        item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(item =>
    {
        item.RequireHttpsMetadata = true;
        item.SaveToken = true;
        item.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
}

void ConfigureIdentityServerAuthentication(IServiceCollection services)
{
    services.AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = "https://localhost:5000";
            options.Audience = "amazonCloneWebAPI";
            options.RequireHttpsMetadata = false;
        });
}

void ConfigureBasicAuthentication(IServiceCollection services)
{
    services.AddAuthentication("BasicAuthentication")
        .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
}
