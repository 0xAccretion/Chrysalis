using System.Formats.Cbor;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace Chrysalis.Cbor;

public static class CborSerializerV2
{
    public static byte[] Serialize(object obj)
    {
        var writer = new CborWriter(CborConformanceMode.Strict);
        SerializeObject(writer, obj, obj.GetType());
        return writer.Encode();
    }

    private static void SerializeObject(CborWriter writer, object obj, Type objType)
    {
        var cborTypeAttrs = (IEnumerable<CborTypeAttribute>?)Attribute.GetCustomAttributes(objType, typeof(CborTypeAttribute));

        if (cborTypeAttrs is not null && cborTypeAttrs.Any())
        {
            foreach (var cborTypeAttr in cborTypeAttrs)
            {
                try
                {
                    if (cborTypeAttr.IsBasicType)
                    {
                        // Use reflection to get the value of the 'Value' property
                        var valueProperty = objType.GetProperty("CborValue") ?? throw new InvalidOperationException("A basic type must have a 'Value' property.");
                        var value = valueProperty.GetValue(obj);
                        if (value == null)
                        {
                            throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                        }
                        SerializePrimitive(writer, value, value.GetType());
                        return;  // Skip the rest of the serialization steps
                    }
                    var representation = cborTypeAttr.Representation;
                    SerializePrimitive(writer, obj, objType, representation);
                    return;
                }
                catch
                {
                    continue;
                }
            }
            throw new NotImplementedException("Unknown CborRepresentation");
        }
        else
        {
            SerializePrimitive(writer, obj, objType);
        }
    }

    private static void SerializePrimitive(CborWriter writer, object obj, Type objType, CborRepresentation? overrideRepresentation = null)
    {
        if (overrideRepresentation != null)
        {
            switch (overrideRepresentation)
            {
                case CborRepresentation.Set:
                case CborRepresentation.Array:
                    SerializeArray(writer, obj, objType);
                    break;
                case CborRepresentation.Tuple:
                    SerializeTuple(writer, obj, objType);
                    break;
                case CborRepresentation.Map:
                    SerializeMap(writer, obj, objType);
                    break;
                case CborRepresentation.Record:
                    SerializeRecord(writer, obj, objType);
                    break;
                case CborRepresentation.Int32:
                    writer.WriteInt32((int)obj);
                    break;
                case CborRepresentation.Int64:
                    writer.WriteInt64((long)obj);
                    break;
                case CborRepresentation.UInt32:
                    writer.WriteUInt32((uint)obj);
                    break;
                case CborRepresentation.UInt64:
                    writer.WriteUInt64((ulong)obj);
                    break;
                case CborRepresentation.ByteString:
                    writer.WriteByteString(Convert.FromHexString((string)obj));
                    break;
                case CborRepresentation.Bool:
                    writer.WriteBoolean((bool)obj);
                    break;
                default:
                    throw new NotImplementedException("Unknown CborRepresentation");
            }
        }
        else
        {
            
            var cborPropAttrs = (IEnumerable<CborPropertyAttribute>?)Attribute.GetCustomAttributes(objType, typeof(CborPropertyAttribute));
            if (cborPropAttrs is not null && cborPropAttrs.Any())
            {
                foreach (var cborPropAttr in cborPropAttrs)
                {
                    try
                    {
                        SerializePrimitive(writer, obj, objType, cborPropAttr.ValueType);
                        return;
                    }
                    catch
                    {
                        continue;
                    }
                }
                throw new NotImplementedException("Unknown CborRepresentation");
            }
            else
            {
                switch (objType)
                {
                    case Type t when t == typeof(string):
                        writer.WriteByteString(Convert.FromHexString((string)obj));
                        break;
                    case Type t when t == typeof(int):
                        writer.WriteInt32((int)obj);
                        break;
                    case Type t when t == typeof(long):
                        writer.WriteInt64((long)obj);
                        break;
                    case Type t when t == typeof(uint):
                        writer.WriteUInt32((uint)obj);
                        break;
                    case Type t when t == typeof(ulong):
                        writer.WriteUInt64((ulong)obj);
                        break;
                    case Type t when t == typeof(bool):
                        writer.WriteBoolean((bool)obj);
                        break;
                    case Type t when t == typeof(int?):
                        writer.WriteInt32((int)obj);
                        break;
                    case Type t when t == typeof(long?):
                        writer.WriteInt64((long)obj);
                        break;
                    case Type t when t == typeof(uint?):
                        writer.WriteUInt32((uint)obj);
                        break;
                    case Type t when t == typeof(ulong?):
                        writer.WriteUInt64((ulong)obj);
                        break;
                    case Type t when t == typeof(bool?):
                        writer.WriteBoolean((bool)obj);
                        break;
                    default:
                        throw new NotImplementedException("Type not supported");
                }
            }
        }
    }

