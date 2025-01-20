using System.Text;

using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators.Configurations;

/// <summary>
/// An implementation to generate the code for enum constraints configuration.
/// </summary>
/// <param name="efConfigurationProvider">A service to provide configuration for
/// the Entity Framework.</param>
public class ConfigurationEnumConstraintCodeGenerator(
    IEFConfigurationProvider efConfigurationProvider) :
    IConfigurationEnumConstraintCodeGenerator
{
    /// <summary>
    /// Generates the code for enum constraints configuration.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> for which the code will be
    /// generated.</param>
    /// <returns>The code for enum constraints configuration.</returns>
    public virtual string? Code(Entity entity)
    {
        var sb = new StringBuilder();

        var first = true;

        foreach (var property in entity.Properties.Where(p => p.IsEnum))
        {
            if (!first)
            {
                sb.AppendLine();
            }

            sb.AppendLine("builder.ToTable(b => b.HasCheckConstraint(");

            var constraintName = efConfigurationProvider.GetColumnConstraintName(
                entity, property);

            sb.AppendLine(tabs: 1, $"\"{constraintName}\",");

            var condition = efConfigurationProvider.GetEnumConstraintCondition(
                property);

            sb.AppendLine(tabs: 1, $"\"{condition}\"));");

            first = false;
        }

        return sb.ToString();
    }
}
