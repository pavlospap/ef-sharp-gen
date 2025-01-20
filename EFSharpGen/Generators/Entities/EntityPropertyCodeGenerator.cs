using EFSharpGen.Design.Models;

namespace EFSharpGen.Generators.Entities;

/// <summary>
/// An implementation to generate the code for a property of an entity.
/// </summary>
/// <param name="typeProvider">A service to provide a .Net CLR type.</param>
public class EntityPropertyCodeGenerator(ITypeProvider typeProvider) :
    IEntityPropertyCodeGenerator
{
    /// <summary>
    /// Gets the code for a property of an entity.
    /// </summary>
    /// <param name="property">The <see cref="Property"/> for which the code
    /// will be generated.</param>
    /// <returns>The code for a property of an entity.</returns>
    public virtual string Code(Property property)
    {
        var type = typeProvider.GetPropertyType(property);

        return $"public {type} {property.Name}{{get;set;}}";
    }
}
