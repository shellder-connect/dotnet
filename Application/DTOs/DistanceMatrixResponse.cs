// CRIE ESTE ARQUIVO: DTOs/DistanceMatrixDTOs.cs
// OU ADICIONE no final do seu ProximityService.cs

namespace Project.Models;

    public class DistanceMatrixResponse
    {
        public List<DistanceMatrixResult> Result { get; set; } = new();
        public string Status { get; set; } = string.Empty;
    }

    public class DistanceMatrixResult
    {
        public DistanceMatrixGeometry Geometry { get; set; } = new();
    }

    public class DistanceMatrixGeometry
    {
        public DistanceMatrixLocation Location { get; set; } = new();
    }

    public class DistanceMatrixLocation
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
