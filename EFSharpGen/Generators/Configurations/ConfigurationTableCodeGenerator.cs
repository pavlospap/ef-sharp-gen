using EFSharpGen.Design.Models;

using Microsoft.Extensions.Options;

namespace EFSharpGen.Generators.Configurations;

/// <summary>
/// An implementation to generate the code for the table configuration.
/// </summary>
/// <param name="options">The application options.</param>
/// <param name="efConfigurationProvider">A service to provide configuration for
/// the Entity Framework.</param>
public class ConfigurationTableCodeGenerator(
    IOptions<Options> options,
    IEFConfigurationProvider efConfigurationProvider) :
    IConfigurationTableCodeGenerator
{
    /// <summary>
    /// Generates the code for the table configuration.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> for which the code will be
    /// generated.</param>
    /// <returns>The code for the table configuration.</returns>
    public virtual string? Code(Entity entity)
    {
        if (options.Value.CustomDbNames)
        {
            var tableName = efConfigurationProvider.GetTableName(entity);

            return $"builder.ToTable(\"{tableName}\");" + Environment.NewLine;
        }

        return null;
    }
}
