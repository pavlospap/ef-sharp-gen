using System.Text;

using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators.Entities;

/// <summary>
/// An implementation to generate the code for the one-to-many collection
/// navigations of a principal entity.
/// </summary>
/// <param name="appContext">The application context.</param>
/// <param name="collectionNavigationCodeGenerator">A service to generate a
/// collection navigation of an entity.</param>
public class EntityOneToManyCollectionNavigationCodeGenerator(
    IAppContext appContext,
    IEntityCollectionNavigationCodeGenerator collectionNavigationCodeGenerator) :
    IEntityOneToManyCollectionNavigationCodeGenerator
{
    /// <summary>
    /// Generates the code for the one-to-many collection navigations of a
    /// principal entity.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> for which the code will be
    /// generated.</param>
    /// <returns>The code for the one-to-many collection navigations of a
    /// principal entity.</returns>
    public virtual string? Code(Entity entity)
    {
        var relationships = appContext.Schema.Relationships
            .Where(r => r.RelationshipType == RelationshipType.OneToMany)
            .Where(r => r.PrincipalEntity.Name == entity.Name)
            .OrderBy(r => r.DependentEntity.Name);

        var sb = new StringBuilder();

        foreach (var relationship in relationships)
        {
            sb.AppendLine(collectionNavigationCodeGenerator.Code(
                relationship.DependentEntity));
        }

        return sb.ToString();
    }
}
