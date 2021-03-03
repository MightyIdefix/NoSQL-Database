using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAB3.Models
{
    public class Comments
    {
        public string UserId { get; set; }
        public DateTime Time { get; set; }
        public string Text { get; set; }
    }
}
