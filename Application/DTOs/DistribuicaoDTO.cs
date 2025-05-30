using System;
using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class DistribuicaoDTO
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "O ID da doação é obrigatório.")]
    public string? IdDoacao { get; set; }

    [Required(ErrorMessage = "A quantidade destinada é obrigatória.")]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade destinada deve ser maior que zero.")]
    public int QuantidadeDestinada { get; set; }

    [Required(ErrorMessage = "A data de destinação é obrigatória.")]
    public DateTime DataDestinada { get; set; }

    [Required(ErrorMessage = "O ID da pessoa atendida é obrigatório.")]
    public string? IdPessoaAtendida { get; set; }
}