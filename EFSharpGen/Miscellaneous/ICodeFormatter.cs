namespace EFSharpGen.Miscellaneous;

/// <summary>
/// A service to format the generated files.
/// </summary>
public interface ICodeFormatter
{
    /// <summary>
    /// Formats the code in the generated files. You can get these files from
    /// the <see cref="IFileRegistry"/> service.
    /// </summary>
    void FormatFiles();
}
