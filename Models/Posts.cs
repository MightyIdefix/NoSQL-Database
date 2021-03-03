using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAB3.Models
{
    public class Posts
    {
        public string Id { get; set; }
        public string img { get; set; } = null;
        public string UserId { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public List<Comments> Comments { get; set; } = new List<Comments>();

        
    }
}
