using EFSharpGen.Design.Models;

using Microsoft.Extensions.Options;

namespace EFSharpGen.Generators.Entities;

/// <summary>
/// An implementation to generate the code for a reference navigation of an entity.
/// </summary>
public class EntityReferenceNavigationCodeGenerator(IOptions<Options> options) :
    IEntityReferenceNavigationCodeGenerator
{
    /// <summary>
    /// Gets the code for a reference navigation of an entity.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> the reference navigation
    /// refers to.</param>
    /// <param name="property">The property name in case we don't want to use
    /// the entity name.</param>
    /// <returns>The code for a reference navigation of an entity.</returns>
    public virtual string Code(Entity entity, string? property = null)
    {
        var @virtual = options.Value.VirtualNavigations ? "virtual " : "";

        property ??= entity.Name;

        return $"public {@virtual}{entity.Name} {property}{{get;set;}}";
    }
}
