using Newtonsoft.Json;

namespace WixSharp.Common;

[JsonObject]
public sealed class Language
{
    [JsonRequired, JsonProperty]
    public string Id { get; set; } = string.Empty;

    [JsonIgnore]
    public string WixId => Id.Replace('-', '_').ToLower();

    [JsonRequired, JsonProperty]
    public string DisplayName { get; set; } = string.Empty;

    [JsonRequired, JsonProperty]
    public string WxlFilePath { get; set; } = string.Empty;
}