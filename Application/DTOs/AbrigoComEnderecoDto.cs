
namespace Project.Models;
public class AbrigoComEnderecoDto
{
    public string? IdAbrigo { get; set; }
    public string? CEP { get; set; }
    public string? EnderecoCompleto { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int? CapacidadeTotal { get; set; }
    public int? OcupacaoAtual { get; set; }
    public string? Descricao { get; set; }
    public string? Status { get; set; }
}