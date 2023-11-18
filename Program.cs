using amazonCloneWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using amazonCloneWebAPI.Handler;
using amazonCloneWebAPI.Container;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//This is for JWT Authentication
var _authKey = builder.Configuration.GetValue<string>("JwtSettings:securityKey");
builder.Services.AddAuthentication(item => {item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item => {
    item.RequireHttpsMetadata = true;
    item.SaveToken= true;
    item.TokenValidationParameters = new TokenValidationParameters(){
        ValidateIssuerSigningKey =true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//This is for Basic Authentication
//builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddDbContext<AmazonCloneContext>(options=>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("constring"));
});

builder.Services.AddScoped<IProductContainer, ProductContainer>();
builder.Services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();

var autoMapper = new MapperConfiguration(item=> item.AddProfile(new AutoMapperHandler()));
IMapper mapper = autoMapper.CreateMapper();
builder.Services.AddSingleton(mapper);

var logger = new LoggerConfiguration().WriteTo.File("C:\\Users\\schit\\OneDrive\\MIB\\WatchLog.log", rollingInterval:RollingInterval.Day)
.CreateLogger();
builder.Logging.AddSerilog();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
