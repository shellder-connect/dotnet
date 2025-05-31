using MongoDB.Driver;
using Project.Models;
using Project.Infrastructure.Interfaces;
using Project.Domain;
using Microsoft.Extensions.Options;

namespace Project.Repositories
{
    public class MLRepository : IMLRepository
    {
        private readonly IMongoCollection<RegistroEvento> _eventoCollection;
        private readonly IMongoCollection<Abrigo> _abrigoCollection;
        private readonly IMongoCollection<Doacao> _doacaoCollection;
        private readonly IMongoCollection<Distribuicao> _distribuicaoCollection;

        public MLRepository(IOptions<ConfigMongoDb> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _eventoCollection = mongoDatabase.GetCollection<RegistroEvento>(settings.Value.RegistroEventoCollectionName);
            _abrigoCollection = mongoDatabase.GetCollection<Abrigo>(settings.Value.AbrigoCollectionName);
            _doacaoCollection = mongoDatabase.GetCollection<Doacao>(settings.Value.DoacaoCollectionName);
            _distribuicaoCollection = mongoDatabase.GetCollection<Distribuicao>(settings.Value.DistribuicaoCollectionName);
        }

        // Construtor para testes (injeção direta dos mocks)
        public MLRepository(
            IMongoCollection<RegistroEvento> eventoCollection,
            IMongoCollection<Abrigo> abrigoCollection,
            IMongoCollection<Doacao> doacaoCollection,
            IMongoCollection<Distribuicao> distribuicaoCollection)
        {
            _eventoCollection = eventoCollection;
            _abrigoCollection = abrigoCollection;
            _doacaoCollection = doacaoCollection;
            _distribuicaoCollection = distribuicaoCollection;
        }

        public async Task<List<DadosTreinamento>> ObterDadosParaTreinamento()
        {
            var dadosTreinamento = new List<DadosTreinamento>();

            // Buscar todos os eventos
            var eventos = await _eventoCollection.Find(_ => true).ToListAsync();
            Console.WriteLine($"Eventos encontrados: {eventos.Count}");

            // Agrupar eventos por localização e data
            var eventosPorDia = eventos
                .GroupBy(e => new { e.Localizacao, Data = e.DataHora.Date })
                .ToList();

            foreach (var grupo in eventosPorDia)
            {
                var localizacao = grupo.Key.Localizacao ?? "Desconhecido";
                var data = grupo.Key.Data;
                var quantidadeEventos = grupo.Count();

                // Buscar dados do abrigo
                var eventoComAbrigo = grupo.FirstOrDefault(e => !string.IsNullOrEmpty(e.IdAbrigo));
                var abrigo = eventoComAbrigo != null ? 
                    await ConsultarAbrigoPorId(eventoComAbrigo.IdAbrigo) : null;

                // Buscar doações e distribuições para essa data e localização
                var doacoes = await ObterDoacoesPorData(data);
                var distribuicoes = await ObterDistribuicoesPorData(data);

                // Calcular necessidades baseadas no histórico
                var alimentos = CalcularNecessidadeAlimentos(doacoes, distribuicoes, quantidadeEventos);
                var medicamentos = CalcularNecessidadeMedicamentos(doacoes, distribuicoes, quantidadeEventos);
                var cobertores = CalcularNecessidadeCobertores(doacoes, distribuicoes, quantidadeEventos);

                dadosTreinamento.Add(new DadosTreinamento
                {
                    Localizacao = localizacao,
                    QuantidadeEventos = quantidadeEventos,
                    CapacidadeAbrigo = abrigo?.CapacidadeTotal ?? 0,
                    OcupacaoAtual = abrigo?.OcupacaoAtual ?? 0,
                    MesAno = data.Month,
                    AlimentosNecessarios = alimentos,
                    MedicamentosNecessarios = medicamentos,
                    CobertoresNecessarios = cobertores
                });
            }

            return dadosTreinamento;
        }

