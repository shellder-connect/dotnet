using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Project.Models;

public class RegistroEvento
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? Descricao { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime DataHora { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? IdUsuario { get; set; } 

    public string? Localizacao { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? IdAbrigo { get; set; }
}