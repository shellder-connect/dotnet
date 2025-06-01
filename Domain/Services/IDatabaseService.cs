
namespace Project.Services
{
    public interface IDatabaseService
    {
        Task<(bool success, string errorMessage)> ImportarJson(string collection, string jsonContent);
        Task<(bool success, string content, string errorMessage)> ExportarJson(string collection);
        Task<(bool success, List<object> dados, string errorMessage)> ConsultarDocumentos(string collection, int limit = 10);
        Task<(bool success, Dictionary<string, object> documento, string errorMessage)> BuscarDocumentoPorId(string collection, string id);
        Task<(bool success, string errorMessage)> AtualizarDocumento(string collection, string id, Dictionary<string, object> campos);
        Task<(bool success, string errorMessage)> ExcluirDocumento(string collection, string id);
        Task<(bool success, string errorMessage)> LimparColecao(string collection);
    }
}
