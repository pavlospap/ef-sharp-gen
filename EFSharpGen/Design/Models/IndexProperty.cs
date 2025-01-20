namespace EFSharpGen.Design.Models;

/// <summary>
/// Represents the index order.
/// </summary>
public enum IndexOrder
{
    /// <summary>
    /// The index ascending order.
    /// </summary>
    Ascending,

    /// <summary>
    /// The index descending order.
    /// </summary>
    Descending
}

/// <summary>
/// Represents a property of an index.
/// </summary>
public class IndexProperty
{
    /// <summary>
    /// The property name.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The index order. The default value is <see cref="IndexOrder.Ascending"/>.
    /// </summary>
    public IndexOrder IndexOrder { get; set; } = IndexOrder.Ascending;
}
