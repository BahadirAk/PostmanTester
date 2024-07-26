using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using PostmanTester.Application.Constants;
using PostmanTester.Application.Helpers;
using PostmanTester.Application.Interfaces.ExternalServices;
using PostmanTester.Application.Middlewares;
using PostmanTester.Application.Models.DataObjects;
using PostmanTester.Application.Models.Results;
using PostmanTester.Infrastructure;
using PostmanTester.Persistance;
using PostmanTester.Persistance.Helpers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "PostmanTester API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Cookie,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var tokenOptionsSection = builder.Configuration.GetSection("TokenOptions");
builder.Services.Configure<TokenOptions>(tokenOptionsSection);

//Get Secret Key as byte array
var tokenOptions = tokenOptionsSection.Get<TokenOptions>();
var key = Encoding.ASCII.GetBytes(tokenOptions?.SecurityKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
                //JWT settings.
                .AddJwtBearer(x =>
                {
                    //Accept only SSL or HTTPS?
                    x.RequireHttpsMetadata = false;
                    //Save to db if approved
                    x.SaveToken = false;
                    //Defines whats going to be controlled
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        //Sign check
                        ValidateIssuerSigningKey = true,
                        //Control it with
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        //Don't validate
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    x.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var endpoint = context.HttpContext.GetEndpoint();
                            var allowAnonymous = endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null;

                            if (allowAnonymous)
                            {
                                return Task.CompletedTask;  // Skip further validation
                            }

                            context.Request.Headers.TryGetValue("Authorization", out var token);
                            if (string.IsNullOrEmpty(token))
                            {
                                var result = new Result(false, Messages.TokenError[1], Messages.TokenError[0], StatusCodes.Status401Unauthorized);
                                return Task.FromException(new Exception(JsonConvert.SerializeObject(result)));
                            }
                            context.Token = TokenEncryptionHelper.DecryptString(token.ToString().Split(" ")[1]);
                            var tokenResult = ServiceTool.ServiceProvider.GetService<ITokenService>().GetTokenInfo(context.Token);
                            if (!tokenResult.Result.Success)
                            {
                                return Task.FromException(new Exception(JsonConvert.SerializeObject(tokenResult.Result)));
                            }
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = async context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(context.Exception.Message);
                            await context.Response.CompleteAsync();
                        }
                    };
                });

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
  options.AddPolicy("Development", builder =>
  {
      // Allow multiple HTTP methods  
      builder.WithMethods("GET", "POST", "PATCH", "DELETE", "OPTIONS")
        .WithHeaders(
          HeaderNames.Accept,
          HeaderNames.ContentType,
          HeaderNames.Authorization)
        .AllowCredentials()
        .SetIsOriginAllowed(origin =>
        {
            if (string.IsNullOrWhiteSpace(origin)) return false;
            if (origin.ToLower().StartsWith("http://localhost:5290")) return true;
            return false;
        });
  })
);

var app = builder.Build();

ServiceTool.ServiceProvider = app.Services;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Development");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseStaticFiles();

app.UseAuthorization();

#region SeedData
await SeedDataHelper.SeedAsync();
#endregion

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
