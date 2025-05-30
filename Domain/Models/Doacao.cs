using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.Models;

public class Doacao
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? IdAbrigo { get; set; } // Referência ao Abrigo (T_Abrigo)

    public string? Descricao { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? IdCategoria { get; set; } // Referência à Categoria (T_Categoria)

    public int Quantidade { get; set; }
}