using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace VehicleServiceAPI.Model
{
	public class Vehicle
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        public int Mileage { get; set; }
        public List<ServiceHistory> ServiceHistory { get; set; }
        public List<Image> ImageHistory { get; set; }

    }
}

