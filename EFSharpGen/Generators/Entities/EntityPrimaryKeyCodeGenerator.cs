using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators.Entities;

/// <summary>
/// An implementation to generate the code for the primary key of an entity.
/// </summary>
/// <param name="propertyCodeGenerator">A service to generate the code for a
/// property of an entity.</param>
public class EntityPrimaryKeyCodeGenerator(
    IEntityPropertyCodeGenerator propertyCodeGenerator) :
    IEntityPrimaryKeyCodeGenerator
{
    /// <summary>
    /// Generates the code for the primary key of an entity.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> for which the code will be
    /// generated.</param>
    /// <returns>The code for the primary key of an entity.</returns>
    public virtual string? Code(Entity entity)
    {
        var pkProperties = entity.Properties.Where(p => p.IsPrimaryKey);

        if (pkProperties.Count() == 1)
        {
            return propertyCodeGenerator.Code(pkProperties.First()) +
                Environment.NewLine;
        }

        return null;
    }
}
