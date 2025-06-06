// DTOs para transferência de dados (não salvos no banco)

namespace Project.Models;

public class UsuarioComEnderecoDto
{
    public string? IdUsuario { get; set; }
    public string? NomeUsuario { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? CEP { get; set; }
    public string? EnderecoCompleto { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Status { get; set; }
}
