using System.Text;

using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators.Entities;

/// <summary>
/// An implementation to generate the code for the simple properties of an entity.
/// </summary>
/// <param name="appContext">The application context.</param>
/// <param name="propertyCodeGenerator">A service to generate the code for a
/// property of an entity.</param>
public class EntitySimplePropertyCodeGenerator(
    IAppContext appContext,
    IEntityPropertyCodeGenerator propertyCodeGenerator) :
    IEntitySimplePropertyCodeGenerator
{
    /// <summary>
    /// Generates the code for the simple properties of an entity.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> for which the code will be
    /// generated.</param>
    /// <returns>The code for the simple properties of an entity.</returns>
    public virtual string? Code(Entity entity)
    {
        var properties = entity.Properties
            .Where(p => !p.IsPrimaryKey)
            .Where(p => !appContext.Schema.Relationships.Any(r =>
                (r.PrincipalProperty.Name == p.Name &&
                 r.PrincipalEntity.Name == entity.Name) ||
                (r.DependentProperty.Name == p.Name &&
                 r.DependentEntity.Name == entity.Name)));

        var sb = new StringBuilder();

        foreach (var property in properties)
        {
            sb.AppendLine(propertyCodeGenerator.Code(property));
        }

        return sb.ToString();
    }
}
