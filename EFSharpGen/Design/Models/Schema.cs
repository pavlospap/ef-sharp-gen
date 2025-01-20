namespace EFSharpGen.Design.Models;

/// <summary>
/// Represents a schema.
/// </summary>
public class Schema
{
    /// <summary>
    /// The schema entities.
    /// </summary>
    public List<Entity> Entities { get; set; } = [];

    /// <summary>
    /// The schema relationships.
    /// </summary>
    public List<Relationship> Relationships { get; set; } = [];
}
