using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class DoacaoDTO
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "O ID do abrigo é obrigatório.")]
    public string? IdAbrigo { get; set; }

    [StringLength(200, ErrorMessage = "A descrição não pode exceder 200 caracteres.")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "O ID da categoria é obrigatório.")]
    public string? IdCategoria { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
    public int Quantidade { get; set; }
}