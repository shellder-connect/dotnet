using Project.Models;
using MongoDB.Driver;

public class DadosCadastraisRepository : IDadosCadastraisRepository
{
    private readonly IMongoCollection<Usuario> _usuarios;
    private readonly IMongoCollection<Endereco> _enderecos;

    public DadosCadastraisRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("Project");

        _usuarios = database.GetCollection<Usuario>("t_usuario");
        _enderecos = database.GetCollection<Endereco>("t_endereco");
    }

    public Usuario ObterUsuarioId(string usuarioId)
    {
        var usuario = _usuarios.Find(u => u.Id == usuarioId).FirstOrDefault();
        Console.WriteLine(usuario != null ? $"Usuário encontrado: {usuario.Nome}" : "Usuário não encontrado.");
        return _usuarios.Find(u => u.Id == usuarioId).FirstOrDefault();
    }

    public Endereco ObterEnderecoId(string usuarioId)
    {
        var endereco = _enderecos.Find(e => e.IdUsuario == usuarioId).FirstOrDefault();
        Console.WriteLine(endereco != null ? $"Endereço encontrado para usuário {usuarioId}" : "Endereço não encontrado.");
        return _enderecos.Find(e => e.IdUsuario == usuarioId).FirstOrDefault();
    }

}
