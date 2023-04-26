using System.Threading;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text;
using VehicleServiceAPI.Model;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Authorization;

namespace VehicleServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class VehicleController : ControllerBase
{
    private readonly ILogger<VehicleController> _logger;
    private readonly IMongoCollection<Vehicle> _vehicles;
    private readonly IConfiguration _config;
    private readonly string _imagePath;


    public VehicleController(ILogger<VehicleController> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;

        _imagePath = config["ImagePath"] ?? "/srv";

        // Client
        var mongoClient = new MongoClient(_config["ConnectionURI"]);
        _logger.LogInformation($"[*] CONNECTION_URI: {_config["ConnectionURI"]}");

        // Database
        var database = mongoClient.GetDatabase(_config["DatabaseName"]);
        _logger.LogInformation($"[*] DATABASE: {_config["DatabaseName"]}");

        // Collection
        _vehicles = database.GetCollection<Vehicle>(_config["CollectionName"]);
        _logger.LogInformation($"[*] COLLECTION: {_config["CollectionName"]}");
    }

    //List vehicles info
    [Authorize]
    [HttpGet("getInfo")]
    public List<Vehicle> GetAllVehicles()
    {
        _logger.LogInformation("\nMetoden: GetAll() kaldt klokken {DT}", DateTime.UtcNow.ToLongTimeString());

        return _vehicles.Find(new BsonDocument()).ToList();
    }

    //Add new vehicle
    [HttpPost("addVehicle")]
    public async Task AddVehicle(VehicleDTO car)
    {
        _logger.LogInformation("\nMetoden: AddVehicle(Vehicle car) kaldt klokken {DT}", DateTime.UtcNow.ToLongTimeString());

        Vehicle vehicle = new Vehicle
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Brand = car.Brand,
            Model = car.Model,
            RegistrationNumber = car.RegistrationNumber,
            Mileage = car.Mileage,
            ServiceHistory = car.ServiceHistory,
            ImageHistory = car.ImageHistory
        };

        await _vehicles.InsertOneAsync(vehicle);

        return;
    }

    //Show vehicle info with image(s)
    [Authorize]
    [HttpGet("{regNumber}", Name = "VehicleInfo")]
    public Vehicle GetVehicleInfo(string regNumber)
    {
        _logger.LogInformation("\nMetoden: GetVehicleInfo(string regNumber) kaldt klokken {DT}", DateTime.UtcNow.ToLongTimeString());

        return _vehicles.Find(vehicle => vehicle.RegistrationNumber == regNumber).FirstOrDefault();
    }

    // Attach image to vehicle
    [Authorize]
    [HttpPost("{regNumber}", Name = "PostImage"), DisableRequestSizeLimit]
    public IActionResult UploadImage(string regNumber)
    {
        _logger.LogInformation($"Uploading image til: {regNumber}");

        List<Uri> images = new List<Uri>();
        try
        {
            foreach (var formFile in Request.Form.Files)
            {
                // Validate file type and size

                if (formFile.ContentType != "image/jpeg" && formFile.ContentType != "image/png")
                {
                    return BadRequest($"Invalid file type for file {formFile.FileName}. Only JPEG and PNG files are allowed.");
                }
                if (formFile.Length > 1048576) // 1MB
                {
                    return BadRequest($"File {formFile.FileName} is too large. Maximum file size is 1MB.");
                }
                if (formFile.Length > 0)
                {
                    var fileName = "image-" + Guid.NewGuid().ToString() + ".jpg";
                    var fullPath = _imagePath + Path.DirectorySeparatorChar + fileName;
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        formFile.CopyTo(stream);
                    }
                    var imageURI = new Uri(fileName, UriKind.RelativeOrAbsolute);
                    images.Add(imageURI);
                }
                else
                {
                    return BadRequest("Empty file submited.");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, $"Internal server error.");
        }
        //insert billedeurl- på bil id-
        var filter = Builders<Vehicle>.Filter.Eq("RegistrationNumber", regNumber); // Find the document to update

        var update = Builders<Vehicle>.Update.Push("ImageHistory", new Image

        {
            Location = _imagePath,
            Description = "Rids på fælgen",
            FileName = images[0].ToString(),
            AddedBy = "Lars"
        }); // Insert the new element into the array

        var result = _vehicles.UpdateOne(filter, update);

        Console.WriteLine($"{result.ModifiedCount} document(s) updated.");
        return Ok(images);
    }

}
