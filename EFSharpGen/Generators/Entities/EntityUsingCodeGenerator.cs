using System.Text;

using EFSharpGen.Design.Models;

using Microsoft.Extensions.Options;

namespace EFSharpGen.Generators.Entities;

/// <summary>
/// An implementation to generate the code for the usings of an entity.
/// </summary>
/// <param name="appContext">The application context.</param>
/// <param name="options">The application options.</param>
/// <param name="namespaceCodeGenerator">A service to generate the code for a
/// namespace.</param>
public class EntityUsingCodeGenerator(
    IAppContext appContext,
    IOptions<Options> options,
    INamespaceCodeGenerator namespaceCodeGenerator) : IEntityUsingCodeGenerator
{
    /// <summary>
    /// Generates the code for the usings of an entity.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> for which the code will be
    /// generated.</param>
    /// <returns>The code for the usings of an entity.</returns>
    public virtual string? Code(Entity entity)
    {
        var usings = new List<string>();

        if (!string.IsNullOrWhiteSpace(options.Value.EnumsPath) &&
            entity.Properties.Any(p => p.IsEnum))
        {
            usings.Add(namespaceCodeGenerator.Code(options.Value.EnumsPath));
        }

        if (entity.Properties.Any(p =>
                p.DataType == DataType.Guid ||
                p.DataType == DataType.DateTime))
        {
            usings.Add("System");
        }

        if (appContext.Schema.Relationships.Any(r =>
                r.RelationshipType == RelationshipType.OneToMany &&
                r.PrincipalEntity.Name == entity.Name))
        {
            usings.Add("System.Collections.Generic");
        }

        usings = usings
            .Except(options.Value.ExcludedNamespaces)
            .ToList();

        var sb = new StringBuilder();

        usings.ForEach(u => sb.AppendLine($"using {u};"));

        return sb.ToString();
    }
}
