using Newtonsoft.Json;

namespace WixSharp.Common;

[JsonObject]
public sealed class Locale
{
    [JsonRequired, JsonProperty]
    public string Language { get; set; } = string.Empty;

    [JsonRequired, JsonProperty]
    public string LocalizationFile { get; set; } = string.Empty;

    [JsonRequired, JsonProperty]
    public string LicenseFile { get; set; } = string.Empty;
}