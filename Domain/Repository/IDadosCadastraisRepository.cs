using Project.Models;

public interface IDadosCadastraisRepository
{
    Usuario ObterUsuarioId(string usuarioId);
    Endereco ObterEnderecoId(string usuarioId);
}
