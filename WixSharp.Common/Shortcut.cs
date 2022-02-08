using System.Collections.Generic;
using Newtonsoft.Json;

namespace WixSharp.Common;

[JsonObject]
public sealed class Shortcut
{
    /// <summary>
    /// It is not path, only filename.
    /// </summary>
    [JsonRequired, JsonProperty]
    public string FileName { get; set; } = string.Empty;

    [JsonRequired, JsonProperty]
    public List<ShortcutItem> Items { get; set; } = new();
}

public sealed class ShortcutItem
{
    /// <summary>
    /// It is not path, only filename.
    /// </summary>
    [JsonRequired, JsonProperty]
    public string ShortcutName { get; set; } = string.Empty;

    [JsonRequired, JsonProperty]
    public string ShortcutPath { get; set; } = string.Empty;
}