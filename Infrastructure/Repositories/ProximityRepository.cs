using MongoDB.Driver;
using Project.Models;
using Project.Infrastructure.Interfaces;
using Project.Domain;
using Microsoft.Extensions.Options;

namespace Project.Repositories
{
    public class ProximityRepository : IProximityRepository
    {
        private readonly IMongoCollection<Usuario> _usuarioCollection;
        private readonly IMongoCollection<Endereco> _enderecoCollection;
        private readonly IMongoCollection<Abrigo> _abrigoCollection;
        private readonly IMongoCollection<EnderecoAbrigo> _enderecosAbrigos;
        private readonly IMongoCollection<Coordenada> _coordenadas;
        private readonly IMongoCollection<AnaliseProximidade> _analiseProximidade;
        private readonly IMongoCollection<RelatorioProximidade> _relatorioProximidade;

        public ProximityRepository(IMongoClient mongoClient, IOptions<ConfigMongoDb> config)
        {
            var database = mongoClient.GetDatabase(config.Value.DatabaseName);
            
            _usuarioCollection = database.GetCollection<Usuario>(config.Value.UsuarioCollectionName);
            _enderecoCollection = database.GetCollection<Endereco>(config.Value.EnderecoCollectionName);
            _abrigoCollection = database.GetCollection<Abrigo>(config.Value.AbrigoCollectionName);
            _enderecosAbrigos = database.GetCollection<EnderecoAbrigo>(config.Value.EnderecoAbrigoCollectionName);
            _coordenadas = database.GetCollection<Coordenada>(config.Value.CoordenadaCollectionName);
            _analiseProximidade = database.GetCollection<AnaliseProximidade>(config.Value.AnaliseProximidadeCollectionName);
            _relatorioProximidade = database.GetCollection<RelatorioProximidade>(config.Value.RelatorioProximidadeCollectionName);
        }

        #region Operações com Usuários

        public async Task<List<Usuario>> BuscarTodosUsuarios()
        {
            return await _usuarioCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Usuario?> BuscarUsuarioPorId(string id)
        {
            return await _usuarioCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        #endregion

        #region Operações com Endereços

        public async Task<List<Endereco>> BuscarTodosEnderecos()
        {
            return await _enderecoCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Endereco?> BuscarEnderecoPorId(string id)
        {
            return await _enderecoCollection.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Endereco?> BuscarEnderecoPorUsuario(string idUsuario)
        {
            return await _enderecoCollection.Find(e => e.IdUsuario == idUsuario).FirstOrDefaultAsync();
        }

        #endregion

        #region Operações com Abrigos

        public async Task<List<Abrigo>> BuscarTodosAbrigos()
        {
            return await _abrigoCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Abrigo?> BuscarAbrigoPorId(string id)
        {
            return await _abrigoCollection.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        #endregion

        #region Operações com Endereços de Abrigos

        public async Task<List<EnderecoAbrigo>> BuscarTodosEnderecosAbrigos()
        {
            return await _enderecosAbrigos.Find(_ => true).ToListAsync();
        }

        public async Task<EnderecoAbrigo?> BuscarEnderecoAbrigoPorId(string id)
        {
            return await _enderecosAbrigos.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<EnderecoAbrigo?> BuscarEnderecoAbrigoPorAbrigo(string idAbrigo)
        {
            return await _enderecosAbrigos.Find(e => e.IdAbrigo == idAbrigo).FirstOrDefaultAsync();
        }

        #endregion

        #region Operações com Coordenadas

        public async Task<Coordenada> CriarCoordenada(Coordenada coordenada)
        {
            await _coordenadas.InsertOneAsync(coordenada);
            return coordenada;
        }

        public async Task<Coordenada?> BuscarCoordenadaPorReferencia(string tipo, string referenciaId)
        {
            return await _coordenadas
                .Find(c => c.Tipo == tipo && c.ReferenciaId == referenciaId && c.Status == "Ativo")
                .FirstOrDefaultAsync();
        }

        public async Task<List<Coordenada>> BuscarCoordenadasPorTipo(string tipo)
        {
            return await _coordenadas
                .Find(c => c.Tipo == tipo && c.Status == "Ativo" && c.Latitude.HasValue && c.Longitude.HasValue)
                .ToListAsync();
        }

        public async Task<Coordenada?> AtualizarCoordenada(Coordenada coordenada)
        {
            var result = await _coordenadas.ReplaceOneAsync(
                c => c.Id == coordenada.Id,
                coordenada
            );
            return result.ModifiedCount > 0 ? coordenada : null;
        }

        #endregion

        #region Operações com Análise de Proximidade

        public async Task<bool> LimparAnaliseProximidade()
        {
            var result = await _analiseProximidade.DeleteManyAsync(_ => true);
            return result.IsAcknowledged;
        }

        public async Task<bool> CriarAnaliseProximidade(List<AnaliseProximidade> analises)
        {
            if (analises?.Any() == true)
            {
                await _analiseProximidade.InsertManyAsync(analises);
                return true;
            }
            return false;
        }

        public async Task<List<AnaliseProximidade>> BuscarAnaliseProximidade(string? idUsuario = null)
        {
            var filtro = idUsuario != null 
                ? Builders<AnaliseProximidade>.Filter.Eq(a => a.IdUsuario, idUsuario)
                : Builders<AnaliseProximidade>.Filter.Empty;

            return await _analiseProximidade
                .Find(filtro)
                .SortBy(a => a.IdUsuario)
                .ThenBy(a => a.Ranking)
                .ToListAsync();
        }

        public async Task<List<AnaliseProximidade>> BuscarTop3ProximidadePorUsuario(string idUsuario)
        {
            return await _analiseProximidade
                .Find(a => a.IdUsuario == idUsuario)
                .SortBy(a => a.Ranking)
                .Limit(3)
                .ToListAsync();
        }

        #endregion

        #region Operações com Relatório de Proximidade

        public async Task<RelatorioProximidade> CriarRelatorioProximidade(RelatorioProximidade relatorio)
        {
            await _relatorioProximidade.InsertOneAsync(relatorio);
            return relatorio;
        }

        public async Task<RelatorioProximidade?> BuscarUltimoRelatorioProximidade()
        {
            return await _relatorioProximidade
                .Find(_ => true)
                .SortByDescending(r => r.DataGeracao)
                .FirstOrDefaultAsync();
        }

        public async Task<List<RelatorioProximidade>> BuscarTodosRelatoriosProximidade()
        {
            return await _relatorioProximidade
                .Find(_ => true)
                .SortByDescending(r => r.DataGeracao)
                .ToListAsync();
        }

        #endregion

        #region Operações Auxiliares

        public async Task<long> ContarUsuarios()
        {
            return await _usuarioCollection.CountDocumentsAsync(_ => true);
        }

        public async Task<long> ContarAbrigos()
        {
            return await _abrigoCollection.CountDocumentsAsync(_ => true);
        }

        public async Task<long> ContarCoordenadasValidas(string tipo)
        {
            return await _coordenadas.CountDocumentsAsync(
                c => c.Tipo == tipo && c.Status == "Ativo" && c.Latitude.HasValue && c.Longitude.HasValue
            );
        }

        public async Task<long> ContarAnaliseProximidade()
        {
            return await _analiseProximidade.CountDocumentsAsync(_ => true);
        }

        #endregion
    }
}