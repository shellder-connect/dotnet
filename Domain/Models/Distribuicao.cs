using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.Models;

public class Distribuicao
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? IdDoacao { get; set; }

    public int QuantidadeDestinada { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime DataDestinada { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? IdPessoaAtendida { get; set; }
}