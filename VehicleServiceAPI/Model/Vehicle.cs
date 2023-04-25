using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
namespace VehicleServiceAPI.Model
{
	public class Vehicle
	{
        [BsonId]
        [BsonElement(elementName:"_id")]
        public ObjectId Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        public int Mileage { get; set; }
        public List<string> ServiceHistory { get; set; }
        public List<string> ImageHistory { get; set; }

        public Vehicle(int id, string brand, string model, string registrationNumber, int mileage)
        {
            Id = id;
            Brand = brand;
            Model = model;
            RegistrationNumber = registrationNumber;
            Mileage = mileage;
            ServiceHistory = new List<string>();
            ImageHistory = new List<string>();
        }

        public void AddServiceRecord(string serviceRecord)
        {
            ServiceHistory.Add(serviceRecord);
        }

        public void AddImage(string imagePath)
        {
            ImageHistory.Add(imagePath);
        }
    }
}

