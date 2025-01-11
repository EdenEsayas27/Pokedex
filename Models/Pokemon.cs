using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace pokedex.Models
{
    [BsonIgnoreExtraElements] // Ignore fields in MongoDB that are not mapped to the class
    public class Pokemon
    {
        [BsonId] // MongoDB default _id
        public ObjectId Id { get; set; } // Non-nullable Id handled by MongoDB

        [BsonIgnore] // Ignore this property in MongoDB serialization
        public string IdString => Id.ToString();

        public string? Name { get; set; } // Nullable string
        public string? Type { get; set; } // Nullable string
        public string? Ability { get; set; } // Nullable string
        public int? Level { get; set; } // Nullable int
        public DateTime? IdTimestamp { get; set; } // Nullable DateTime
    }
}
