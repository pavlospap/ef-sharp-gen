using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators.Entities;

/// <summary>
/// A service to generate the code for a property of an entity.
/// </summary>
public interface IEntityPropertyCodeGenerator
{
    /// <summary>
    /// Gets the code for a property of an entity.
    /// </summary>
    /// <param name="property">The <see cref="Property"/> for which the code
    /// will be generated.</param>
    /// <returns>The code for a property of an entity.</returns>
    string Code(Property property);
}
