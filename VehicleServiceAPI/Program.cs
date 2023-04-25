using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

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

app.UseAuthorization();

app.MapControllers();

// Den følgende stump kode skal tilføjes til Program.cs

// Setup application to handle download of static content (images)
// from a predetermined folder
var imagePath = builder.Configuration["ImagePath"];
var fileProvider = new PhysicalFileProvider(Path.GetFullPath(imagePath));
var requestPath = new PathString("/images");
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = fileProvider,
    RequestPath = requestPath
});
Console.WriteLine("File Provider Root: " + fileProvider.Root);
Console.WriteLine("requestPath" + requestPath);

app.Run();
