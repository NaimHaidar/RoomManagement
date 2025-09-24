using RoomManagement.Repository.Models;

public class FeatureDto
{
    public int Id { get; set; }
    public string? Feature1 { get; set; }

    public FeatureDto() { }

    public FeatureDto(Feature feature)
    {
        Id = feature.Id;
        Feature1 = feature.Feature1;
    }
}