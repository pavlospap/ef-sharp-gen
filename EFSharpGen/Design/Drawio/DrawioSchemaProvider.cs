using System.Xml.Linq;

using EFSharpGen.Design.Models;

using Microsoft.Extensions.Options;

namespace EFSharpGen.Design.Drawio;

/// <summary>
/// An draw.io implementation to provide the schema that will be used for the
/// code generation.
/// </summary>
/// <param name="options">The application options.</param>
public class DrawioSchemaProvider(IOptions<Options> options) : ISchemaProvider
{
    static class Arrow
    {
        public const string ERone = "ERone";
        public const string ERmandOne = "ERmandOne";
        public const string ERzeroToOne = "ERzeroToOne";
        public const string ERmany = "ERmany";
        public const string ERoneToMany = "ERoneToMany";
        public const string ERzeroToMany = "ERzeroToMany";
    }

    readonly IReadOnlyDictionary<
        RelationshipType,
        List<(string?, string)>> _relationshipArrows =
            new Dictionary<RelationshipType, List<(string?, string)>>
            {
                {
                    RelationshipType.OneToOne,
                    new()
                    {
                        (null, Arrow.ERmandOne),
                        (Arrow.ERmandOne, Arrow.ERmandOne),
                        (null, Arrow.ERone),
                        (null, Arrow.ERzeroToOne),
                        (Arrow.ERmandOne, Arrow.ERzeroToOne)
                    }
                },
                {
                    RelationshipType.OneToMany,
                    new()
                    {
                        (null, Arrow.ERzeroToMany),
                        (null, Arrow.ERoneToMany),
                        (null, Arrow.ERmany),
                        (Arrow.ERzeroToOne, Arrow.ERzeroToMany),
                        (Arrow.ERmandOne, Arrow.ERzeroToMany),
                        (Arrow.ERmandOne, Arrow.ERoneToMany),
                        (Arrow.ERzeroToOne, Arrow.ERoneToMany)
                    }
                },
                {
                    RelationshipType.ManyToMany,
                    new()
                    {
                        (Arrow.ERmany, Arrow.ERmany),
                        (Arrow.ERoneToMany, Arrow.ERoneToMany),
                        (Arrow.ERzeroToMany, Arrow.ERoneToMany),
                        (Arrow.ERzeroToMany, Arrow.ERzeroToMany)
                    }
                }
            };

    List<Element> _entityElements = default!;
    List<Element> _propertyElements = default!;
    List<Element> _relationshipElements = default!;

    /// <summary>
    /// Gets the <see cref="Schema"></see> that will be used for the code
    /// generation.
    /// </summary>
    /// <returns>A <see cref="Schema"></see> that contains entities and
    /// relationships.</returns>
    public virtual Schema GetSchema()
    {
        var document = XDocument.Load(options.Value.SchemaFile);

        return BuildSchema(document);
    }

    Schema BuildSchema(XDocument document)
    {
        var schemaElement = GetSchemaElement(document);

        SetEntityElements(schemaElement);

        SetPropertyElements(schemaElement);

        SetRelationshipElements(schemaElement);

        var schema = new Schema();

        AddEntitiesToSchema(schema);

        AddRelationshipsToSchema(schema);

        return schema;
    }

    XElement GetSchemaElement(XDocument document)
    {
        var schemaElement = document.Descendants("diagram")
            .FirstOrDefault(e =>
                e.Attribute("name")!.Value == options.Value.SchemaName);

        if (schemaElement == null)
        {
            throw new ArgumentException(
                $"Schema '{options.Value.SchemaName}' not found.");
        }

        return schemaElement;
    }

    void SetEntityElements(XElement schemaElement)
    {
        _entityElements = schemaElement.Descendants("object")
            .Where(e => EntityFilter(e.Element("mxCell")!))
            .Select(ObjectSelector)
            .ToList();

        _entityElements.AddRange(schemaElement.Descendants("mxCell")
            .Where(e => e.Parent!.Name != "object")
            .Where(EntityFilter)
            .Select(MxCellSelector));
    }

    static bool EntityFilter(XElement e)
    {
        return e.Attribute("parent")?.Value == "1" &&
            !e.Attributes().Any(a => a.Name == "source") &&
            !e.Attributes().Any(a => a.Name == "target");
    }

