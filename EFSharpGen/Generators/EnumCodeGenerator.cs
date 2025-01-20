using System.Text;

using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators;

/// <summary>
/// An implementation to generate code for an enum.
/// </summary>
/// <param name="typeProvider">A service to provide a .Net CLR type.</param>
public class EnumCodeGenerator(ITypeProvider typeProvider) : IEnumCodeGenerator
{
    /// <summary>
    /// Gets the code for an enum.
    /// </summary>
    /// <param name="property">The <see cref="Property"/> for which the code
    /// will be generated.</param>
    /// <returns>The code for an enum.</returns>
    public virtual string Code(Property property)
    {
        var sb = new StringBuilder();

        var type = typeProvider.MapNetCLRType(property.DataType);

        sb.AppendLine($"public enum {property.EnumName}:{type}{{");

        var cnt = 0;

        foreach (var value in property.EnumValues!)
        {
            cnt++;

            var comma = cnt < property.EnumValues.Count ? "," : "";

            sb.AppendLine($"{value.Key}={value.Value}{comma}");
        }

        sb.Append('}');

        return sb.ToString();
    }
}
