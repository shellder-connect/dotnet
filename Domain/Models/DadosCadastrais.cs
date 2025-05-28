
namespace Project.Models;

public class DadosCadastrais
{
    public required Usuario Usuario { get; set; } = new Usuario();
    public required Endereco Endereco { get; set; } = new Endereco();
    
}