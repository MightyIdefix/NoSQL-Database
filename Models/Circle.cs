using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAB3.Models
{
    public class Circle
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public List<string> UserIds { get; set; } = new List<string>();
        public string CircleName { get; set; }
        public string CircleOwner { get; set; }
        public List<Posts> Posts { get; set; } = new List<Posts>();
    }
}
