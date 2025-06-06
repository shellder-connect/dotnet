

namespace Project.Models;
public class ProximidadeDto
{
    public string? IdUsuario { get; set; }
    public string? NomeUsuario { get; set; }
    public string? EmailUsuario { get; set; }
    public string? EnderecoUsuario { get; set; }
    public string? IdAbrigo { get; set; }
    public string? EnderecoAbrigo { get; set; }
    public int? CapacidadeAbrigo { get; set; }
    public int? OcupacaoAtual { get; set; }
    public double DistanciaKm { get; set; }
    public int Ranking { get; set; }
    public DateTime? DataCalculo { get; set; }
}