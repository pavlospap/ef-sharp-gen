using System.Text;

namespace EFSharpGen.Generators.Configurations;

/// <summary>
/// An implementation to generate the code for a composite key configuration.
/// </summary>
public class ConfigurationCompositeKeyCodeGenerator :
    IConfigurationCompositeKeyCodeGenerator
{
    /// <summary>
    /// Generates the code for a composite key lambda configuration.
    /// </summary>
    /// <param name="properties">The properties that compose the key.</param>
    /// <returns>The code for a composite key lambda configuration.</returns>
    public string BuildCompositeKeyLambda(IEnumerable<string> properties)
    {
        var sb = new StringBuilder();

        sb.AppendLine("e=>new{");

        var cnt = 0;

        foreach (var property in properties)
        {
            cnt++;

            var comma = cnt < properties.Count() ? "," : "";

            sb.AppendLine($"e.{property}{comma}");
        }

        sb.Append('}');

        return sb.ToString();
    }
}
