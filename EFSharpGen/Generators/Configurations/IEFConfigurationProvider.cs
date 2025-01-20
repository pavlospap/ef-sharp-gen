using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators.Configurations;

/// <summary>
/// A service to provide configuration for the Entity Framework.
/// </summary>
public interface IEFConfigurationProvider
{
    /// <summary>
    /// Gets the table name.
    /// </summary>
    /// <param name="entity">The schema <see cref="Entity"></see>.</param>
    /// <returns>The table name.</returns>
    string GetTableName(Entity entity);

    /// <summary>
    /// Gets the column name.
    /// </summary>
    /// <param name="property">The entity <see cref="Property"></see>.</param>
    /// <returns>The column name.</returns>
    string GetColumnName(Property property);

    /// <summary>
    /// Gets the primary key name.
    /// </summary>
    /// <param name="entity">The schema <see cref="Entity"></see>.</param>
    /// <returns>The primary key name.</returns>
    string GetPrimaryKeyName(Entity entity);

    /// <summary>
    /// Gets the constraint name for a specific column.
    /// </summary>
    /// <param name="entity">The schema <see cref="Entity"></see>.</param>
    /// <param name="property">The entity <see cref="Property"></see>.</param>
    /// <returns>The constraint name for a specific column.</returns>
    string GetColumnConstraintName(Entity entity, Property property);

    /// <summary>
    /// Gets the sql constraint condition for a <see cref="Property"></see> if
    /// <see cref="Property.IsEnum"></see> is true.
    /// </summary>
    /// <param name="property">The entity <see cref="Property"></see>.</param>
    /// <returns>The sql constraint condition.</returns>
    string GetEnumConstraintCondition(Property property);

    /// <summary>
    /// Gets the database data type.
    /// </summary>
    /// <param name="dataType">The <see cref="DataType"></see> of a
    /// <see cref="Property"></see>.</param>
    /// <returns>The database data type.</returns>
    string GetDbDataType(DataType dataType);
}
