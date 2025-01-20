using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators.Configurations;

/// <summary>
/// An implementation to generate the code for the Configure method declaration
/// of a configuration.
/// </summary>
public class ConfigurationConfigureMethodDeclarationCodeGenerator :
    IConfigurationConfigureMethodDeclarationCodeGenerator
{
    /// <summary>
    /// Indicates if the generator starts a new code block. This is usefull in
    /// order to add empty lines in the code, when necessary, and close code
    /// blocks at the end. For the Configure method declaration it will be true.
    /// </summary>
    public bool IsBlockStart => true;

    /// <summary>
    /// Generates the code for the Configure method declaration of a configuration.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> for which the code will be
    /// generated.</param>
    /// <returns>The code for the Configure method declaration of a
    /// configuration.</returns>
    public virtual string? Code(Entity entity) =>
        $"public void Configure(EntityTypeBuilder<{entity.Name}> builder){{" +
        Environment.NewLine;
}
