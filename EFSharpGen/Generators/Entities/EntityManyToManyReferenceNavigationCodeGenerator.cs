using System.Text;

using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators.Entities;

/// <summary>
/// An implementation to generate the code for the many-to-many reference
/// navigations of a dependent entity.
/// </summary>
/// <param name="appContext">The application context.</param>
/// <param name="propertyCodeGenerator">A service to generate the code for a
/// property of an entity.</param>
/// <param name="referenceNavigationCodeGenerator">A service to generate the
/// code for a reference navigation of an entity.</param>
public class EntityManyToManyReferenceNavigationCodeGenerator(
    IAppContext appContext,
    IEntityPropertyCodeGenerator propertyCodeGenerator,
    IEntityReferenceNavigationCodeGenerator referenceNavigationCodeGenerator) :
    IEntityManyToManyReferenceNavigationCodeGenerator
{
    /// <summary>
    /// Generates the code for the many-to-many reference navigations of a
    /// dependent entity.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> for which the code will be
    /// generated.</param>
    /// <returns>The code for the many-to-many reference navigations of a
    /// dependent entity.</returns>
    public virtual string? Code(Entity entity)
    {
        var relationship = appContext.Schema.Relationships.SingleOrDefault(r =>
            r.RelationshipType == RelationshipType.ManyToMany &&
            r.PrincipalEntity.Name + r.DependentEntity.Name == entity.Name);

        if (relationship == null)
        {
            return null;
        }

        var relationshipProperties = new (Entity Entity, Property Property)[]
        {
            (relationship.PrincipalEntity, entity.Properties.Single(
                p => p.Name == relationship.PrincipalEntity.Name + "Id")),
            (relationship.DependentEntity, entity.Properties.Single(
                p => p.Name == relationship.DependentEntity.Name + "Id"))
        };

        var sb = new StringBuilder();

        var first = true;

        foreach (var relationshipProperty in relationshipProperties)
        {
            if (!first)
            {
                sb.AppendLine();
            }

            sb.AppendLine(
                propertyCodeGenerator.Code(relationshipProperty.Property));

            sb.AppendLine(
                referenceNavigationCodeGenerator.Code(relationshipProperty.Entity));

            first = false;
        }

        return sb.ToString();
    }
}
