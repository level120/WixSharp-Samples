using System.Collections.Generic;
using Newtonsoft.Json;

namespace WixSharp.Common;

[JsonObject]
public sealed class Item
{
    [JsonRequired, JsonProperty]
    public string Source { get; set; } = string.Empty;

    [JsonRequired, JsonProperty]
    public string? Destination { get; set; }

    [JsonProperty]
    public string? ExcludeType { get; set; }

    [JsonProperty]
    public List<Item> MappingItems { get; set; } = new();
}