using System.Text;

using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators.Entities;

/// <summary>
/// An implementation to generate the code for the one-to-one or one-to-many
/// reference navigations of a dependent entity.
/// </summary>
/// <param name="appContext">The application context.</param>
/// <param name="propertyCodeGenerator">A service to generate the code for a
/// property of an entity.</param>
/// <param name="referenceNavigationCodeGenerator">A service to generate the
/// code for a reference navigation of an entity.</param>
public class EntityOneToNReferenceNavigationCodeGenerator(
    IAppContext appContext,
    IEntityPropertyCodeGenerator propertyCodeGenerator,
    IEntityReferenceNavigationCodeGenerator referenceNavigationCodeGenerator) :
    IEntityOneToNReferenceNavigationCodeGenerator
{
    /// <summary>
    /// Generates the code for the one-to-one or one-to-many reference
    /// navigations of a dependent entity.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> for which the code will be
    /// generated.</param>
    /// <returns>The code for the one-to-one or one-to-many reference navigations
    /// of a dependent entity.</returns>
    public virtual string? Code(Entity entity)
    {
        var relationships = appContext.Schema.Relationships
            .Where(r =>
                (r.RelationshipType == RelationshipType.OneToOne ||
                 r.RelationshipType == RelationshipType.OneToMany) &&
                 r.DependentEntity.Name == entity.Name)
            .OrderBy(r => r.PrincipalEntity.Name);

        var sb = new StringBuilder();

        var first = true;

        foreach (var relationship in relationships)
        {
            if (!first)
            {
                sb.AppendLine();
            }

            sb.AppendLine(propertyCodeGenerator.Code(
                relationship.DependentProperty));

            sb.AppendLine(referenceNavigationCodeGenerator.Code(
                relationship.PrincipalEntity,
                relationship.DependentProperty.Name[..^2]));

            first = false;
        }

        return sb.ToString();
    }
}
