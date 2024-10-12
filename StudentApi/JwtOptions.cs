

namespace StudentApi;
public class JwtOptions
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required int Lifetime { get; set; }
    public required string SigningKey { get; set; }
}
