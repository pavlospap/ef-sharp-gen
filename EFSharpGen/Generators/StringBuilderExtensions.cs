using System.Text;

namespace EFSharpGen.Generators;

/// <summary>
/// Extensions for the <see cref="StringBuilder"/>.
/// </summary>
public static class StringBuilderExtensions
{
    /// <summary>
    /// Appends a value to a <see cref="StringBuilder"/> together with a new
    /// line at the end.
    /// </summary>
    /// <param name="sb">A <see cref="StringBuilder"/>.</param>
    /// <param name="tabs">The number of tabs to add before the value.</param>
    /// <param name="value">The value to append.</param>
    public static void AppendLine(this StringBuilder sb, int tabs, string value)
    {
        sb.AppendLine(new string('\t', tabs) + value);
    }
}
