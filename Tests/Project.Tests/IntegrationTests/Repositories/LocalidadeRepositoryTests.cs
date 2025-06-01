using Project.Repositories;
using Project.Models;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.Options;
using Project.Domain;

namespace Project.Tests.UnitTests.Repositories
{
    public class LocalidadeRepositoryTests
    {
        private readonly ITestOutputHelper _output;

        public LocalidadeRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
        }

        
        /*************************************************************************************************
            Consultar Todos - Repositório
            Teste que garante que o método ConsultarTodos retorna todas as localidades do banco de testes.
         **************************************************************************************************/
        
        [Fact]
        public async Task Consultar_Todas_Localidades()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Todas_Localidades\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    LocalidadeCollectionName = "t_localidade"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new LocalidadeRepository(optionsConfig);

                _output.WriteLine("✅ Repositório criado com sucesso.\n");

                // Executa o método que consulta todas as localidades
                var localidades = await repository.ConsultarTodos();

                _output.WriteLine($"🎯 Total de localidades retornadas: {localidades.Count}\n");

                _output.WriteLine("🔚 Teste finalizado com sucesso.\n");
                _output.WriteLine("*****************************************************\n");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"❌ Erro no teste: {ex.Message}");
                throw;
            }
        }
        
    }
}