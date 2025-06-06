using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.Models;
public class AnaliseProximidade
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("idUsuario")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdUsuario { get; set; } = string.Empty;

    [BsonElement("idAbrigo")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdAbrigo { get; set; } = string.Empty;

    [BsonElement("distanciaKm")]
    public double DistanciaKm { get; set; }

    [BsonElement("ranking")]
    public int Ranking { get; set; } // Posição no ranking para este usuário

    [BsonElement("dataCalculo")]
    public DateTime DataCalculo { get; set; } = DateTime.UtcNow;

    [BsonElement("coordenadaUsuarioId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? CoordenadaUsuarioId { get; set; }

    [BsonElement("coordenadaAbrigoId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? CoordenadaAbrigoId { get; set; }
}
