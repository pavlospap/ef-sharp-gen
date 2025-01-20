using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators.Entities;

/// <summary>
/// A service to generate the code for a reference navigation of an entity.
/// </summary>
public interface IEntityReferenceNavigationCodeGenerator
{
    /// <summary>
    /// Gets the code for a reference navigation of an entity.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> the reference navigation
    /// refers to.</param>
    /// <param name="property">The property name in case we don't want to use
    /// the entity name.</param>
    /// <returns>The code for a reference navigation of an entity.</returns>
    string Code(Entity entity, string? property = null);
}
