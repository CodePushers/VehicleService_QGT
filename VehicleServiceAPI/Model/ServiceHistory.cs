using System.Globalization;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace VehicleServiceAPI.Model
{
	public class ServiceHistory
	{
        [BsonElement]
        public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public string ServicedBy { get; set; }
    }
}