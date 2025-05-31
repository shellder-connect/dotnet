using Microsoft.ML;
using Microsoft.ML.Data;
using Project.Infrastructure.Interfaces;
using Project.Models;
using System.Globalization;

namespace Project.Application.Services
{
    public class MLService : IMLService
    {
        private readonly IMLRepository _mlRepository;
        private readonly MLContext _mlContext;
        private ITransformer? _modeloAlimentos;
        private ITransformer? _modeloMedicamentos;
        private ITransformer? _modeloCobertores;

        public MLService(IMLRepository mlRepository)
        {
            _mlRepository = mlRepository;
            _mlContext = new MLContext(seed: 0);
        }

        // Método original (mantido para compatibilidade)
        public async Task<bool> TreinarModelo()
        {
            try
            {
                var dadosTreinamento = await _mlRepository.ObterDadosParaTreinamento();
                Console.WriteLine($"[MLService] Dados obtidos do banco: {dadosTreinamento.Count}");

                if (dadosTreinamento.Count < 10) // Aumentei o mínimo
                {
                    Console.WriteLine("[MLService] Dados insuficientes do banco, tentando CSV...");
                    return await TreinarModeloComCSV();
                }

                return TreinarComDados(dadosTreinamento);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MLService] Erro no treinamento: {ex.Message}");
                return false;
            }
        }

