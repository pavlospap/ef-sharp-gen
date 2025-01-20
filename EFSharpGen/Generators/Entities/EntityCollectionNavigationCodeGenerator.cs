using EFSharpGen.Design.Models;

using Humanizer;

using Microsoft.Extensions.Options;

namespace EFSharpGen.Generators.Entities;

/// <summary>
/// An implementation to generate the code for a collection navigation of an entity.
/// </summary>
/// <param name="options">The application options.</param>
public class EntityCollectionNavigationCodeGenerator(IOptions<Options> options) :
    IEntityCollectionNavigationCodeGenerator
{
    /// <summary>
    /// Generatew the code for a collection navigation of an entity.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> the collection navigation
    /// refers to.</param>
    /// <returns>The code for a collection navigation of an entity.</returns>
    public virtual string? Code(Entity entity)
    {
        var @virtual = options.Value.VirtualNavigations ? "virtual " : "";

        var type = options.Value.ListNavigations ?
            $"List<{entity.Name}>" :
            $"ICollection<{entity.Name}>";

        var property = entity.Name.Pluralize();

        var initialization = options.Value.ListNavigations ?
            "[]" :
            $"new HashSet<{entity.Name}>()";

        return
            $"public {@virtual}{type} {property} {{get;set;}}={initialization};";
    }
}
