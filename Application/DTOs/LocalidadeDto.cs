
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.Models;

    public class LocalidadeDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("idUsuario")]
        public string? IdUsuario { get; set; }

        [BsonElement("nome")]
        public string? Nome { get; set; }

        [BsonElement("idEndereco")]
        public string? IdEndereco { get; set; }

        [BsonElement("cepUsuario")]
        public string? CEPUsuario { get; set; }

        [BsonElement("idAbrigo")] 
        public string? IdAbrigo { get; set; }

        [BsonElement("cepAbrigo")]
        public string? CEPAbrigo { get; set; }

        [BsonElement("distancia")] 
        public string? Distancia { get; set; }
    }