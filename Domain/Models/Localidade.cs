
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.Models;

    public class Localidade
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? IdUsuario { get; set; }
        public string? Nome { get; set; }
        public string? IdEndereco { get; set; }
        public string? CEPUsuario { get; set; }
        public string? IdAbrigo { get; set; }
        public string? CEPAbrigo { get; set; }
        public string? Distancia { get; set; }
    }