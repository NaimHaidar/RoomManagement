using RoomManagement.Repository.Models;

public class LocationDto
{
    public int Id { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Building { get; set; }
    public int Floor { get; set; }
    public string? MapLink { get; set; }

    public LocationDto() { }

    public LocationDto(Location location)
    {
        Id = location.Id;
        Country = location.Country;
        City = location.City;
        Building = location.Building;
        Floor = location.Floor;
        MapLink = location.MapLink;
    }
}