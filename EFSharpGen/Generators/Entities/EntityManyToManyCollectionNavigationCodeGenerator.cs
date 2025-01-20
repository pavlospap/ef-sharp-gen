using System.Text;

using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators.Entities;

/// <summary>
/// An implementation to generate the code for the many-to-many collection
/// navigations of a principal entity.
/// </summary>
/// <param name="appContext">The application context.</param>
/// <param name="collectionNavigationCodeGenerator">A service to generate the
/// code for a collection navigation of an entity.</param>
public class EntityManyToManyCollectionNavigationCodeGenerator(
    IAppContext appContext,
    IEntityCollectionNavigationCodeGenerator collectionNavigationCodeGenerator) :
    IEntityManyToManyCollectionNavigationCodeGenerator
{
    /// <summary>
    /// Generates the code for the many-to-many collection navigations of a
    /// principal entity.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> for which the code will be
    /// generated.</param>
    /// <returns>The code for the many-to-many collection navigations of a
    /// principal entity.</returns>
    public virtual string? Code(Entity entity)
    {
        var relationships = appContext.Schema.Relationships
            .Where(r => r.RelationshipType == RelationshipType.ManyToMany)
            .Where(r => r.PrincipalEntity.Name == entity.Name)
            .OrderBy(r => r.DependentEntity.Name)
            .ToList();

        relationships.AddRange(appContext.Schema.Relationships
            .Where(r => r.RelationshipType == RelationshipType.ManyToMany)
            .Where(r => r.DependentEntity.Name == entity.Name)
            .OrderBy(r => r.PrincipalEntity.Name));

        var sb = new StringBuilder();

        var first = true;

        foreach (var relationship in relationships)
        {
            if (!first)
            {
                sb.AppendLine();
            }

            sb.AppendLine(collectionNavigationCodeGenerator.Code(
                relationship.PrincipalEntity.Name == entity.Name ?
                    relationship.DependentEntity :
                    relationship.PrincipalEntity));

            sb.AppendLine(collectionNavigationCodeGenerator.Code(
                appContext.Schema.Entities.Single(e => e.Name ==
                    relationship.PrincipalEntity.Name +
                    relationship.DependentEntity.Name)));

            first = false;
        }

        return sb.ToString();
    }
}