        public async Task<List<DadosTreinamento>> ObterDadosPorLocalizacao(string localizacao)
        {
            var filtro = Builders<RegistroEvento>.Filter.Eq(e => e.Localizacao, localizacao);
            var eventos = await _eventoCollection.Find(filtro).ToListAsync();

            var dadosTreinamento = new List<DadosTreinamento>();

            var eventosPorDia = eventos
                .GroupBy(e => e.DataHora.Date)
                .ToList();

            foreach (var grupo in eventosPorDia)
            {
                var data = grupo.Key;
                var quantidadeEventos = grupo.Count();

                var eventoComAbrigo = grupo.FirstOrDefault(e => !string.IsNullOrEmpty(e.IdAbrigo));
                var abrigo = eventoComAbrigo != null ? 
                    await ConsultarAbrigoPorId(eventoComAbrigo.IdAbrigo) : null;

                var doacoes = await ObterDoacoesPorData(data);
                var distribuicoes = await ObterDistribuicoesPorData(data);

                var alimentos = CalcularNecessidadeAlimentos(doacoes, distribuicoes, quantidadeEventos);
                var medicamentos = CalcularNecessidadeMedicamentos(doacoes, distribuicoes, quantidadeEventos);
                var cobertores = CalcularNecessidadeCobertores(doacoes, distribuicoes, quantidadeEventos);

                dadosTreinamento.Add(new DadosTreinamento
                {
                    Localizacao = localizacao,
                    QuantidadeEventos = quantidadeEventos,
                    CapacidadeAbrigo = abrigo?.CapacidadeTotal ?? 0,
                    OcupacaoAtual = abrigo?.OcupacaoAtual ?? 0,
                    MesAno = data.Month,
                    AlimentosNecessarios = alimentos,
                    MedicamentosNecessarios = medicamentos,
                    CobertoresNecessarios = cobertores
                });
            }

            return dadosTreinamento;
        }

        public async Task<List<string>> ObterLocalizacoesAtivas()
        {
            var eventos = await _eventoCollection.Find(_ => true).ToListAsync();
            
            return eventos
                .Where(e => !string.IsNullOrEmpty(e.Localizacao))
                .Select(e => e.Localizacao!)
                .Distinct()
                .ToList();
        }

        private async Task<Abrigo?> ConsultarAbrigoPorId(string? idAbrigo)
        {
            if (string.IsNullOrEmpty(idAbrigo))
                return null;

            var filtro = Builders<Abrigo>.Filter.Eq(a => a.Id, idAbrigo);
            return await _abrigoCollection.Find(filtro).FirstOrDefaultAsync();
        }

        private async Task<List<Doacao>> ObterDoacoesPorData(DateTime data)
        {
            // Como não temos data na Doacao, vamos buscar todas
            // Em um cenário real, você adicionaria um campo de data na Doacao
            return await _doacaoCollection.Find(_ => true).ToListAsync();
        }

        private async Task<List<Distribuicao>> ObterDistribuicoesPorData(DateTime data)
        {
            var filtro = Builders<Distribuicao>.Filter.Eq(d => d.DataDestinada.Date, data.Date);
            return await _distribuicaoCollection.Find(filtro).ToListAsync();
        }

        private float CalcularNecessidadeAlimentos(List<Doacao> doacoes, List<Distribuicao> distribuicoes, int quantidadeEventos)
        {
            // Filtrar doações de alimentos por descrição
            var doacoesAlimentos = doacoes.Where(d => 
                d.Descricao != null && (
                    d.Descricao.ToLower().Contains("alimento") ||
                    d.Descricao.ToLower().Contains("comida") ||
                    d.Descricao.ToLower().Contains("agua")
                )).Sum(d => d.Quantidade);

            // Estimar necessidade baseada em eventos + distribuições
            var distribuicoesTotais = distribuicoes.Sum(d => d.QuantidadeDestinada);
            
            // Fórmula simples: doações + estimativa baseada em eventos
            return doacoesAlimentos + (quantidadeEventos * 2) + (distribuicoesTotais * 0.6f);
        }

        private float CalcularNecessidadeMedicamentos(List<Doacao> doacoes, List<Distribuicao> distribuicoes, int quantidadeEventos)
        {
            var doacoesMedicamentos = doacoes.Where(d => 
                d.Descricao != null && (
                    d.Descricao.ToLower().Contains("medicamento") ||
                    d.Descricao.ToLower().Contains("remedio")
                )).Sum(d => d.Quantidade);

            var distribuicoesTotais = distribuicoes.Sum(d => d.QuantidadeDestinada);
            
            return doacoesMedicamentos + (quantidadeEventos * 0.5f) + (distribuicoesTotais * 0.2f);
        }

        private float CalcularNecessidadeCobertores(List<Doacao> doacoes, List<Distribuicao> distribuicoes, int quantidadeEventos)
        {
            var doacoesCobertores = doacoes.Where(d => 
                d.Descricao != null && (
                    d.Descricao.ToLower().Contains("cobertor") ||
                    d.Descricao.ToLower().Contains("roupa") ||
                    d.Descricao.ToLower().Contains("agasalho")
                )).Sum(d => d.Quantidade);

            var distribuicoesTotais = distribuicoes.Sum(d => d.QuantidadeDestinada);
            
            return doacoesCobertores + (quantidadeEventos * 0.3f) + (distribuicoesTotais * 0.2f);
        }
    }
}