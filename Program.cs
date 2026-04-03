using DevJobsAPI.Data;
using DevJobsAPI.Interfaces;
using DevJobsAPI.Middleware;
using DevJobsAPI.Models;
using DevJobsAPI.Repository;
using DevJobsAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // Allows Antigravity to connect
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();

// this code for telling the swagger to add a lock icon so i can paste my JWT token here
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "DevJobs API", Version = "v1" });

    // 1. This adds the "Authorize" button and defines HOW the security works
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    // 2. This tells Swagger to actually USE that security for every endpoint
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[]{}
        }
    });
});

builder.Services.AddScoped<ITokenService, TokenService>();
// This tells .NET: "Whenever a Controller asks for IJobRepository, give it JobRepository"
builder.Services.AddScoped<IJobRepository, JobRepository>();

builder.Services.AddScoped<ISavedJobRepository, SavedJobRepository>();

builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// This configures the Identity system to use our AppUser class and the default IdentityRole class, and it also sets some password requirements for security. We can adjust these requirements based on our needs, but we want to find a balance between security and usability. For example, we might want to require a longer password for better security, but we also don't want to make it too difficult for users to create passwords, so we need to find a balance between security and usability.
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false; // we can set this to false if we don't want to require special characters in the password, this will make it easier for users to create passwords, but it will also make it less secure, so we need to find a balance between security and usability.
    options.Password.RequiredLength = 10; // we can set this to a higher value if we want to require longer passwords, this will make it more secure, but it will also make it less usable, so we need to find a balance between security and usability.
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add authentication and JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };
});

var app = builder.Build();

// --- MIDDLEWARE PIPELINE ---

// We keep this outside the IF block for now so you can see real errors on Azure if things crash
app.UseDeveloperExceptionPage();

app.UseStaticFiles();

// 1. Move these OUTSIDE the if statement so they run on Azure (Production)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DocumentTitle = "DevJobs API - Jordan";
    c.InjectStylesheet("/Images/swagger-logo.css");
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevJobsAPI v1");
});

// 2. Keep the rest of the logic for development-only tools
if (app.Environment.IsDevelopment())
{
    // You can leave this empty or put developer-only exception pages here
}

app.UseHttpsRedirection();


app.UseCors();



// app.UseMiddleware<ExceptionMiddleware>(); // Commented out so we see raw errors for now

app.UseAuthentication(); // who are u?
app.UseAuthorization(); // are u allowed in?

app.MapControllers();

app.Run();