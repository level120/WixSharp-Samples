using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WixSharp.Common;

[JsonObject]
public sealed class Specification
{
    [JsonRequired, JsonProperty]
    public string Version { get; set; } = string.Empty;

    [JsonRequired, JsonProperty]
    public Guid GUID { get; set; }

    [JsonRequired, JsonProperty]
    public string InstallerName { get; set; } = string.Empty;

    [JsonProperty]
    public string? Manufacturer { get; set; }

    [JsonProperty]
    public string? ServiceName { get; set; }

    [JsonProperty]
    public bool UseService { get; set; }

    /// <example>../doc/license.rtf</example>
    [JsonProperty]
    public string LicenseFilePath { get; set; } = string.Empty;

    /// <example>../icon/favicon.ico</example>
    [JsonProperty]
    public string? IconPath { get; set; }

    /// <example>../img/background.png</example>
    [JsonProperty]
    public string? BackgroundImagePath { get; set; }

    /// <example>../img/banner.png</example>
    [JsonProperty]
    public string? BannerImagePath { get; set; }

    [JsonProperty]
    public bool UseRunningAsAdministrator { get; set; }

    [JsonProperty]
    public List<Language> SupportLanguages { get; set; } = new();

    [JsonProperty]
    public List<Registry> MappingRegistries { get; set; } = new();

    [JsonProperty]
    public List<Item> MappingItems { get; set; } = new();
}