using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators;

/// <summary>
/// A service to generate a piece of code.
/// </summary>
public interface ICodeGenerator
{
    /// <summary>
    /// Indicates if the generator starts a new code block. This is usefull in
    /// order to add empty lines in the code, when necessary, and close code
    /// blocks at the end. The default value is false.
    /// </summary>
    bool IsBlockStart { get => false; }

    /// <summary>
    /// Generates a piece of code.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> for which the code will be
    /// generated.</param>
    /// <returns>A piece of code.</returns>
    string? Code(Entity entity);
}