    private static void SerializeRecord(CborWriter writer, object obj, Type objType)
    {
        // Collect properties with CborPropertyAttribute and sort by index
        var properties = objType.GetProperties()
            .Select(prop => (prop, cborPropAttr: (CborPropertyAttribute?)Attribute.GetCustomAttribute(prop, typeof(CborPropertyAttribute))))
            .Where(x => x.cborPropAttr != null)
            .OrderBy(x => x.cborPropAttr!.IndexValue)
            .ToList();

        // Write the start of the map with the item count
        writer.WriteStartMap(properties.Count);

        foreach (var (prop, cborPropAttr) in properties)
        {
            // Serialize the index value as the key
            SerializeObject(writer, cborPropAttr!.IndexValue, cborPropAttr.IndexValue.GetType());

            // Get the property value
            object? value = prop.GetValue(obj);

            if (value != null)
            {
                // Serialize the property value
                SerializeObject(writer, value, prop.PropertyType);
            }
            else
            {
                // If the value is null, write a CBOR null value or handle as you see fit
                writer.WriteNull();
            }
        }

        // Write the end of the map
        writer.WriteEndMap();
    }

    private static void SerializeArray(CborWriter writer, object obj, Type objType)
    {
        if (typeof(IEnumerable).IsAssignableFrom(objType) && objType != typeof(string))
        {
            var enumerable = (IEnumerable)obj;
            writer.WriteStartArray(enumerable.Cast<object>().Count());

            foreach (var item in enumerable)
            {
                SerializeObject(writer, item, item.GetType());
            }

            writer.WriteEndArray();
        }
        else
        {
            throw new ArgumentException("Type must be an enumerable to be serialized as a CBOR array");
        }
    }

    private static void SerializeTuple(CborWriter writer, object obj, Type objType)
    {
        var sortedProperties = objType.GetProperties()
            .Select(prop => (prop, cborPropAttrs: (IEnumerable<CborPropertyAttribute>?)Attribute.GetCustomAttributes(prop, typeof(CborPropertyAttribute))))
            .Where(x => x.cborPropAttrs != null && x.cborPropAttrs.Any() && x.prop.GetValue(obj) != null)
            .OrderBy(x => x.cborPropAttrs!.First().IndexValue)
            .ToList();

        writer.WriteStartArray(sortedProperties.Count);

        foreach (var (prop, cborPropAttr) in sortedProperties)
        {
            object? value = prop.GetValue(obj);
            SerializeObject(writer, value!, value!.GetType());
        }

        writer.WriteEndArray();
    }

    private static void SerializeMap(CborWriter writer, object obj, Type objType)
    {
        if (typeof(IDictionary).IsAssignableFrom(objType))
        {
            // Logic for handling dictionary types
            IDictionary dictionary = (IDictionary)obj;
            writer.WriteStartMap(dictionary.Count);

            foreach (DictionaryEntry entry in dictionary)
            {
                SerializeObject(writer, entry.Key, entry.Key.GetType());

                if (entry.Value == null)
                {
                    throw new ArgumentNullException(nameof(entry.Value), "Dictionary entry value cannot be null.");
                }

                SerializeObject(writer, entry.Value, entry.Value.GetType());
            }
        }
        writer.WriteEndMap();
    }

    public static T? Deserialize<T>(byte[] cborData)
    {
        var reader = new CborReader(cborData, CborConformanceMode.Strict);
        return (T?)DeserializeObject(reader, typeof(T));
    }

    private static object? DeserializeObject(CborReader reader, Type targetType, CborRepresentation? overrideRepresentation = null)
    {
        var readState = reader.PeekState();
        var cborTypeAttrs = (IEnumerable<CborTypeAttribute>?)Attribute.GetCustomAttributes(targetType, typeof(CborTypeAttribute));
        var cborTypeAttr = cborTypeAttrs?.FirstOrDefault(x => ConvertCborRepresentationToState(x.Representation) == readState);
        var representationToUse = overrideRepresentation ?? cborTypeAttr?.Representation;

