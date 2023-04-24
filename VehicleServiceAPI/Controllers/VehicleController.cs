using Microsoft.AspNetCore.Mvc;

namespace VehicleServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class VehicleController : ControllerBase
{

    private readonly ILogger<VehicleController> _logger;
    private readonly string _imagePath;

    public VehicleController(ILogger<VehicleController> logger, IConfiguration config)
    {
        _logger = logger;
        _imagePath = config["ImagePath"] ?? "/srv";
    }

    /*
     * 
    //Add new vehicle
    [HttpPost("addVehicle")]
    //Attach image to vehicle
    [HttpPost("uploadImage"), DisableRequestSizeLimit]

    //List vehicles info
    [HttpGet("listImages")]

    //Show vehicle info with image(s)
    [HttpGet("listVehicleInfo")]
    */
}
