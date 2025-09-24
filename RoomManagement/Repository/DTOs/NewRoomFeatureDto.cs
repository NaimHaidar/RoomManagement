public class NewRoomFeatureDto
{
    public int FeatureId { get; set; }
    public int RoomId { get; set; }

    public NewRoomFeatureDto() { }

    public NewRoomFeatureDto(int featureId, int roomId)
    {
        FeatureId = featureId;
        RoomId = roomId;
    }
}