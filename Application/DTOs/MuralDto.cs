
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class MuralDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? IdUsuario { get; set; }
    public string? Titulo { get; set; }
    public string? Comentario { get; set; }
    public string? TipoPost { get; set; }
    public DateTime Data { get; set; }

}