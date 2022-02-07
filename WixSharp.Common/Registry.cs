using Newtonsoft.Json;

namespace WixSharp.Common;

[JsonObject]
public sealed class Registry
{
    [JsonRequired, JsonProperty]
    public string Root { get; set; } = string.Empty;

    [JsonRequired, JsonProperty]
    public string Key { get; set; } = string.Empty;

    [JsonRequired, JsonProperty]
    public string Type { get; set; } = string.Empty;

    [JsonRequired, JsonProperty]
    public string Value { get; set; } = string.Empty;
}