using EFSharpGen.Design;
using EFSharpGen.Generators;
using EFSharpGen.Miscellaneous;

namespace EFSharpGen;

/// <summary>
/// An implementation to generate entities and configurations for a C# project
/// that uses the Entity Framework.
/// </summary>
/// <param name="appContext">The application context.</param>
/// <param name="schemaProvider">A service to provide the schema that will be
/// used for the code generation.</param>
/// <param name="fileGenerators">An <see cref="IEnumerable{T}"/> of services to
/// generate code files.</param>
/// <param name="codeFormatter">A service to format the generated files.</param>
/// <param name="fileRegistry">A service to register the generated files.</param>
public class CodeEngine(
    IAppContext appContext,
    ISchemaProvider schemaProvider,
    IEnumerable<IFileGenerator> fileGenerators,
    ICodeFormatter codeFormatter,
    IFileRegistry fileRegistry) : ICodeEngine
{
    /// <summary>
    /// Executes the code generation.
    /// </summary>
    public virtual void GenerateCode()
    {
        appContext.Schema = schemaProvider.GetSchema();

        foreach (var fileGenerator in fileGenerators)
        {
            fileGenerator.GenerateFiles();
        }

        codeFormatter.FormatFiles();

        fileRegistry.ClearRegistry();
    }
}
