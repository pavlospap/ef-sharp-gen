namespace EFSharpGen.Miscellaneous;

/// <summary>
/// An implementation to register the generated files.
/// </summary>
public class FileRegistry : IFileRegistry
{
    readonly List<string> _files = [];

    /// <summary>
    /// Registers a file.
    /// </summary>
    /// <param name="file">The full path of the file to register.</param>
    public virtual void RegisterFile(string file) => _files.Add(file);

    /// <summary>
    /// Gets a list of the registered files.
    /// </summary>
    /// <returns>A list of the registered files.</returns>
    public virtual List<string> GetRegisteredFiles() => _files;

    /// <summary>
    /// Clears the registry.
    /// </summary>
    public virtual void ClearRegistry() => _files.Clear();
}
