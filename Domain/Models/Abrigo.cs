using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.Models;

public class Abrigo
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public int CapacidadeTotal { get; set; }
    public int OcupacaoAtual { get; set; }
    public string? Descricao { get; set; }
}