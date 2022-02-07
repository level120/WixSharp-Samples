using System.IO;
using Newtonsoft.Json;

namespace WixSharp.Common.Serialization;

public sealed class JsonSerialization
{
    public static string Serialize<T>(
        T model, Formatting formatting, params JsonConverter[] converters)
    {
        return JsonConvert.SerializeObject(model, formatting, converters);
    }

    public static bool TrySerialize<T>(
        T model, Formatting formatting, out string? serialized, params JsonConverter[] converters)
    {
        try
        {
            serialized = Serialize(model, formatting, converters);
            return true;
        }
        catch
        {
            serialized = default;
            return false;
        }
    }

    public static T? DeserializeFrom<T>(string filepath, params JsonConverter[] converters)
    {
        return JsonConvert.DeserializeObject<T>(File.ReadAllText(filepath), converters);
    }

    public static bool TryDeserializeFrom<T>(
        string filepath, out T? deserialized, params JsonConverter[] converters)
    {
        try
        {
            deserialized = DeserializeFrom<T>(filepath, converters);
            return true;
        }
        catch
        {
            deserialized = default;
            return false;
        }
    }

    public static T? Deserialize<T>(string serialized, params JsonConverter[] converters)
    {
        return JsonConvert.DeserializeObject<T>(serialized, converters);
    }

    public static bool TryDeserialize<T>(
        string serialized, out T? deserialized, params JsonConverter[] converters)
    {
        try
        {
            deserialized = Deserialize<T>(serialized, converters);
            return true;
        }
        catch
        {
            deserialized = default;
            return false;
        }
    }
}