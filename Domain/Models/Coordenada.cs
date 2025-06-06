using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.Models;
// Model para armazenar coordenadas geográficas
public class Coordenada
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("tipo")]
    public string Tipo { get; set; } = string.Empty; 

    [BsonElement("referenciaId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ReferenciaId { get; set; } = string.Empty;

    [BsonElement("enderecoId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? EnderecoId { get; set; } 

    [BsonElement("latitude")]
    public double? Latitude { get; set; }

    [BsonElement("longitude")]
    public double? Longitude { get; set; }

    [BsonElement("enderecoCompleto")]
    public string? EnderecoCompleto { get; set; }

    [BsonElement("dataAtualizacao")]
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

    [BsonElement("status")]
    public string Status { get; set; } = "Ativo";
}
