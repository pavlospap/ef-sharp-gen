using Microsoft.Extensions.Options;

namespace EFSharpGen.Generators;

/// <summary>
/// An implementation to generate the code for a namespace.
/// </summary>
/// <param name="options">The application options.</param>
public class NamespaceCodeGenerator(IOptions<Options> options) :
    INamespaceCodeGenerator
{
    /// <summary>
    /// Generates the code for a namespace based on a relative path.
    /// </summary>
    /// <param name="relativePath">The relative (to the
    /// <see cref="Options.ProjectPath"/>) path for which the namespace will be
    /// generated.</param>
    /// <returns>The code for a namespace based on a relative path.</returns>
    public virtual string Code(string relativePath)
    {
        var separators = new[]
        {
          Path.DirectorySeparatorChar,
          Path.AltDirectorySeparatorChar
        };

        var namespaceParts = relativePath
            .Split(separators, StringSplitOptions.RemoveEmptyEntries);

        var @namespace = string.Join(".", namespaceParts);

        return $"{options.Value.DefaultNamespace}.{@namespace}"
            .Replace(' ', '_');
    }
}
