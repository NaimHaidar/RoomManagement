public class NewFeatureDto
{
    public string? Feature1 { get; set; }

    public NewFeatureDto() { }

    public NewFeatureDto(string? feature1)
    {
        Feature1 = feature1;
    }
}