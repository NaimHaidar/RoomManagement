public class NewLocationDto
{
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Building { get; set; }
    public int Floor { get; set; }
    public string? MapLink { get; set; }

    public NewLocationDto() { }

    public NewLocationDto(string? country, string? city, string? building, int floor, string? mapLink)
    {
        Country = country;
        City = city;
        Building = building;
        Floor = floor;
        MapLink = mapLink;
    }
}