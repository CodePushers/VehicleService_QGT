using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

var builder = WebApplication.CreateBuilder(args);

string mySecret = Environment.GetEnvironmentVariable("Secret") ?? "none";
string myIssuer = Environment.GetEnvironmentVariable("Issuer") ?? "none";

logger.Info($"MySecret: {mySecret}, MyIssuer: {myIssuer}");

builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = myIssuer,
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mySecret))
    };
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Den følgende stump kode skal tilføjes til Program.cs

// Setup application to handle download of static content (images)
// from a predetermined folder
//var imagePath = builder.Configuration["ImagePath"];
//var fileProvider = new PhysicalFileProvider(Path.GetFullPath(imagePath));
//var requestPath = new PathString("/images");
//app.UseStaticFiles(new StaticFileOptions()
//{
//    FileProvider = fileProvider,
//    RequestPath = requestPath
//});
//Console.WriteLine("File Provider Root: " + fileProvider.Root);
//Console.WriteLine("requestPath" + requestPath);

app.Run();
