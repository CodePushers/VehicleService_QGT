using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text;
using VehicleServiceAPI.Model;

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
    [HttpGet("getInfo")]
    public List<Vehicle> GetAllVehicles()
    {
        _logger.LogInformation("\nMetoden: GetAll() kaldt klokken {DT}", DateTime.UtcNow.ToLongTimeString());

        return _vehicles.Find(_ => true).ToList();
    }

    /*
     * 
    //Add new vehicle
    [HttpPost("addVehicle")]
    //Attach image to vehicle
    [HttpPost("uploadImage"), DisableRequestSizeLimit]

    //Show vehicle info with image(s)
    [HttpGet("listVehicleInfo")]
    */
}
