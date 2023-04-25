using System.Globalization;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace VehicleServiceAPI.Model
{
	public class Image
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FileName { get; set; }
        public string Location { get; set; }
        [BsonElement]
        public DateTime? Date { get; set; }  = DateTime.UtcNow;
        public string Description { get; set; }
        public string AddedBy { get; set; }
    }
}