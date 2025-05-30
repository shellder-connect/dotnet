using System;
using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class RegistroEventoDTO
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "A descrição do evento é obrigatória.")]
    [StringLength(255, ErrorMessage = "A descrição não pode exceder 255 caracteres.")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "A data e hora do evento são obrigatórias.")]
    public DateTime DataHora { get; set; }

    [Required(ErrorMessage = "O ID do usuário responsável é obrigatório.")]
    public string? IdUsuario { get; set; }

    [Required(ErrorMessage = "A localização do evento é obrigatória.")]
    [StringLength(200, ErrorMessage = "A localização não pode exceder 200 caracteres.")]
    public string? Localizacao { get; set; }

    [Required(ErrorMessage = "O ID do abrigo é obrigatório.")]
    public string? IdAbrigo { get; set; }
}