    void SetPropertyElements(XElement schemaElement)
    {
        _propertyElements = schemaElement.Descendants("object")
            .Where(e => e.Descendants("mxCell")!
                .Single().Attribute("parent")!.Value != "1")
            .Select(ObjectSelector)
            .ToList();
    }

    void SetRelationshipElements(XElement schemaElement)
    {
        _relationshipElements = schemaElement.Descendants("object")
            .Where(e => RelationshipFilter(e.Element("mxCell")!))
            .Select(ObjectSelector)
            .ToList();

        _relationshipElements.AddRange(schemaElement.Descendants("mxCell")
            .Where(e => e.Parent!.Name != "object")
            .Where(RelationshipFilter)
            .Select(MxCellSelector));
    }

    static bool RelationshipFilter(XElement e)
    {
        return e.Attribute("parent")?.Value == "1" &&
            e.Attributes().Any(a => a.Name == "source") &&
            e.Attributes().Any(a => a.Name == "target");
    }

    static Func<XElement, Element> ObjectSelector =>
        e => new Element
        {
            Id = e.Attribute("id")!.Value,
            Name = e.Attribute("label")!.Value,
            Attributes = e.Attributes()
                .Concat(e.Element("mxCell")!.Attributes())
                .ToDictionary(a => a.Name.LocalName, a => a.Value)
        };

    static Func<XElement, Element> MxCellSelector =>
        e => new Element
        {
            Id = e.Attribute("id")!.Value,
            Name = e.Attribute("value")!.Value,
            Attributes = e.Attributes()
                .ToDictionary(a => a.Name.LocalName, a => a.Value)
        };

    void AddEntitiesToSchema(Schema schema)
    {
        foreach (var entityElement in _entityElements)
        {
            AddEntityToSchema(schema, entityElement);
        }

        ValidateEntityNames(schema);
    }

    void AddEntityToSchema(Schema schema, Element entityElement)
    {
        var entity = new Entity
        {
            Name = entityElement.Name
        };

        schema.Entities.Add(entity);

        entity.HasCustomConfiguration = ParseAttribute<bool>(
            entityElement, nameof(Entity.HasCustomConfiguration), entity);

        var propertyElements =
            GetPropertyElementsByEntityElementId(entityElement.Id);

        foreach (var propertyElement in propertyElements)
        {
            AddPropertyToEntity(entity, propertyElement);
        }

        ValidatePropertyNames(entity);

        AddIndexesToEntity(entityElement, entity);
    }

    IEnumerable<Element> GetPropertyElementsByEntityElementId(
        string entityElementId)
    {
        return _propertyElements
            .Where(e => e.Attributes["parent"] == entityElementId);
    }

    static void AddPropertyToEntity(Entity entity, Element propertyElement)
    {
        var property = new Property();

        entity.Properties.Add(property);

        property.Name = propertyElement.Name;

        property.DataType = ParseDataType(entity, property, propertyElement);

        property.Size = ParseAttribute<long>(
            propertyElement, nameof(Property.Size), entity, property);

        property.IsPrimaryKey = ParseAttribute<bool>(
            propertyElement, nameof(Property.IsPrimaryKey), entity, property);

        property.IsAutoGenerated = ParseAttribute<bool>(
            propertyElement, nameof(Property.IsAutoGenerated), entity, property);

        property.IsNullable = ParseAttribute<bool>(
            propertyElement, nameof(Property.IsNullable), entity, property);

        property.HasFixedSize = ParseAttribute<bool>(
            propertyElement, nameof(Property.HasFixedSize), entity, property);

        property.IsArray = ParseAttribute<bool>(
            propertyElement, nameof(Property.IsArray), entity, property);

        property.IsEnum = ParseAttribute<bool>(
            propertyElement, nameof(Property.IsEnum), entity, property);

        if (property.IsEnum)
        {
            property.EnumName =
                propertyElement.Attributes.ContainsKey(nameof(Property.EnumName)) ?
                propertyElement.Attributes[nameof(Property.EnumName)] :
                property.Name;

            property.EnumValues = ParseEnumValues(
                entity, property, propertyElement);
        }
    }

    static DataType ParseDataType(
        Entity entity, Property property, Element propertyElement)
    {
        var name = nameof(Property.DataType);

        if (!propertyElement.Attributes.TryGetValue(name, out var value))
        {
            ThrowMissingAttributeException(name, entity, property);
        }

        if (!Enum.TryParse<DataType>(value, out var dataType))
        {
            ThrowInvalidAttributeException(name, entity, property);
        }

        return dataType;
    }