        if (cborTypeAttr != null)
        {
            try
            {
                if (cborTypeAttr.IsBasicType)
                {
                    // Assume there's a "Value" property and that its type matches the representation
                    var valueProperty = targetType.GetProperty("CborValue") ?? throw new InvalidOperationException("A basic type must have a 'Value' property.");
                    var value = DeserializePrimitive(reader, valueProperty.PropertyType, cborTypeAttr.Representation);

                    // Create an instance and set the 'Value' property
                    var instance = Activator.CreateInstance(targetType);
                    valueProperty.SetValue(instance, value);

                    return instance;
                }
                else
                {
                    return DeserializePrimitive(reader, targetType, representationToUse);
                }
            }
            catch
            {
                return null;
            }
        }
        else
        {
            // Fall back to primitive deserialization if no attributes are found
            return DeserializePrimitive(reader, targetType, representationToUse);
        }
    }

    private static object? DeserializePrimitive(CborReader reader, Type targetType, CborRepresentation? representation)
    {
        if (representation != null)
        {
            return representation switch
            {
                CborRepresentation.Set => DeserializeArray(reader, targetType, true),
                CborRepresentation.Array => DeserializeArray(reader, targetType),
                CborRepresentation.Tuple => DeserializeTuple(reader, targetType),
                CborRepresentation.Map => DeserializeMap(reader, targetType),
                CborRepresentation.Record => DeserializeRecord(reader, targetType),
                CborRepresentation.Int32 => reader.ReadInt32(),
                CborRepresentation.Int64 => reader.ReadInt64(),
                CborRepresentation.UInt32 => reader.ReadUInt32(),
                CborRepresentation.UInt64 => reader.ReadUInt64(),
                CborRepresentation.ByteString => Convert.ToHexString(reader.ReadByteString()).ToLowerInvariant(),
                CborRepresentation.Bool => reader.ReadBoolean(),
                _ => throw new NotImplementedException("Unknown CborRepresentation"),
            };
        }
        else
        {
            return targetType switch
            {
                Type t when t == typeof(int) => reader.ReadInt32(),
                Type t when t == typeof(long) => reader.ReadInt64(),
                Type t when t == typeof(uint) => reader.ReadUInt32(),
                Type t when t == typeof(ulong) => reader.ReadUInt64(),
                Type t when t == typeof(string) => Convert.ToHexString(reader.ReadByteString()).ToLowerInvariant(),
                Type t when t == typeof(bool) => reader.ReadBoolean(),
                Type t when t == typeof(int?) => reader.ReadInt32(),
                Type t when t == typeof(long?) => reader.ReadInt64(),
                Type t when t == typeof(uint?) => reader.ReadUInt32(),
                Type t when t == typeof(ulong?) => reader.ReadUInt64(),
                Type t when t == typeof(bool?) => reader.ReadBoolean(),
                _ => null
            };
        }
    }

    private static object? DeserializeRecord(CborReader reader, Type targetType)
    {
        if (reader.PeekState() != CborReaderState.StartMap)
        {
            throw new InvalidOperationException("Unexpected CBOR data format");
        }

        var itemCount = reader.ReadStartMap();
        object obj = Activator.CreateInstance(targetType)!;

        // Create a lookup dictionary that maps index values to PropertyInfo objects
        var properties = targetType.GetProperties()
        .Select(prop => (prop, cborPropAttr: (CborPropertyAttribute?)Attribute.GetCustomAttribute(prop, typeof(CborPropertyAttribute))))
        .Where(x => x.cborPropAttr != null)
        .ToDictionary(x => x.cborPropAttr!.IndexValue, x => (x.prop, x.cborPropAttr!.ValueType));

        for (int i = 0; i < itemCount; i++)
        {
            object indexValue = reader.ReadInt32();  // Assuming your Record keys are always integers. Adapt as necessary.

            if (properties.TryGetValue(indexValue, out var propertyInfo))
            {
                // Deserialize the property value.
                object? value = DeserializeObject(reader, propertyInfo.prop.PropertyType);
                propertyInfo.prop.SetValue(obj, value);
            }
            else
            {
                // Skip unknown key-value pair
                reader.SkipValue();
            }
        }

        reader.ReadEndMap();
        return obj;
    }

    private static object? DeserializeArray(CborReader reader, Type targetType, bool isSet = false)
    {
        if (!typeof(IEnumerable).IsAssignableFrom(targetType))
        {
            throw new ArgumentException("Type must be an enumerable to be deserialized as a CBOR array");
        }

