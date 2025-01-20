namespace EFSharpGen.Generators.Configurations;

/// <summary>
/// A service to generate the code for a composite key configuration.
/// </summary>
public interface IConfigurationCompositeKeyCodeGenerator
{
    /// <summary>
    /// Generates the code for a composite key lambda configuration.
    /// </summary>
    /// <param name="properties">The properties that compose the key.</param>
    /// <returns>The code for a composite key lambda configuration.</returns>
    string BuildCompositeKeyLambda(IEnumerable<string> properties);
}
