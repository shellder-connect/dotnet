using Newtonsoft.Json;
using Project.Infrastructure.Interfaces;
using Project.Models;

namespace Project.Application.Services
{
    public class CepService : ICepService
    {
        private readonly HttpClient _httpClient;

        public CepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EnderecoDTO?> ConsultarCepAsync(string cep)
        {
            var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<EnderecoViaCepResponse>(content);

            if (data == null || data.Cep == null)
                return null;

            return new EnderecoDTO
            {
                CEP = data.Cep,
                Estado = data.Uf,
                Cidade = data.Localidade,
                Bairro = data.Bairro,
                Rua = data.Logradouro
            };
        }

        private class EnderecoViaCepResponse
        {
            public string? Cep { get; set; }
            public string? Logradouro { get; set; }
            public string? Bairro { get; set; }
            public string? Localidade { get; set; }
            public string? Uf { get; set; }
        }
    }
}