    static void ThrowMissingAttributeException(
        string name, Entity entity, Property property)
    {
        throw new ArgumentException(
            $"{name} missing from '{entity.Name}.{property.Name}.");
    }

    static void ThrowInvalidAttributeException(
        string name, Entity entity, Property? property = null)
    {
        var propertyName = property == null ?
            "" :
            "." + property.Name;

        throw new ArgumentException(
            $"Invalid {name} value in '{entity.Name}{propertyName}.");
    }

    static T? ParseAttribute<T>(
        Element element, string name, Entity entity, Property? property = null)
    {
        if (!element.Attributes.TryGetValue(name, out var value))
        {
            return default;
        }

        var types = new Type[] { typeof(string), typeof(T).MakeByRefType() };

        var method = typeof(T).GetMethod("TryParse", types);

        var args = new object?[] { value, null };

        if (!(bool) method!.Invoke(null, args)!)
        {
            ThrowInvalidAttributeException(name, entity, property);
        }

        return (T?) args[1];
    }

    static IReadOnlyDictionary<string, long> ParseEnumValues(
        Entity entity, Property property, Element propertyElement)
    {
        // e.g. EnumValues="Viber=1,None=99"

        var name = nameof(Property.EnumValues);

        if (!propertyElement.Attributes.TryGetValue(name, out var value) ||
            string.IsNullOrWhiteSpace(value))
        {
            ThrowMissingAttributeException(name, entity, property);
        }

        var values = new Dictionary<string, long>();

        try
        {
            values = value!
                .Replace(" ", "")
                .Split(',')
                .Select(s =>
                {
                    var parts = s.Split('=');

                    return new KeyValuePair<string, long>(
                        parts[0], long.Parse(parts[1]));
                })
                .ToDictionary(p => p.Key, x => x.Value);
        }
        catch
        {
            ThrowInvalidAttributeException(name, entity, property);
        }

        return values;
    }

    static void ValidateEntityNames(Schema schema)
    {
        if (schema.Entities.Count >
            schema.Entities.DistinctBy(e => e.Name).Count())
        {
            throw new ArgumentException("Duplicate entity names found.");
        }
    }

    static void ValidatePropertyNames(Entity entity)
    {
        if (entity.Properties.Count >
            entity.Properties.DistinctBy(e => e.Name).Count())
        {
            throw new ArgumentException(
                $"Duplicate property names found for entity '{entity.Name}'.");
        }
    }

    static void AddIndexesToEntity(Element entityElement, Entity entity)
    {
        var indexAttributes = entityElement.Attributes
            .Where(p => p.Key.StartsWith("Index"));

        foreach (var indexAttribute in indexAttributes)
        {
            AddIndexToEntity(indexAttribute, entity);
        }
    }

    static void AddIndexToEntity(
        KeyValuePair<string, string> indexAttribute, Entity entity)
    {
        // e.g. Index01="OrganizationId,Name;Descending#True"

        try
        {
            var index = new Models.Index();

            entity.Indexes.Add(index);

            var indexParts = indexAttribute.Value.Split('#');

            if (indexParts.Length > 1)
            {
                index.IsUnique = Convert.ToBoolean(indexParts[1]);
            }

            var properties = indexParts[0].Split(',');

            foreach (var property in properties)
            {
                var indexProperty = new IndexProperty();

                index.Properties.Add(indexProperty);

                var propertyParts = property.Split(';');

                indexProperty.Name = propertyParts[0];

                if (propertyParts.Length > 1)
                {
                    indexProperty.IndexOrder =
                        Enum.Parse<IndexOrder>(propertyParts[1]);
                }
            }
        }
        catch
        {
            ThrowInvalidAttributeException(indexAttribute.Key, entity);
        }
    }

    void AddRelationshipsToSchema(Schema schema)
    {
        foreach (var relationshipElement in _relationshipElements)
        {
            AddRelationshipToSchema(schema, relationshipElement);
        }
    }

