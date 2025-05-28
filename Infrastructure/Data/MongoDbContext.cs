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
    }
}
