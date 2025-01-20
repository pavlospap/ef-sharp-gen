using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators.Entities;

/// <summary>
/// An implementation to generate the code for the class declaration of an entity.
/// </summary>
public class EntityClassDeclarationCodeGenerator :
    IEntityClassDeclarationCodeGenerator
{
    /// <summary>
    /// Indicates if the generator starts a new code block. This is usefull in
    /// order to add empty lines in the code, when necessary, and close code
    /// blocks at the end. For the class declaration it will be true.
    /// </summary>
    public bool IsBlockStart => true;

    /// <summary>
    /// Generates the code for the class declaration of an entity.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> for which the code will be
    /// generated.</param>
    /// <returns>The code for the class declaration of an entity.</returns>
    public virtual string? Code(Entity entity) =>
        $"public class {entity.Name}{{" + Environment.NewLine;
}
