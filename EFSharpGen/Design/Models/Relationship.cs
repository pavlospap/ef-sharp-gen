namespace EFSharpGen.Design.Models;

/// <summary>
/// The entity relationship type.
/// </summary>
public enum RelationshipType
{
    /// <summary>
    /// The one-to-one relationship.
    /// </summary>
    OneToOne,

    /// <summary>
    /// The one-to-many relationship.
    /// </summary>
    OneToMany,

    /// <summary>
    /// The many-to-many relationship.
    /// </summary>
    ManyToMany
}

/// <summary>
/// Represents an entity relationship.
/// </summary>
public class Relationship
{
    /// <summary>
    /// The entity relationship type.
    /// </summary>
    public RelationshipType RelationshipType { get; set; }

    /// <summary>
    /// The entity that contains the principal property.
    /// </summary>
    public Entity PrincipalEntity { get; set; } = default!;

    /// <summary>
    /// The principal property.
    /// </summary>
    public Property PrincipalProperty { get; set; } = default!;

    /// <summary>
    /// The entity that contains the dependent property.
    /// </summary>
    public Entity DependentEntity { get; set; } = default!;

    /// <summary>
    /// The dependent property.
    /// </summary>
    public Property DependentProperty { get; set; } = default!;
}
