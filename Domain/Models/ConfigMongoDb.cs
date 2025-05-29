using Project.Infrastructure;

namespace Project.Domain
    {
        public class ConfigMongoDb
        {
            public string ConnectionString { get; set; } = null!;
            public string DatabaseName { get; set; } = null!;
            public string DatabaseTestsName { get; set; } = null!;
            public string UsuarioCollectionName { get; set; } = null!;
            public string LoginCollectionName { get; set; } = null!;
            public string EnderecoCollectionName { get; set; } = null!;
            public string FeedbackCollectionName { get; set; } = null!;
            public string TipoUsuarioCollectionName { get; set; } = null!;

        public static implicit operator ConfigMongoDb(MongoDbContext v)
        {
            throw new NotImplementedException();
        }
    }
    }