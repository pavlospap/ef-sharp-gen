namespace EFSharpGen.Design.Models;

/// <summary>
/// Represents an index.
/// </summary>
public class Index
{
    /// <summary>
    /// The index properties.
    /// </summary>
    public List<IndexProperty> Properties { get; set; } = [];

    /// <summary>
    /// Indicates if the index is unique.
    /// </summary>
    public bool IsUnique { get; set; }
}
