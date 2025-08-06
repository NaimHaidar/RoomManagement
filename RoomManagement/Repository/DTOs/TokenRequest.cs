using System.ComponentModel.DataAnnotations;

public class TokenRequest
{
    [Required]
    public string AccessToken { get; set; } = null!;

    [Required]
    public string RefreshToken { get; set; } = null!;
}