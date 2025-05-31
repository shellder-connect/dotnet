
namespace Project.Models;
public class DadosTreinamento
{
    public string Localizacao { get; set; } = string.Empty;
    public float QuantidadeEventos { get; set; }
    public float CapacidadeAbrigo { get; set; }
    public float OcupacaoAtual { get; set; }
    public float MesAno { get; set; }
    
    // O que o modelo vai aprender a prever
    public float AlimentosNecessarios { get; set; }
    public float MedicamentosNecessarios { get; set; }
    public float CobertoresNecessarios { get; set; }
}