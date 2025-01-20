namespace EFSharpGen.Generators;

/// <summary>
/// A service to generate code files.
/// </summary>
public interface IFileGenerator
{
    /// <summary>
    /// Executes the generation of the code files.
    /// </summary>
    void GenerateFiles();
}