        // Novo método para treinar com CSV
        public async Task<bool> TreinarModeloComCSV(string? caminhoCSV = null)
        {
            try
            {
                var dadosTreinamento = caminhoCSV != null 
                    ? LerDadosDoCSV(caminhoCSV)
                    : GerarDadosExemplo(); // Fallback para dados sintéticos

                Console.WriteLine($"[MLService] Dados de treinamento gerados: {dadosTreinamento.Count}");

                if (!dadosTreinamento.Any())
                {
                    return false;
                }

                return TreinarComDados(dadosTreinamento);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MLService] Erro no treinamento com CSV: {ex.Message}");
                return false;
            }
        }

        private bool TreinarComDados(List<DadosTreinamento> dadosTreinamento)
        {
            var dataView = _mlContext.Data.LoadFromEnumerable(dadosTreinamento);

            var pipeline = _mlContext.Transforms.Categorical.OneHotEncoding("LocalizacaoEncoded", "Localizacao")
                .Append(_mlContext.Transforms.Concatenate("Features", 
                    "LocalizacaoEncoded", "QuantidadeEventos", "CapacidadeAbrigo", "OcupacaoAtual", "MesAno"))
                .Append(_mlContext.Transforms.NormalizeMinMax("Features"));

            // Treinar modelo para Alimentos
            var pipelineAlimentos = pipeline
                .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: "AlimentosNecessarios", maximumNumberOfIterations: 100));
            _modeloAlimentos = pipelineAlimentos.Fit(dataView);

            // Treinar modelo para Medicamentos  
            var pipelineMedicamentos = pipeline
                .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: "MedicamentosNecessarios", maximumNumberOfIterations: 100));
            _modeloMedicamentos = pipelineMedicamentos.Fit(dataView);

            // Treinar modelo para Cobertores
            var pipelineCobertores = pipeline
                .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: "CobertoresNecessarios", maximumNumberOfIterations: 100));
            _modeloCobertores = pipelineCobertores.Fit(dataView);

            Console.WriteLine("[MLService] Modelos treinados com sucesso!");
            return true;
        }

        private List<DadosTreinamento> LerDadosDoCSV(string caminhoCSV)
        {
            var dados = new List<DadosTreinamento>();

            try
            {
                var linhas = File.ReadAllLines(caminhoCSV);
                
                // Pular cabeçalho se existir
                var linhasDados = linhas.Skip(1);

                foreach (var linha in linhasDados)
                {
                    var campos = linha.Split(',');
                    
                    if (campos.Length >= 7) // Verificar se tem campos suficientes
                    {
                        dados.Add(new DadosTreinamento
                        {
                            Localizacao = campos[0].Trim(),
                            QuantidadeEventos = int.Parse(campos[1]),
                            CapacidadeAbrigo = int.Parse(campos[2]),
                            OcupacaoAtual = int.Parse(campos[3]),
                            MesAno = float.Parse(campos[4], CultureInfo.InvariantCulture),
                            AlimentosNecessarios = float.Parse(campos[5], CultureInfo.InvariantCulture),
                            MedicamentosNecessarios = float.Parse(campos[6], CultureInfo.InvariantCulture),
                            CobertoresNecessarios = campos.Length > 7 ? float.Parse(campos[7], CultureInfo.InvariantCulture) : 0
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MLService] Erro ao ler CSV: {ex.Message}");
            }

            return dados;
        }

        // Método para gerar dados sintéticos (fallback)
        private List<DadosTreinamento> GerarDadosExemplo()
        {
            var dados = new List<DadosTreinamento>();
            var random = new Random(42);
            var localizacoes = new[] { "São Paulo", "Rio de Janeiro", "Belo Horizonte", "Salvador", "Curitiba" };

            Console.WriteLine("[MLService] Gerando 1000 registros sintéticos para treinamento...");

            for (int i = 0; i < 1000; i++)
            {
                var localizacao = localizacoes[random.Next(localizacoes.Length)];
                var quantidadeEventos = random.Next(1, 20);
                var capacidadeAbrigo = random.Next(50, 300);
                var ocupacaoAtual = random.Next(20, capacidadeAbrigo);
                var mesAno = random.Next(1, 13);

                // Fórmulas realistas baseadas em correlações
                var fatorBase = quantidadeEventos * (ocupacaoAtual / (float)capacidadeAbrigo);
                var fatorSazonalidade = mesAno >= 6 && mesAno <= 8 ? 1.3f : 1.0f; // Inverno = mais necessidades

                var alimentos = fatorBase * 15 * fatorSazonalidade + random.Next(-5, 15);
                var medicamentos = fatorBase * 5 * fatorSazonalidade + random.Next(-2, 8);
                var cobertores = fatorBase * 3 * fatorSazonalidade + random.Next(-1, 5);

                dados.Add(new DadosTreinamento
                {
                    Localizacao = localizacao,
                    QuantidadeEventos = quantidadeEventos,
                    CapacidadeAbrigo = capacidadeAbrigo,
                    OcupacaoAtual = ocupacaoAtual,
                    MesAno = mesAno,
                    AlimentosNecessarios = Math.Max(0, alimentos),
                    MedicamentosNecessarios = Math.Max(0, medicamentos),
                    CobertoresNecessarios = Math.Max(0, cobertores)
                });
            }

            return dados;
        }

        public async Task<PredicaoResultado> Prever(string localizacao, int quantidadeEventos, int capacidadeAbrigo, int ocupacaoAtual)
        {
            // Verificar se os modelos foram treinados
            if (_modeloAlimentos == null || _modeloMedicamentos == null || _modeloCobertores == null)
            {
                Console.WriteLine("[MLService] Modelos não treinados, treinando automaticamente...");
                var treinamentoBemSucedido = await TreinarModelo();
                if (!treinamentoBemSucedido)
                {
                    Console.WriteLine("[MLService] Fallback para estimativas simples");
                    return new PredicaoResultado
                    {
                        AlimentosPrevistos = quantidadeEventos * 2,
                        MedicamentosPrevistos = quantidadeEventos * 1,
                        CobertoresPrevistos = quantidadeEventos * 1
                    };
                }
            }

            var dadosEntrada = new DadosTreinamento
            {
                Localizacao = localizacao,
                QuantidadeEventos = quantidadeEventos,
                CapacidadeAbrigo = capacidadeAbrigo,
                OcupacaoAtual = ocupacaoAtual,
                MesAno = DateTime.Now.Month,
                AlimentosNecessarios = 0,
                MedicamentosNecessarios = 0,
                CobertoresNecessarios = 0
            };

            var engineAlimentos = _mlContext.Model.CreatePredictionEngine<DadosTreinamento, PredicaoML>(_modeloAlimentos!);
            var engineMedicamentos = _mlContext.Model.CreatePredictionEngine<DadosTreinamento, PredicaoML>(_modeloMedicamentos!);
            var engineCobertores = _mlContext.Model.CreatePredictionEngine<DadosTreinamento, PredicaoML>(_modeloCobertores!);

            var predicaoAlimentos = engineAlimentos.Predict(dadosEntrada);
            var predicaoMedicamentos = engineMedicamentos.Predict(dadosEntrada);
            var predicaoCobertores = engineCobertores.Predict(dadosEntrada);

            Console.WriteLine($"[MLService] Predição realizada para {localizacao}");

            return new PredicaoResultado
            {
                AlimentosPrevistos = Math.Max(0, (int)Math.Round(predicaoAlimentos.Score)),
                MedicamentosPrevistos = Math.Max(0, (int)Math.Round(predicaoMedicamentos.Score)),
                CobertoresPrevistos = Math.Max(0, (int)Math.Round(predicaoCobertores.Score))
            };
        }

        public async Task<List<string>> ObterLocalizacoesDisponiveis()
        {
            return await _mlRepository.ObterLocalizacoesAtivas();
        }
    }

    public class PredicaoML
    {
        [ColumnName("Score")]
        public float Score { get; set; }
    }
}