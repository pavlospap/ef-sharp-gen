using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators.Configurations;

/// <summary>
/// An implementation to generate the code for the class declaration of a
/// configuration.
/// </summary>
public class ConfigurationClassDeclarationCodeGenerator :
    IConfigurationClassDeclarationCodeGenerator
{
    /// <summary>
    /// Indicates if the generator starts a new code block. This is usefull in
    /// order to add empty lines in the code, when necessary, and close code
    /// blocks at the end. For the class declaration it will be true.
    /// </summary>
    public bool IsBlockStart => true;

    /// <summary>
    /// Generates the code for the class declaration of a configuration.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> for which the code will be
    /// generated.</param>
    /// <returns>The code for the class declaration of a configuration.</returns>
    public virtual string? Code(Entity entity)
    {
        var @partial = entity.HasCustomConfiguration ? "partial " : "";

        return
            $"public {partial}class {entity.Name}Configuration:" +
            $"IEntityTypeConfiguration<{entity.Name}>{{" +
            Environment.NewLine;
    }
}
