using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentApi;
using StudentApi.Authorization;
using StudentApi.Data;
using StudentDataAccessLayer;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<attachmentOptions>(builder.Configuration.GetSection("Attachments"));


builder.Services.AddDbContext<AppDbcontext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<PermissionBasedAuthorizationFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

/*-----------------------------Swagger PArt-----------------------------*/
#region Swagger REgion
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation    
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASP.NET 8 Web API",
        Description = " Hassan Projrcy"
    });
    // To Enable authorization using Swagger (JWT)    
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] {}
                    }
                    });
});
#endregion

//builder.Services.AddSwaggerGen();

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();

builder.Services.AddSingleton(jwtOptions!);      // هنا للتسجيل 

builder.Services.AddScoped<IStudentsData, StudentsData>();



builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

    }
    )
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions!.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
