using EFSharpGen.Design.Models;

using Microsoft.Extensions.Options;

namespace EFSharpGen.Generators.Configurations;

/// <summary>
/// An implementation to generate the code for the namespace of a configuration.
/// </summary>
/// <param name="options">The application options.</param>
/// <param name="namespaceCodeGenerator">A service to generate the code for a
/// namespace.</param>
public class ConfigurationNamespaceCodeGenerator(
    IOptions<Options> options,
    INamespaceCodeGenerator namespaceCodeGenerator) :
    IConfigurationNamespaceCodeGenerator
{
    /// <summary>
    /// Indicates if the generator starts a new code block. This is usefull in
    /// order to add empty lines in the code, when necessary, and close code
    /// blocks at the end. It will be true if the
    /// <see cref="Options.FileScopedNamespaces"/> is false.
    /// </summary>
    public bool IsBlockStart => !options.Value.FileScopedNamespaces;

    /// <summary>
    /// Generates the code for the namespace of a configuration.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> for which the code will be
    /// generated.</param>
    /// <returns>The code for the namespace of a configuration.</returns>
    public virtual string? Code(Entity entity)
    {
        var @namespace = "namespace " +
            namespaceCodeGenerator.Code(options.Value.ConfigurationsPath);

        @namespace = options.Value.FileScopedNamespaces ?
            @namespace + ";" :
            @namespace + "{{";

        return @namespace + Environment.NewLine;
    }
}
