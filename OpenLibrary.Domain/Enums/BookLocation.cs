using System.ComponentModel;

namespace OpenLibrary.Domain;

public enum BookLocation
{
    [Description("Server")]
    Server = 1,

    [Description("Cache")]
    Cache = 2
}

public static class BookLocationExtensions
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        return attribute == null ? value.ToString() : attribute.Description;
    }
}