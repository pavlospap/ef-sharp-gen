using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators;

/// <summary>
/// A service to provide a .Net CLR type.
/// </summary>
public interface ITypeProvider
{
    /// <summary>
    /// Gets the .Net CLR type for a <see cref="Property"/>.
    /// </summary>
    /// <param name="property">The <see cref="Property"/> for which the type
    /// will be provided.</param>
    /// <returns>The name of a .Net CLR type.</returns>
    string GetPropertyType(Property property);

    /// <summary>
    /// Maps a <see cref="DataType"/> with its corresponding .Net CLR type.
    /// </summary>
    /// <param name="dataType">The <see cref="DataType"/> of the
    /// <see cref="Property"/>.</param>
    /// <returns>The name of a .Net CLR type.</returns>
    string MapNetCLRType(DataType dataType);
}
