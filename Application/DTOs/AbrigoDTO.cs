using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.Models;

public class AbrigoDTO
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required(ErrorMessage = "A capacidade total é obrigatória.")]
    [Range(1, int.MaxValue, ErrorMessage = "A capacidade total deve ser maior que zero.")]
    public int CapacidadeTotal { get; set; }

    [Required(ErrorMessage = "A ocupação atual é obrigatória.")]
    [Range(0, int.MaxValue, ErrorMessage = "A ocupação atual não pode ser negativa.")]
    public int OcupacaoAtual { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(200, ErrorMessage = "A descrição não pode exceder 200 caracteres.")]
    public string? Descricao { get; set; }

    // Validação adicional para garantir que a ocupação não ultrapasse a capacidade
    public bool IsOcupacaoValida => OcupacaoAtual <= CapacidadeTotal;
}