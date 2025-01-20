namespace EFSharpGen.Design.Drawio;

/// <summary>
/// Represents a draw.io diagram element.
/// </summary>
public class Element
{
    /// <summary>
    /// The element ID.
    /// </summary>
    public string Id { get; set; } = default!;

    /// <summary>
    /// The element name.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The element attributes.
    /// </summary>
    public IReadOnlyDictionary<string, string> Attributes { get; set; } = default!;
}
