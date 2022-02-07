using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace WixSharp.Common;

[JsonObject]
public sealed class Specification
{
    [JsonRequired, JsonProperty]
    public string Version { get; set; } = string.Empty;

    [JsonRequired, JsonProperty]
    public Guid GUID { get; set; }

    [JsonProperty]
    public string? Manufacturer { get; set; }

    [JsonProperty]
    public string? InstallerName { get; set; }

    [JsonProperty]
    public string? BackgroundImagePath { get; set; }

    [JsonProperty]
    public string? BannerImagePath { get; set; }

    [JsonProperty]
    public bool UseRunningAsAdministrator { get; set; }

    [JsonProperty]
    public List<Locale> Locales { get; set; } = new();

    [JsonProperty]
    public List<Registry> MappingRegistries { get; set; } = new();

    [JsonProperty]
    public List<Item> MappingItems { get; set; } = new();

    [JsonIgnore]
    public IReadOnlyDictionary<string, Item[]> MappingItemDictionary => MappingItems
        .GroupBy(item => item.Destination)
        .ToDictionary(item => item.Key, grouping => grouping.ToArray());
}