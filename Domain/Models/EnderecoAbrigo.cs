using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.Models
{
    public class EnderecoAbrigo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdAbrigo { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
        public string? Estado { get; set; }
        public string? Cidade { get; set; }
        public string? Bairro { get; set; }
        public string? Rua { get; set; }
    }
}