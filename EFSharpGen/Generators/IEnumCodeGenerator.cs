using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators;

/// <summary>
/// A service to generate code for an enum.
/// </summary>
public interface IEnumCodeGenerator
{
    /// <summary>
    /// Gets the code for an enum.
    /// </summary>
    /// <param name="property">The <see cref="Property"/> for which the code
    /// will be generated.</param>
    /// <returns>The code for an enum.</returns>
    string Code(Property property);
}
