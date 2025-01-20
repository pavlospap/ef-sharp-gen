using EFSharpGen.Design.Models;

namespace EFSharpGen.Design;

/// <summary>
/// A service to provide the schema that will be used for the code generation.
/// </summary>
public interface ISchemaProvider
{
    /// <summary>
    /// Gets the <see cref="Schema"></see> that will be used for the code
    /// generation.
    /// </summary>
    /// <returns>A <see cref="Schema"></see> that contains entities and
    /// relationships.</returns>
    Schema GetSchema();
}