        Type collectionType = targetType != null && targetType!.IsGenericType ? targetType : targetType!.BaseType!;
        if (collectionType == null || collectionType.GetGenericArguments().Length == 0)
        {
            throw new ArgumentException("Type or its base type must be generic.");
        }

        Type itemType = collectionType.GetGenericArguments()[0];

        if (isSet)
        {
            // create a hashset instnace of setType<itemType>
            var set = Activator.CreateInstance(targetType);

            reader.ReadStartArray();
            while (reader.PeekState() != CborReaderState.EndArray)
            {
                object? item = DeserializeObject(reader, itemType);
                targetType.GetMethod("Add")!.Invoke(set, [item]);
            }
            reader.ReadEndArray();

            return set;
        }
        else
        {
            IList? list = (IList?)Activator.CreateInstance(targetType);

            reader.ReadStartArray();
            while (reader.PeekState() != CborReaderState.EndArray)
            {
                object? item = DeserializeObject(reader, itemType);
                list?.Add(item);
            }
            reader.ReadEndArray();

            return list;
        }
    }

    private static object? DeserializeTuple(CborReader reader, Type targetType)
    {
        reader.ReadStartArray();

        object? instance = Activator.CreateInstance(targetType);

        targetType.GetProperties().ToList()
            .ForEach(prop =>
            {
                // Take into account that there might be multiple CborPropertyAttributes on the same property
                var readState = reader.PeekState();
                var cborPropAttrs = (CborPropertyAttribute[])Attribute.GetCustomAttributes(prop, typeof(CborPropertyAttribute));
                if (cborPropAttrs.Any() && !IsEndState(readState))
                {
                    var cborPropAttr = cborPropAttrs.FirstOrDefault(x => ConvertCborRepresentationToState(x.ValueType) == readState);
                    var deserializedValue = DeserializeObject(reader, prop.PropertyType, cborPropAttr?.ValueType);
                    prop.SetValue(instance, deserializedValue);
                }
            });

        reader.ReadEndArray();

        return instance;
    }

    private static object? DeserializeMap(CborReader reader, Type targetType)
    {
        if (reader.PeekState() != CborReaderState.StartMap)
        {
            throw new InvalidOperationException("Unexpected CBOR data format");
        }

        var itemCount = reader.ReadStartMap();
        object? obj = null;

        Type[] genericTypes = targetType.BaseType?.GetGenericArguments() ?? new Type[0];
        if (genericTypes.Length >= 2)
        {
            Type keyType = genericTypes[0];
            Type valueType = genericTypes[1];

            obj = Activator.CreateInstance(targetType);
            if (obj == null)
            {
                throw new InvalidOperationException($"Could not create an instance of {targetType}");
            }

            IDictionary dictionary = (IDictionary)obj;

            for (int i = 0; i < itemCount; i++)
            {
                object? key = DeserializeObject(reader, keyType);
                object? value = DeserializeObject(reader, valueType);

                if (key != null)
                {
                    dictionary.Add(key, value);
                }
            }
        }

        reader.ReadEndMap();
        return obj;
    }

    private static CborReaderState ConvertCborRepresentationToState(CborRepresentation representation)
    {
        return representation switch
        {
            CborRepresentation.Int32 => CborReaderState.UnsignedInteger,
            CborRepresentation.Int64 => CborReaderState.UnsignedInteger,
            CborRepresentation.UInt32 => CborReaderState.UnsignedInteger,
            CborRepresentation.UInt64 => CborReaderState.UnsignedInteger,
            CborRepresentation.ByteString => CborReaderState.ByteString,
            CborRepresentation.Bool => CborReaderState.Boolean,
            CborRepresentation.Tuple => CborReaderState.StartArray,
            CborRepresentation.Set => CborReaderState.StartArray,  // Assuming sets are also represented as arrays
            CborRepresentation.Array => CborReaderState.StartArray,
            CborRepresentation.Record => CborReaderState.StartMap,
            CborRepresentation.Map => CborReaderState.StartMap,
            _ => throw new ArgumentException("Invalid CborRepresentation"),
        };
    }

    private static bool IsEndState(CborReaderState state)
    {
        return state == CborReaderState.EndArray || state == CborReaderState.EndMap;
    }



    public static string ToHex(object obj)
    {
        byte[] cborData = Serialize(obj);
        return Convert.ToHexString(cborData).ToLowerInvariant();
    }

    public static T? FromHex<T>(string hexString)
    {
        byte[] cborData = Convert.FromHexString(hexString);
        return Deserialize<T>(cborData);
    }
}