    void AddRelationshipToSchema(Schema schema, Element relationshipElement)
    {
        var style = relationshipElement.Attributes["style"];

        var relationship = new Relationship
        {
            RelationshipType = GetRelationshipType(style)
        };

        schema.Relationships.Add(relationship);

        var source = GetRelationshipPartByAttribute(relationshipElement, "source");

        var target = GetRelationshipPartByAttribute(relationshipElement, "target");

        if ((relationship.RelationshipType == RelationshipType.OneToOne &&
             ShouldSwitchOneToOneRelationshipParts(schema, source, target)) ||
            (relationship.RelationshipType == RelationshipType.ManyToMany &&
             ShouldSwitchManyToManyRelationshipParts(relationshipElement, source)))
        {
            (source, target) = (target, source);
        }

        relationship.PrincipalEntity = schema.Entities
            .Single(e => e.Name == source.Entity);

        relationship.PrincipalProperty = relationship.PrincipalEntity.Properties
            .Single(p => p.Name == source.Property);

        relationship.DependentEntity = schema.Entities
            .Single(e => e.Name == target.Entity);

        relationship.DependentProperty = relationship.DependentEntity.Properties
            .Single(p => p.Name == target.Property);

        if (relationship.RelationshipType == RelationshipType.ManyToMany)
        {
            AddIntermediateEntityToSchema(schema, source, target);
        }
    }

    RelationshipType GetRelationshipType(string style)
    {
        var startArrow = GetArrow(style, "startArrow");
        var endArrow = GetArrow(style, "endArrow");

        return _relationshipArrows
            .Where(ra => ra.Value.Contains((startArrow, endArrow!)))
            .Single().Key;
    }

    static string? GetArrow(string style, string arrow)
    {
        var arrowIndex = style.IndexOf(arrow);

        if (arrowIndex < 0)
        {
            return null;
        }

        var equalIndex = style.IndexOf('=', arrowIndex);

        var semicolonIndex = style.IndexOf(';', arrowIndex);

        return style.Substring(equalIndex + 1, semicolonIndex - equalIndex - 1);
    }

    (string Entity, string Property) GetRelationshipPartByAttribute(
        Element relationshipElement, string attribute)
    {
        var value = relationshipElement.Attributes[attribute];

        var propertyElement = _propertyElements.Single(e => e.Id == value);

        var entityElementId = propertyElement.Attributes["parent"];

        var entityElement = _entityElements.Single(e => e.Id == entityElementId);

        return (entityElement.Name, propertyElement.Name);
    }

    static bool ShouldSwitchOneToOneRelationshipParts(
        Schema schema,
        (string Entity, string Property) source,
        (string Entity, string Property) target)
    {
        // In one-to-one relationships the source (principal) could be saved
        // as target (dependent) in the document, and vice versa, depending on
        // the way it is designed by the user

        var sourcePropertyIsPK = RelationshipPartIsPrimaryKey(schema, source);

        var targetPropertyIsPK = RelationshipPartIsPrimaryKey(schema, target);

        return !sourcePropertyIsPK && targetPropertyIsPK;
    }

    static bool RelationshipPartIsPrimaryKey(
        Schema schema, (string Entity, string Property) relationshipPart)
    {
        return schema.Entities
            .Single(e => e.Name == relationshipPart.Entity).Properties
                .Single(p => p.Name == relationshipPart.Property).IsPrimaryKey;
    }

    static bool ShouldSwitchManyToManyRelationshipParts(
        Element relationshipElement, (string Entity, string Property) source)
    {
        // In many-to-many relationships it doesn't matter which end is the
        // source and which is the target. We select a source, if we want, just
        // for naming purposes

        if (!relationshipElement.Attributes.TryGetValue("Source", out var entity))
        {
            return false;
        }

        return source.Entity != entity;
    }

    static void AddIntermediateEntityToSchema(
        Schema schema,
        (string Entity, string Property) source,
        (string Entity, string Property) target)
    {
        schema.Entities.Add(new()
        {
            Name = source.Entity + target.Entity,
            Properties =
            [
                AddIntermediateProperty(schema, source),
                AddIntermediateProperty(schema, target)
            ]
        });
    }

    static Property AddIntermediateProperty(
        Schema schema, (string Entity, string Property) relationshipPart)
    {
        var entity = schema.Entities
            .Single(e => e.Name == relationshipPart.Entity);

        var entityPK = entity.Properties.Single(p => p.IsPrimaryKey);

        return new()
        {
            Name = entity.Name + "Id",
            DataType = entityPK.DataType,
            IsPrimaryKey = true
        };
    }
}
