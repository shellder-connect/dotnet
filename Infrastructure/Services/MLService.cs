using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using Project.Infrastructure.Interfaces;
using Project.Models;

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

        public async Task<bool> TreinarModelo()
        {
            try
            {
                // Tentar carregar CSV da raiz primeiro
                var baseDir = AppContext.BaseDirectory;
                var projectRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));
                var csvPath = Path.Combine(projectRoot, "ml_training_data.csv");

                List<DadosTreinamento> dadosTreinamento;

                if (File.Exists(csvPath))
                {
                    Console.WriteLine($"[MLService] Carregando dados do CSV: {csvPath}");
                    dadosTreinamento = CarregarDadosDoCSV(csvPath);
                    Console.WriteLine($"[MLService] {dadosTreinamento.Count} registros carregados do CSV");
                }
                else
                {
                    Console.WriteLine("[MLService] CSV não encontrado, tentando banco de dados...");
                    dadosTreinamento = await _mlRepository.ObterDadosParaTreinamento();
                    Console.WriteLine($"[MLService] {dadosTreinamento.Count} registros obtidos do banco");

                    if (dadosTreinamento.Count < 10)
                    {
                        Console.WriteLine("[MLService] Dados insuficientes, gerando dados sintéticos...");
                        dadosTreinamento = GerarDadosSinteticos();
                        Console.WriteLine($"[MLService] {dadosTreinamento.Count} registros sintéticos gerados");
                    }
                }

                if (!dadosTreinamento.Any())
                {
                    Console.WriteLine("[MLService] Nenhum dado disponível para treinamento");
                    return false;
                }

                return TreinarModelos(dadosTreinamento);
                //return Task.FromResult(TreinarModelos(dadosTreinamento));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MLService] Erro no treinamento: {ex.Message}");
                return false;
            }
        }

        public Task<bool> TreinarModeloComCSV(string? caminhoCSV = null)
        {
            try
            {
                List<DadosTreinamento> dadosTreinamento;

                if (!string.IsNullOrEmpty(caminhoCSV) && File.Exists(caminhoCSV))
                {
                    Console.WriteLine($"[MLService] Carregando CSV customizado: {caminhoCSV}");
                    dadosTreinamento = CarregarDadosDoCSV(caminhoCSV);
                }
                else
                {
                    Console.WriteLine("[MLService] Gerando dados sintéticos para treinamento");
                    dadosTreinamento = GerarDadosSinteticos();
                }

                Console.WriteLine($"[MLService] {dadosTreinamento.Count} registros carregados");

                if (!dadosTreinamento.Any())
                {
                    //return false;
                    return Task.FromResult(false);
                }

                //return TreinarModelos(dadosTreinamento);
                return Task.FromResult(TreinarModelos(dadosTreinamento));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MLService] Erro no treinamento com CSV: {ex.Message}");
                //return false;
                return Task.FromResult(false);
            }
        }

        private List<DadosTreinamento> CarregarDadosDoCSV(string caminhoCSV)
        {
            var dados = new List<DadosTreinamento>();

            try
            {
                // Usar ML.NET para carregar CSV (similar ao SentimentAnalysis)
                var dataView = _mlContext.Data.LoadFromTextFile<MLTrainingInput>(
                    caminhoCSV,
                    separatorChar: ',',
                    hasHeader: true);

                // Converter para nossa estrutura
                var inputData = _mlContext.Data.CreateEnumerable<MLTrainingInput>(dataView, reuseRowObject: false);

                foreach (var input in inputData)
                {
                    dados.Add(new DadosTreinamento
                    {
                        Localizacao = input.Localizacao ?? "Desconhecido",
                        QuantidadeEventos = input.QuantidadeEventos,
                        CapacidadeAbrigo = input.CapacidadeAbrigo,
                        OcupacaoAtual = input.OcupacaoAtual,
                        MesAno = input.MesAno,
                        AlimentosNecessarios = input.AlimentosNecessarios,
                        MedicamentosNecessarios = input.MedicamentosNecessarios,
                        CobertoresNecessarios = input.CobertoresNecessarios
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MLService] Erro ao carregar CSV: {ex.Message}");
            }

            return dados;
        }

        private bool TreinarModelos(List<DadosTreinamento> dadosTreinamento)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"[MLService] Erro ao treinar modelos: {ex.Message}");
                return false;
            }
        }

        private List<DadosTreinamento> GerarDadosSinteticos()
        {
            var dados = new List<DadosTreinamento>();
            var random = new Random(42);
            var localizacoes = new[] { "São Paulo", "Rio de Janeiro", "Belo Horizonte", "Salvador", "Curitiba" };

            for (int i = 0; i < 1000; i++)
            {
                var localizacao = localizacoes[random.Next(localizacoes.Length)];
                var quantidadeEventos = random.Next(1, 20);
                var capacidadeAbrigo = random.Next(50, 300);
                var ocupacaoAtual = random.Next(20, capacidadeAbrigo);
                var mesAno = random.Next(1, 13);

                // Fórmulas realistas com correlações
                var fatorBase = quantidadeEventos * (ocupacaoAtual / (float)capacidadeAbrigo);
                var fatorSazonalidade = mesAno >= 6 && mesAno <= 8 ? 1.3f : 1.0f; // Inverno

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

    // Classe para carregar dados do CSV (similar ao FeedbackInput)
    public class MLTrainingInput
    {
        [LoadColumn(0)]
        public string? Localizacao { get; set; }

        [LoadColumn(1)]
        public int QuantidadeEventos { get; set; }

        [LoadColumn(2)]
        public int CapacidadeAbrigo { get; set; }

        [LoadColumn(3)]
        public int OcupacaoAtual { get; set; }

        [LoadColumn(4)]
        public float MesAno { get; set; }

        [LoadColumn(5)]
        public float AlimentosNecessarios { get; set; }

        [LoadColumn(6)]
        public float MedicamentosNecessarios { get; set; }

        [LoadColumn(7)]
        public float CobertoresNecessarios { get; set; }
    }

    public class PredicaoML
    {
        [ColumnName("Score")]
        public float Score { get; set; }
    }
}