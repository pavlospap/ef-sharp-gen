using System.Text;

using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators.Entities;

/// <summary>
/// An implementation to generate the code for the one-to-one reference
/// navigations of a principal entity.
/// </summary>
/// <param name="appContext">The application context.</param>
/// <param name="referenceNavigationCodeGenerator">A service to generate the
/// code for a reference navigation of an entity.</param>
public class EntityOneToOneReferenceNavigationCodeGenerator(
    IAppContext appContext,
    IEntityReferenceNavigationCodeGenerator referenceNavigationCodeGenerator) :
    IEntityOneToOneReferenceNavigationCodeGenerator
{
    /// <summary>
    /// Generate the code for the one-to-one reference navigations of a
    /// principal entity.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> for which the code will be
    /// generated.</param>
    /// <returns>The code for the one-to-one reference navigations of a principal
    /// entity.</returns>
    public virtual string? Code(Entity entity)
    {
        var sb = new StringBuilder();

        var relationships = appContext.Schema.Relationships
           .Where(r => r.RelationshipType == RelationshipType.OneToOne)
           .Where(r => r.PrincipalEntity.Name == entity.Name)
           .OrderBy(r => r.DependentEntity.Name);

        foreach (var relationship in relationships)
        {
            sb.AppendLine(referenceNavigationCodeGenerator.Code(relationship.DependentEntity));
        }

        return sb.ToString();
    }
}
