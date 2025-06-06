// DTO para resposta da API OpenCage

namespace Project.Models;
public class OpenCageResponse
{
    public List<OpenCageResult> Results { get; set; } = new();
}