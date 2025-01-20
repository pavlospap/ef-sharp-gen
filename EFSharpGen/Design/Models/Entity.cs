namespace EFSharpGen.Design.Models;

/// <summary>
/// Represents an entity.
/// </summary>
public class Entity
{
    /// <summary>
    /// The entity name.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Indicates if the entity has custom configuration.
    /// </summary>
    public bool HasCustomConfiguration { get; set; }

    /// <summary>
    /// The entity properties.
    /// </summary>
    public List<Property> Properties { get; set; } = [];

    /// <summary>
    /// The entity indexes.
    /// </summary>
    public List<Index> Indexes { get; set; } = [];
}
