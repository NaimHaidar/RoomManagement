using RoomManagement.Repository.Models;

public class RoomFeatureDto
{
    public int Id { get; set; }
    public int FeatureId { get; set; }
    public int RoomId { get; set; }

    public RoomFeatureDto() { }

    public RoomFeatureDto(RoomFeature roomFeature)
    {
        Id = roomFeature.Id;
        FeatureId = roomFeature.FeatureId;
        RoomId = roomFeature.RoomId;
    }
}