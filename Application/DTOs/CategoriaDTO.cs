using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.Models;

public class CategoriaDTO
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required(ErrorMessage = "A descrição do tipo de usuário é obrigatória.")]
    [StringLength(50, ErrorMessage = "A descrição não pode ter mais de 50 caracteres.")]
    public string? Descricao { get; set; }

}
