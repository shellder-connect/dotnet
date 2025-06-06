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
            public string CategoriaCollectionName { get; set; } = null!;
            public string AbrigoCollectionName { get; set; } = null!;
            public string DoacaoCollectionName { get; set; } = null!;
            public string DistribuicaoCollectionName { get; set; } = null!;
            public string RegistroEventoCollectionName { get; set; } = null!;
            public string LocalidadeCollectionName { get; set; } = null!;
            public string MuralCollectionName { get; set; } = null!;
            public string EnderecoAbrigoCollectionName { get; set; } = null!;
            public string CoordenadaCollectionName { get; set; } = null!;
            public string AnaliseProximidadeCollectionName { get; set; } = null!;
            public string RelatorioProximidadeCollectionName { get; set; } = null!;

        public static implicit operator ConfigMongoDb(MongoDbContext v)
        {
            throw new NotImplementedException();
        }
    }
    }