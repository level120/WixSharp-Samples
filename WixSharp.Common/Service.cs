using Newtonsoft.Json;

namespace WixSharp.Common;

[JsonObject]
public sealed class Service
{
    [JsonRequired, JsonProperty]
    public string ServiceName { get; set; } = string.Empty;

    [JsonRequired, JsonProperty]
    public string ServiceFile { get; set; } = string.Empty;

    [JsonProperty]
    public bool IsInstalling { get; set; }

    [JsonProperty]
    public string? ServiceArguments { get; set; }
}