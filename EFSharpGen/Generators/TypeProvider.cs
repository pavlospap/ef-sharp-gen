using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators;

/// <summary>
/// An implementation to provide a .Net CLR type.
/// </summary>
public class TypeProvider : ITypeProvider
{
    /// <summary>
    /// Gets the .Net CLR type for a <see cref="Property"/>.
    /// </summary>
    /// <param name="property">The <see cref="Property"/> for which the type
    /// will be provided.</param>
    /// <returns>The name of a .Net CLR type.</returns>
    public virtual string GetPropertyType(Property property)
    {
        var dataType = property.IsEnum ?
            property.EnumName :
            MapNetCLRType(property.DataType);

        ApplyArray(ref dataType!, property);

        ApplyNullity(ref dataType, property);

        return dataType;
    }

    /// <summary>
    /// Maps a <see cref="DataType"/> with its corresponding .Net CLR type.
    /// </summary>
    /// <param name="dataType">The <see cref="DataType"/> of the
    /// <see cref="Property"/>.</param>
    /// <returns>The name of a .Net CLR type.</returns>
    public virtual string MapNetCLRType(DataType dataType)
    {
        return dataType switch
        {
            DataType.Guid => "Guid",
            DataType.String => "string",
            DataType.Int8 => "byte",
            DataType.Int16 => "short",
            DataType.Int32 => "int",
            DataType.Int64 => "long",
            DataType.DateTime => "DateTime",
            DataType.Boolean => "bool",
            DataType.Blob => "byte[]",
            _ => throw new NotSupportedException(nameof(DataType)),
        };
    }

    static void ApplyArray(ref string dataType, Property property)
    {
        if (property.IsArray)
        {
            dataType += "[]";
        }
    }

    static void ApplyNullity(ref string dataType, Property property)
    {
        if (property.IsNullable &&
            property.DataType != DataType.String &&
            property.DataType != DataType.Blob &&
            !property.IsArray)
        {
            dataType += "?";
        }
    }
}
