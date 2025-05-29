using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class TipoUsuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] 
        public string? Id { get; set; }

        public string Descricao { get; set; } 
    }
}
