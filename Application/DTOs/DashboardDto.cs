

using Project.Models;

public class DashboardDto
{
    public List<UsuarioComEnderecoDto> Usuarios { get; set; } = new();
    public List<AbrigoComEnderecoDto> Abrigos { get; set; } = new();
    public List<ProximidadeDto> Proximidade { get; set; } = new();
    public EstatisticasDto Estatisticas { get; set; } = new();
}