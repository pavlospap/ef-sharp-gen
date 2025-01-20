namespace EFSharpGen.Generators;

/// <summary>
/// A service to generate the code for a namespace.
/// </summary>
public interface INamespaceCodeGenerator
{
    /// <summary>
    /// Generates the code for a namespace based on a relative path.
    /// </summary>
    /// <param name="relativePath">The relative (to the
    /// <see cref="Options.ProjectPath"/>) path for which the namespace will be
    /// generated.</param>
    /// <returns>The code for a namespace based on a relative path.</returns>
    string Code(string relativePath);
}
