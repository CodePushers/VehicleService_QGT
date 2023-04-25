using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Runtime.ConstrainedExecution;

namespace VehicleServiceAPI.Model
{
	public class VehicleDTO
	{
        public string Brand { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        public int Mileage { get; set; }
        public List<ServiceHistory> ServiceHistory { get; set; }
        public List<Image> ImageHistory { get; set; }

        public VehicleDTO(string brand, string model, string registrationNumber, int mileage, List<ServiceHistory> serviceHistory, List<Image> imageHistory)
        {
            this.Brand = brand;
            this.Model = model;
            this.RegistrationNumber = registrationNumber;
            this.Mileage = mileage;
            this.ServiceHistory = serviceHistory;
            this.ImageHistory = imageHistory;
        }

        public VehicleDTO()
        {
        }

    }
}

