public class PredicaoResponseDto
{
    public string Localizacao { get; set; } = string.Empty;
    public DateTime DataPrevisao { get; set; }
    public int AlimentosPrevistos { get; set; }
    public int MedicamentosPrevistos { get; set; }
    public int CobertoresPrevistos { get; set; }
}