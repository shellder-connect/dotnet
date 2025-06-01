using Moq;
using Project.Application.Services;
using Project.Infrastructure.Interfaces;
using Project.Models;
using Xunit;
using Xunit.Abstractions;

namespace Project.Tests.UnitTests.Services
{
    public class LocalidadeServiceTests
    {
        private readonly ITestOutputHelper _output;

        public LocalidadeServiceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /*************************************************************************************************

         Consultar todos os Localidades

         Teste será realizado para consultar a lista completa de localidades com dados.

        **************************************************************************************************/

        [Fact]
        public async Task ConsultarTodos_DeveRetornarListaDeLocalidades()
        {
            var mockRepo = new Mock<ILocalidadeRepository>();
            var localidadesEsperadas = new List<Localidade>
            {
                new Localidade 
                { 
                    IdUsuario = "683a34fe6333f0107e656ff3",
                    Nome = "Localidade Centro",
                    IdEndereco = "683a3c9b46214868cac01d6b",
                    CEPUsuario = "01234567",
                    IdAbrigo = "683a44470f3c2967f9c6ebb3",
                    CEPAbrigo = "05763290",
                    Distancia = "2.5 km"
                },
                new Localidade 
                { 
                    IdUsuario = "683a34fe6333f0107e656ff4",
                    Nome = "Localidade Vila Madalena",
                    IdEndereco = "683a3c9b46214868cac01d6c",
                    CEPUsuario = "05433000",
                    IdAbrigo = "683a44470f3c2967f9c6ebb4",
                    CEPAbrigo = "05433100",
                    Distancia = "1.8 km"
                }
            };
            mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(localidadesEsperadas);
            var service = new LocalidadeService(mockRepo.Object);

            _output.WriteLine("********************( Chamando o serviço Consultar todas as localidades )********************\n");

            var resultado = await service.ConsultarTodos();

            _output.WriteLine("Localidades retornadas:");
            foreach (var localidade in resultado)
            {
                _output.WriteLine($"- IdUsuario: {localidade.IdUsuario}, Nome: {localidade.Nome}, CEPUsuario: {localidade.CEPUsuario}, CEPAbrigo: {localidade.CEPAbrigo}, Distancia: {localidade.Distancia}");
            }

            Assert.Equal(2, resultado.Count);
            Assert.Contains(resultado, e => e.CEPAbrigo == "05763290");
            Assert.Contains(resultado, e => e.Nome == "Localidade Centro");
            Assert.Contains(resultado, e => e.CEPUsuario == "05433000");

            _output.WriteLine("********************( Teste de consulta de localidade concluído )********************\n");
        }

    }
}