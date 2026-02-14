using System.Text.Json;

namespace Avtobus1.Infrastructure.Extensions;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public override string ToString() => JsonSerializer.Serialize(this);
}
