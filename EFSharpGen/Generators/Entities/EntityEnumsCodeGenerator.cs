using System.Text;

using EFSharpGen.Design.Models;

using Microsoft.Extensions.Options;

namespace EFSharpGen.Generators.Entities;

/// <summary>
/// An implementation to generate the code for the enums of an entity.
/// </summary>
/// <param name="options">The application options.</param>
/// <param name="enumCodeGenerator">A service to generate code for an enum.</param>
public class EntityEnumsCodeGenerator(
    IOptions<Options> options,
    IEnumCodeGenerator enumCodeGenerator) : IEntityEnumCodeGenerator
{
    /// <summary>
    /// Generates the code for the enums of an entity.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> for which the code will be
    /// generated.</param>
    /// <returns>The code for the enums of an entity.</returns>
    public virtual string? Code(Entity entity)
    {
        if (!string.IsNullOrWhiteSpace(options.Value.EnumsPath))
        {
            return null;
        }

        var sb = new StringBuilder();

        var first = true;

        foreach (var property in entity.Properties.Where(p => p.IsEnum))
        {
            if (!first)
            {
                sb.AppendLine();
            }

            sb.AppendLine(enumCodeGenerator.Code(property));

            first = false;
        }

        return sb.ToString();
    }
}
