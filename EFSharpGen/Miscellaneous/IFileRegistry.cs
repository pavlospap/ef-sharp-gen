namespace EFSharpGen.Miscellaneous;

/// <summary>
/// A service to register the generated files.
/// </summary>
public interface IFileRegistry
{
    /// <summary>
    /// Registers a file.
    /// </summary>
    /// <param name="file">The full path of the file to register.</param>
    void RegisterFile(string file);

    /// <summary>
    /// Gets a list of the registered files.
    /// </summary>
    /// <returns>A list of the registered files.</returns>
    List<string> GetRegisteredFiles();

    /// <summary>
    /// Clears the registry.
    /// </summary>
    void ClearRegistry();
}
