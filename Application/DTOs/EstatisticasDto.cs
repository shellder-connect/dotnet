
namespace Project.Models;
public class EstatisticasDto
{
    public int TotalUsuarios { get; set; }
    public int TotalAbrigos { get; set; }
    public int UsuariosComCoordenadas { get; set; }
    public int AbrigosComCoordenadas { get; set; }
    public int TotalCalculosProximidade { get; set; }
    public double? DistanciaMedia { get; set; }
    public double? DistanciaMinima { get; set; }
    public double? DistanciaMaxima { get; set; }
}