namespace EFSharpGen;

/// <summary>
/// A service to generate entities and configurations for a C# project that uses
/// the Entity Framework.
/// </summary>
public interface ICodeEngine
{
    /// <summary>
    /// Executes the code generation.
    /// </summary>
    void GenerateCode();
}
