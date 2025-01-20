namespace EFSharpGen.Generators;

/// <summary>
/// A service to create a file after its content is generated.
/// </summary>
public interface IFileCreator
{
    /// <summary>
    /// Creates a file after its content is generated.
    /// </summary>
    /// <param name="relativePath">The relative (to the project path) path the
    /// file is going to be created.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="code">The generated code.</param>
    void CreateFile(string relativePath, string fileName, string code);
}
