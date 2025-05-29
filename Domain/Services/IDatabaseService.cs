
namespace Project.Services
{
    public interface IDatabaseService
    {
        Task<(bool success, string errorMessage)> ImportarJson(string collection, string jsonContent);
        /*Task<string> ExportarDataset(string collection);
        Task<IEnumerable<Dictionary<string, object>>> ConsultarColecao(string collection);
        Task<Dictionary<string, object>?> BuscarDocumento(string collection, string id);
        Task<bool> AtualizarDocumento(string collection, string id, Dictionary<string, object> dados);
        Task<bool> ExcluirDocumento(string collection, string id);*/
        object GetCollection<T>(string collection);
    }
}
