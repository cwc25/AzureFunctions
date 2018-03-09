using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.Model
{
    public class CircleData
    {
        [BsonElement("ImagePath")]
        public string ImagePath { get; set; }
        [BsonElement("Content")]
        public string Content { get; set; }
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }
        [BsonElement("DriverId")]
        public string DriverId { get; set; }
    }
}
