using EFSharpGen.Miscellaneous;

using Microsoft.Extensions.Options;

namespace EFSharpGen.Generators;

/// <summary>
/// An implementation to create a file after its content is generated.
/// </summary>
/// <param name="options">The application options.</param>
/// <param name="fileRegistry">A registry of the generated files.</param>
public class FileCreator(
    IOptions<Options> options,
    IFileRegistry fileRegistry) : IFileCreator
{
    /// <summary>
    /// Creates a file after its content is generated.
    /// </summary>
    /// <param name="relativePath">The relative (to the project path) path the
    /// file is going to be created.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="code">The generated code.</param>
    public virtual void CreateFile(
        string relativePath, string fileName, string code)
    {
        var fullFileName = GetFullFileName(relativePath, fileName);

        File.WriteAllText(fullFileName, code);

        fileRegistry.RegisterFile(fullFileName);
    }

    string GetFullFileName(string relativePath, string fileName)
    {
        var absolutePath = Path.Combine(
            options.Value.ProjectPath, relativePath);

        Directory.CreateDirectory(absolutePath);

        return Path.Combine(absolutePath, fileName);
    }
}
