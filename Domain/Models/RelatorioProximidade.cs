using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.Models;

public class RelatorioProximidade
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("dataGeracao")]
    public DateTime DataGeracao { get; set; } = DateTime.UtcNow;

    [BsonElement("totalUsuarios")]
    public int TotalUsuarios { get; set; }

    [BsonElement("totalAbrigos")]
    public int TotalAbrigos { get; set; }

    [BsonElement("totalCalculos")]
    public int TotalCalculos { get; set; }

    [BsonElement("usuariosComCoordenadas")]
    public int UsuariosComCoordenadas { get; set; }

    [BsonElement("abrigosComCoordenadas")]
    public int AbrigosComCoordenadas { get; set; }

    [BsonElement("distanciaMediaKm")]
    public double? DistanciaMediaKm { get; set; }

    [BsonElement("distanciaMinimaKm")]
    public double? DistanciaMinimaKm { get; set; }

    [BsonElement("distanciaMaximaKm")]
    public double? DistanciaMaximaKm { get; set; }

    [BsonElement("status")]
    public string Status { get; set; } = "Gerado";

    [BsonElement("observacoes")]
    public string? Observacoes { get; set; }
}