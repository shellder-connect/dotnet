using MongoDB.Driver;
using Project.Models;
using Project.Infrastructure.Interfaces;
using Project.Domain;
using Microsoft.Extensions.Options;

namespace Project.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IMongoCollection<Usuario> _usuarioCollection;

        public UsuarioRepository(IOptions<ConfigMongoDb> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _usuarioCollection = mongoDatabase.GetCollection<Usuario>(settings.Value.UsuarioCollectionName);
        }

        // 👇 Novo construtor para testes (injeção direta do mock)
        public UsuarioRepository(IMongoCollection<Usuario> usuarioCollection)
        {
            _usuarioCollection = usuarioCollection;
        }

        public async Task<Usuario> Criar(Usuario usuario)
        {
            await _usuarioCollection.InsertOneAsync(usuario);
            return usuario;
        }

        public async Task<Usuario> ConsultarId(string id)
        {
            var filtro = Builders<Usuario>.Filter.Eq(u => u.Id, id);
            return await _usuarioCollection.Find(filtro).FirstOrDefaultAsync();
        }

        public async Task<List<Usuario>> ConsultarTodos()
        {
            return await _usuarioCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Usuario?> Atualizar(Usuario usuario)
        {
            var filtro = Builders<Usuario>.Filter.Eq(u => u.Id, usuario.Id);

            var update = Builders<Usuario>.Update
                .Set(u => u.Nome, usuario.Nome)
                .Set(u => u.Email, usuario.Email)
                .Set(u => u.Senha, usuario.Senha)
                .Set(u => u.IdTipoUsuario, usuario.IdTipoUsuario)
                .Set(u => u.Telefone, usuario.Telefone)
                .Set(u => u.IdEndereco, usuario.IdEndereco)
                .Set(u => u.DataNascimento, usuario.DataNascimento)
                .Set(u => u.Documento, usuario.Documento)
                .Set(u => u.Status, usuario.Status);

            var resultado = await _usuarioCollection.UpdateOneAsync(filtro, update);

            if (resultado.ModifiedCount > 0)
            {
                return usuario;
            }

            return null;
        }

        public async Task<Usuario?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            var usuario = await ConsultarId(id);
            if (usuario == null)
            {
                return null;
            }

            var updateDefinitionBuilder = Builders<Usuario>.Update;
            var updateDefinitions = new List<UpdateDefinition<Usuario>>();

            foreach (var campo in camposParaAtualizar)
            {
                switch (campo.Key.ToLower())
                {
                    case "nome":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.Nome, campo.Value.ToString()));
                        break;
                    case "email":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.Email, campo.Value.ToString()));
                    break;
                    case "senha":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.Senha, campo.Value.ToString()));
                        break;
                    case "idtipousuario":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.IdTipoUsuario, campo.Value.ToString()));
                        break;
                    case "telefone":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.Telefone, campo.Value.ToString()));
                        break;
                    case "idendereco":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.IdEndereco, campo.Value.ToString()));
                        break;
                    case "documento":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.Documento, campo.Value.ToString()));
                        break;
                    case "status":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.Status, campo.Value.ToString()));
                        break;  
                }
            }

            if (updateDefinitions.Count == 0)
            {
                return usuario;
            }

            var combinedUpdate = updateDefinitionBuilder.Combine(updateDefinitions);
            
            var filtro = Builders<Usuario>.Filter.Eq(c => c.Id, id);
            
            var resultado = await _usuarioCollection.UpdateOneAsync(filtro, combinedUpdate);

            if (resultado.ModifiedCount > 0)
            {
                return await ConsultarId(id);
            }

            return null;
        }

        public async Task Excluir(string id)
        {
            var filtro = Builders<Usuario>.Filter.Eq(u => u.Id, id);
            await _usuarioCollection.DeleteOneAsync(filtro);
        }
    }
}
