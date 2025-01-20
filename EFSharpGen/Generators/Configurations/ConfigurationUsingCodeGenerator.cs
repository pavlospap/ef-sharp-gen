using System.Text;

using EFSharpGen.Design.Models;

using Microsoft.Extensions.Options;

namespace EFSharpGen.Generators.Configurations;

/// <summary>
/// An implementation to generate the code for the usings of a configuration.
/// </summary>
/// <param name="options">The application options.</param>
/// <param name="namespaceCodeGenerator">A service to generate the code for a
/// namespace.</param>
public class ConfigurationUsingCodeGenerator(
    IOptions<Options> options,
    INamespaceCodeGenerator namespaceCodeGenerator) :
    IConfigurationUsingCodeGenerator
{
    /// <summary>
    /// Generates the code for the usings of a configuration.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> for which the code will be
    /// generated.</param>
    /// <returns>The code for the usings of a configuration.</returns>
    public virtual string? Code(Entity entity)
    {
        var usings = new List<string>
        {
            "Microsoft.EntityFrameworkCore",
            "Microsoft.EntityFrameworkCore.Metadata.Builders",
            namespaceCodeGenerator.Code(options.Value.EntitiesPath)
        };

        usings = usings
            .Except(options.Value.ExcludedNamespaces)
            .ToList();

        var sb = new StringBuilder();

        usings.ForEach(u => sb.AppendLine($"using {u};"));

        return sb.ToString();
    }
}
