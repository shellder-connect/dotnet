using System.Text.Json;
using Project.Infrastructure.Interfaces;
using Project.Models;

namespace Project.Application.Services
{
    public class ProximityService : IProximityService
    {
        private readonly IProximityRepository _proximityRepository;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "lze01Ej8Jlh1m6cSIDDbJDCcBl1WZqfwTPlO8NmEK8Cq0q0HOSxs9AFmnI3XHveO";
        private readonly string _baseUrl = "https://api.distancematrix.ai/maps/api/geocode/json";


        public ProximityService(IProximityRepository proximityRepository, HttpClient httpClient)
        {
            _proximityRepository = proximityRepository;
            _httpClient = httpClient;
        }

        #region Geocodificação (OpenCage API)

        /*
        public async Task<(double? lat, double? lng)> ObterCoordenadasAsync(string endereco)
        {
            try
            {
                var url = $"{_baseUrl}?address={Uri.EscapeDataString(endereco)}&key={_apiKey}&language=pt";

                // 🔍 LOG 1: URL que está sendo chamada
                Console.WriteLine($"🌐 URL: {url}");

                var response = await _httpClient.GetAsync(url);

                // 🔍 LOG 2: Status da resposta HTTP
                Console.WriteLine($"📡 Status HTTP: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    // 🔍 LOG 3: Resposta completa da API
                    Console.WriteLine($"📄 Resposta API: {jsonString}");


                    var data = JsonSerializer.Deserialize<OpenCageResponse>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });


                    if (data?.Results?.Any() == true)
                    {
                        return (data.Results[0].Geometry.Lat, data.Results[0].Geometry.Lng);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar coordenadas para {endereco}: {ex.Message}");
            }

            return (null, null);
        }
        */

