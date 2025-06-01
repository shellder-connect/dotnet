using MongoDB.Driver;
using Project.Models;

namespace Project.Infrastructure
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDbConnection");
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("Global-Solution");
        }

        public IMongoCollection<Usuario> Usuarios => _database.GetCollection<Usuario>("t_usuario");
        public IMongoCollection<Login> Login => _database.GetCollection<Login>("t_login");
        public IMongoCollection<Endereco> Endereco => _database.GetCollection<Endereco>("t_endereco");
        public IMongoCollection<Feedback> Feedback => _database.GetCollection<Feedback>("t_feedback");
        public IMongoCollection<TipoUsuario> TipoUsuario => _database.GetCollection<TipoUsuario>("t_tipo_usuario");
        public IMongoCollection<Categoria> Categoria => _database.GetCollection<Categoria>("t_categoria");
        public IMongoCollection<Abrigo> Abrigo => _database.GetCollection<Abrigo>("t_abrigo");
        public IMongoCollection<Doacao> Doacao => _database.GetCollection<Doacao>("t_doacao");
        public IMongoCollection<Distribuicao> Distribuicao => _database.GetCollection<Distribuicao>("t_distribuicao");
        public IMongoCollection<RegistroEvento> RegistroEvento => _database.GetCollection<RegistroEvento>("t_registro_evento");
        public IMongoCollection<Localidade> Localidade => _database.GetCollection<Localidade>("t_localidade");
        public IMongoCollection<Mural> Mural => _database.GetCollection<Mural>("t_mural");
    }
}
