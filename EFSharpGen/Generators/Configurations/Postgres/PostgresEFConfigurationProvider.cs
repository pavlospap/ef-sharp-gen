using EFSharpGen.Design.Models;

using Humanizer;

namespace EFSharpGen.Generators.Configurations.Postgres;

/// <summary>
/// An Postgres implementation to provide configuration for the Entity Framework.
/// </summary>
public class PostgresEFConfigurationProvider : IEFConfigurationProvider
{
    /// <summary>
    /// Gets the table name.
    /// </summary>
    /// <param name="entity">The schema <see cref="Entity"></see>.</param>
    /// <returns>The table name.</returns>
    public virtual string GetTableName(Entity entity) =>
        entity.Name.Underscore();

    /// <summary>
    /// Gets the column name.
    /// </summary>
    /// <param name="property">The entity <see cref="Property"></see>.</param>
    /// <returns>The column name.</returns>
    public virtual string GetColumnName(Property property) =>
        property.Name.Underscore();

    /// <summary>
    /// Gets the primary key name.
    /// </summary>
    /// <param name="entity">The schema <see cref="Entity"></see>.</param>
    /// <returns>The primary key name.</returns>
    public virtual string GetPrimaryKeyName(Entity entity) =>
        $"{GetTableName(entity)}_pkey";

    /// <summary>
    /// Gets the constraint name for a specific column.
    /// </summary>
    /// <param name="entity">The schema <see cref="Entity"></see>.</param>
    /// <param name="property">The entity <see cref="Property"></see>.</param>
    /// <returns>The constraint name for a specific column.</returns>
    public virtual string GetColumnConstraintName(Entity entity, Property property) =>
        $"{GetTableName(entity)}_{GetColumnName(property)}_check";

    /// <summary>
    /// Gets the sql constraint condition for a <see cref="Property"></see> if
    /// <see cref="Property.IsEnum"></see> is true.
    /// </summary>
    /// <param name="property">The entity <see cref="Property"></see>.</param>
    /// <returns>The sql constraint condition.</returns>
    public virtual string GetEnumConstraintCondition(Property property)
    {
        var column = GetColumnName(property);

        var values = property.EnumValues!.Values;

        if (property.IsArray)
        {
            var dataType = GetDbDataType(property.DataType);

            var valuesWithCast = values.Select(v => $"{v}::{dataType}");

            return $"{column} <@ ARRAY[{string.Join(", ", valuesWithCast)}]";
        }

        return $"{column} = ANY (ARRAY[{string.Join(", ", values)}])";
    }

    /// <summary>
    /// Gets the database data type.
    /// </summary>
    /// <param name="dataType">The <see cref="DataType"></see> of a
    /// <see cref="Property"></see>.</param>
    /// <returns>The database data type.</returns>
    public virtual string GetDbDataType(DataType dataType)
    {
        return dataType switch
        {
            DataType.Int8 or
            DataType.Int16 => "smallint",
            DataType.Int32 => "integer",
            DataType.Int64 => "bigint",
            _ => throw new NotImplementedException(nameof(DataType))
        };
    }
}
