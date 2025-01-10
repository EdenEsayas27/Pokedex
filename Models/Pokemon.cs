using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace pokedex.Models
{
    public class Pokemon
    {
        [BsonId]  // This is MongoDB's default _id
        public ObjectId Id { get; set; }  // MongoDB handles it as ObjectId
        
        public string Name { get; set; }
        public string Type { get; set; }
        public string Ability { get; set; }
        public int Level { get; set; }

        // Add a custom property to serialize the ObjectId to string when returning to API
        [BsonIgnore]
        public string IdString
        {
            get { return Id.ToString(); }
        }
    }
}
