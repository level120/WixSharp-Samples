using Newtonsoft.Json;

namespace WixSharp.Common;

[JsonObject]
public sealed class Item
{
    [JsonRequired, JsonProperty]
    public string Source { get; set; } = string.Empty;

    [JsonRequired, JsonProperty]
    public string Destination { get; set; } = string.Empty;
}