        public async Task<(double? lat, double? lng)> ObterCoordenadasAsync(string endereco)
        {
            try
            {
                var url = $"{_baseUrl}?address={Uri.EscapeDataString(endereco)}&key={_apiKey}&language=pt";

                // 🔍 LOG 1: URL que está sendo chamada
                Console.WriteLine($"🌐 URL: {url}");

                var response = await _httpClient.GetAsync(url);

                // 🔍 LOG 2: Status da resposta HTTP
                Console.WriteLine($"📡 Status HTTP: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    // 🔍 LOG 3: Resposta completa da API
                    Console.WriteLine($"📄 Resposta API: {jsonString}");

                    // ✅ DTO CORRETO para DistanceMatrix.ai
                    var data = JsonSerializer.Deserialize<DistanceMatrixResponse>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    Console.WriteLine($"🔍 Data: Status={data?.Status}, Result.Count={data?.Result?.Count}");
                    
                    // ✅ VERIFICAÇÃO CORRETA para DistanceMatrix.ai
                    if (data?.Result?.Any() == true && data.Status == "OK")
                    {
                        var location = data.Result[0].Geometry.Location;
                        Console.WriteLine($"✅ Coordenadas encontradas: Lat={location.Lat}, Lng={location.Lng}");
                        return (location.Lat, location.Lng);
                    }
                    else
                    {
                        Console.WriteLine($"❌ API retornou: Status={data?.Status}, ResultCount={data?.Result?.Count ?? 0}");
                    }
                }
                else
                {
                    Console.WriteLine($"❌ Erro HTTP: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao buscar coordenadas para {endereco}: {ex.Message}");
            }

            return (null, null);
        }
        #endregion

        #region Cálculo de Distância

        public double CalcularDistancia(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Raio da Terra em km

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c;

            return distance;
        }

        private double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        #endregion

        #region Gerenciamento de Coordenadas

        public async Task<Coordenada?> BuscarOuCriarCoordenadaUsuarioAsync(string idUsuario)
        {
            // Primeiro, verifica se já existe coordenada para este usuário
            var coordenadaExistente = await _proximityRepository.BuscarCoordenadaPorReferencia("Usuario", idUsuario);
            if (coordenadaExistente != null)
            {
                return coordenadaExistente;
            }

            // Buscar dados do usuário e endereço
            var usuario = await _proximityRepository.BuscarUsuarioPorId(idUsuario);
            if (usuario?.IdEndereco == null) return null;

            var endereco = await _proximityRepository.BuscarEnderecoPorId(usuario.IdEndereco);
            if (endereco == null) return null;

            // Montar endereço completo
            var enderecoCompleto = $"{endereco.Rua}, {endereco.Bairro}, {endereco.Cidade}, {endereco.Estado}, Brasil";

            // Obter coordenadas
            var (lat, lng) = await ObterCoordenadasAsync(enderecoCompleto);

            // Criar nova coordenada
            var novaCoordenada = new Coordenada
            {
                Tipo = "Usuario",
                ReferenciaId = idUsuario,
                EnderecoId = endereco.Id,
                Latitude = lat,
                Longitude = lng,
                EnderecoCompleto = enderecoCompleto,
                Status = lat.HasValue && lng.HasValue ? "Ativo" : "Erro"
            };

            return await _proximityRepository.CriarCoordenada(novaCoordenada);
        }

        public async Task<Coordenada?> BuscarOuCriarCoordenadaAbrigoAsync(string idAbrigo)
        {
            // Primeiro, verifica se já existe coordenada para este abrigo
            var coordenadaExistente = await _proximityRepository.BuscarCoordenadaPorReferencia("Abrigo", idAbrigo);
            if (coordenadaExistente != null)
            {
                return coordenadaExistente;
            }

            // Buscar dados do abrigo e endereço
            var abrigo = await _proximityRepository.BuscarAbrigoPorId(idAbrigo);
            if (abrigo == null) return null;

            var enderecoAbrigo = await _proximityRepository.BuscarEnderecoAbrigoPorAbrigo(idAbrigo);
            if (enderecoAbrigo == null) return null;

            // Montar endereço completo
            var enderecoCompleto = $"{enderecoAbrigo.Rua}, {enderecoAbrigo.Bairro}, {enderecoAbrigo.Cidade}, {enderecoAbrigo.Estado}, Brasil";

            // Obter coordenadas
            var (lat, lng) = await ObterCoordenadasAsync(enderecoCompleto);

            // Criar nova coordenada
            var novaCoordenada = new Coordenada
            {
                Tipo = "Abrigo",
                ReferenciaId = idAbrigo,
                EnderecoId = enderecoAbrigo.Id,
                Latitude = lat,
                Longitude = lng,
                EnderecoCompleto = enderecoCompleto,
                Status = lat.HasValue && lng.HasValue ? "Ativo" : "Erro"
            };

            return await _proximityRepository.CriarCoordenada(novaCoordenada);
        }

        #endregion

        #region Processamento de Dados

        public async Task<List<UsuarioComEnderecoDto>> ProcessarUsuariosComEnderecosAsync()
        {
            var usuarios = await _proximityRepository.BuscarTodosUsuarios();
            var resultado = new List<UsuarioComEnderecoDto>();

            foreach (var usuario in usuarios)
            {
                if (usuario.IdEndereco == null) continue;

                var coordenada = await BuscarOuCriarCoordenadaUsuarioAsync(usuario.Id!);
                var endereco = await _proximityRepository.BuscarEnderecoPorId(usuario.IdEndereco);

                resultado.Add(new UsuarioComEnderecoDto
                {
                    IdUsuario = usuario.Id,
                    NomeUsuario = usuario.Nome,
                    Email = usuario.Email,
                    Telefone = usuario.Telefone,
                    CEP = endereco?.CEP,
                    EnderecoCompleto = coordenada?.EnderecoCompleto,
                    Latitude = coordenada?.Latitude,
                    Longitude = coordenada?.Longitude,
                    Status = coordenada?.Status ?? "Sem Endereço"
                });
            }

            return resultado;
        }

        public async Task<List<AbrigoComEnderecoDto>> ProcessarAbrigosComEnderecosAsync()
        {
            var abrigos = await _proximityRepository.BuscarTodosAbrigos();
            var resultado = new List<AbrigoComEnderecoDto>();

            foreach (var abrigo in abrigos)
            {
                var coordenada = await BuscarOuCriarCoordenadaAbrigoAsync(abrigo.Id!);
                var endereco = await _proximityRepository.BuscarEnderecoAbrigoPorAbrigo(abrigo.Id!);

                resultado.Add(new AbrigoComEnderecoDto
                {
                    IdAbrigo = abrigo.Id,
                    CEP = endereco?.CEP,
                    EnderecoCompleto = coordenada?.EnderecoCompleto,
                    Latitude = coordenada?.Latitude,
                    Longitude = coordenada?.Longitude,
                    CapacidadeTotal = abrigo.CapacidadeTotal,
                    OcupacaoAtual = abrigo.OcupacaoAtual,
                    Descricao = abrigo.Descricao,
                    Status = coordenada?.Status ?? "Sem Endereço"
                });
            }

            return resultado;
        }

        #endregion

        #region Análise de Proximidade

        public async Task<bool> CalcularTodasProximidadesAsync()
        {
            try
            {
                // Limpar análises antigas
                await _proximityRepository.LimparAnaliseProximidade();

                // Buscar todas as coordenadas válidas
                var coordenadasUsuarios = await _proximityRepository.BuscarCoordenadasPorTipo("Usuario");
                var coordenadasAbrigos = await _proximityRepository.BuscarCoordenadasPorTipo("Abrigo");

                var analises = new List<AnaliseProximidade>();

                // Para cada usuário, calcular distância para todos os abrigos
                foreach (var coordUsuario in coordenadasUsuarios)
                {
                    var proximidadesUsuario = new List<AnaliseProximidade>();

                    foreach (var coordAbrigo in coordenadasAbrigos)
                    {
                        var distancia = CalcularDistancia(
                            coordUsuario.Latitude!.Value, coordUsuario.Longitude!.Value,
                            coordAbrigo.Latitude!.Value, coordAbrigo.Longitude!.Value
                        );

                        proximidadesUsuario.Add(new AnaliseProximidade
                        {
                            IdUsuario = coordUsuario.ReferenciaId,
                            IdAbrigo = coordAbrigo.ReferenciaId,
                            DistanciaKm = Math.Round(distancia, 2),
                            CoordenadaUsuarioId = coordUsuario.Id,
                            CoordenadaAbrigoId = coordAbrigo.Id
                        });
                    }

                    // Ordenar por distância e atribuir ranking
                    var proximidadesOrdenadas = proximidadesUsuario
                        .OrderBy(p => p.DistanciaKm)
                        .Select((p, index) => { p.Ranking = index + 1; return p; })
                        .ToList();

                    analises.AddRange(proximidadesOrdenadas);
                }

                // Inserir todas as análises
                return await _proximityRepository.CriarAnaliseProximidade(analises);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao calcular proximidades: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ProximidadeDto>> BuscarProximidadesAsync(string? idUsuario = null)
        {
            var analises = await _proximityRepository.BuscarAnaliseProximidade(idUsuario);
            var resultado = new List<ProximidadeDto>();

            foreach (var analise in analises)
            {
                // Buscar dados do usuário
                var usuario = await _proximityRepository.BuscarUsuarioPorId(analise.IdUsuario);
                var coordUsuario = await _proximityRepository.BuscarCoordenadaPorReferencia("Usuario", analise.IdUsuario);
                
                // Buscar dados do abrigo
                var abrigo = await _proximityRepository.BuscarAbrigoPorId(analise.IdAbrigo);
                var coordAbrigo = await _proximityRepository.BuscarCoordenadaPorReferencia("Abrigo", analise.IdAbrigo);

                resultado.Add(new ProximidadeDto
                {
                    IdUsuario = analise.IdUsuario,
                    NomeUsuario = usuario?.Nome,
                    EmailUsuario = usuario?.Email,
                    EnderecoUsuario = coordUsuario?.EnderecoCompleto,
                    IdAbrigo = analise.IdAbrigo,
                    EnderecoAbrigo = coordAbrigo?.EnderecoCompleto,
                    CapacidadeAbrigo = abrigo?.CapacidadeTotal,
                    OcupacaoAtual = abrigo?.OcupacaoAtual,
                    DistanciaKm = analise.DistanciaKm,
                    Ranking = analise.Ranking,
                    DataCalculo = analise.DataCalculo
                });
            }

            return resultado;
        }

        public async Task<List<ProximidadeDto>> BuscarTop3ProximidadesPorUsuarioAsync(string idUsuario)
        {
            var analises = await _proximityRepository.BuscarTop3ProximidadePorUsuario(idUsuario);
            var resultado = new List<ProximidadeDto>();

            foreach (var analise in analises)
            {
                var usuario = await _proximityRepository.BuscarUsuarioPorId(analise.IdUsuario);
                var coordUsuario = await _proximityRepository.BuscarCoordenadaPorReferencia("Usuario", analise.IdUsuario);
                var abrigo = await _proximityRepository.BuscarAbrigoPorId(analise.IdAbrigo);
                var coordAbrigo = await _proximityRepository.BuscarCoordenadaPorReferencia("Abrigo", analise.IdAbrigo);

                resultado.Add(new ProximidadeDto
                {
                    IdUsuario = analise.IdUsuario,
                    NomeUsuario = usuario?.Nome,
                    EmailUsuario = usuario?.Email,
                    EnderecoUsuario = coordUsuario?.EnderecoCompleto,
                    IdAbrigo = analise.IdAbrigo,
                    EnderecoAbrigo = coordAbrigo?.EnderecoCompleto,
                    CapacidadeAbrigo = abrigo?.CapacidadeTotal,
                    OcupacaoAtual = abrigo?.OcupacaoAtual,
                    DistanciaKm = analise.DistanciaKm,
                    Ranking = analise.Ranking,
                    DataCalculo = analise.DataCalculo
                });
            }

            return resultado;
        }

        #endregion

        #region Dashboard

        public async Task<DashboardDto> GerarDashboardAsync()
        {
            var usuarios = await ProcessarUsuariosComEnderecosAsync();
            var abrigos = await ProcessarAbrigosComEnderecosAsync();
            var proximidades = await BuscarProximidadesAsync();

            var estatisticas = new EstatisticasDto
            {
                TotalUsuarios = usuarios.Count,
                TotalAbrigos = abrigos.Count,
                UsuariosComCoordenadas = usuarios.Count(u => u.Latitude.HasValue),
                AbrigosComCoordenadas = abrigos.Count(a => a.Latitude.HasValue),
                TotalCalculosProximidade = proximidades.Count,
                DistanciaMedia = proximidades.Any() ? Math.Round(proximidades.Average(p => p.DistanciaKm), 2) : null,
                DistanciaMinima = proximidades.Any() ? proximidades.Min(p => p.DistanciaKm) : null,
                DistanciaMaxima = proximidades.Any() ? proximidades.Max(p => p.DistanciaKm) : null
            };

            return new DashboardDto
            {
                Usuarios = usuarios,
                Abrigos = abrigos,
                Proximidade = proximidades,
                Estatisticas = estatisticas
            };
        }

        #endregion
    }
}