using System.Formats.Cbor;
using System.Reflection;

namespace Chrysalis.Cbor;

public static class CborSerializerV2
{
    public static byte[] Serialize(object obj)
    {
        Type objType = obj.GetType();
        var writer = new CborWriter(CborConformanceMode.Strict);
        var cborTypeAttr = (CborTypeAttribute)Attribute.GetCustomAttribute(objType, typeof(CborTypeAttribute))!;
        if (cborTypeAttr != null)
        {
            switch (cborTypeAttr.CborRepresentation)
            {
                case CborRepresentation.Array:
                    SerializeArray(writer, obj, objType);
                    break;
            }
        }

        return writer.Encode();
    }

    private static void SerializeArray(CborWriter writer, object obj, Type objType)
    {
        PropertyInfo[] properties = objType.GetProperties();

        // Sort the properties by their CborProperty attribute index value
        var sortedProperties = new SortedDictionary<int, PropertyInfo>();
        foreach (var prop in properties)
        {
            var cborPropAttr = (CborPropertyAttribute)Attribute.GetCustomAttribute(prop, typeof(CborPropertyAttribute))!;
            if (cborPropAttr != null && cborPropAttr.IndexType == CborRepresentation.Int32)
            {
                sortedProperties.Add((int)cborPropAttr.IndexValue, prop);
            }
        }

        writer.WriteStartArray(sortedProperties.Count);

        // Serialize properties in sorted order
        foreach (var prop in sortedProperties.Values)
        {
            var cborPropAttr = (CborPropertyAttribute)Attribute.GetCustomAttribute(prop, typeof(CborPropertyAttribute))!;

            object value = prop.GetValue(obj)!;
            if (cborPropAttr.ValueType == CborRepresentation.Int32)
            {
                writer.WriteInt32((int)value);
            }
            else if (cborPropAttr.ValueType == CborRepresentation.ByteString)
            {
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes((string)value);
                writer.WriteByteString(byteArray);
            }
        }

        writer.WriteEndArray();
    }


    public static T Deserialize<T>(byte[] cborData)
    {
        Type targetType = typeof(T);
        var reader = new CborReader(cborData, CborConformanceMode.Strict);
        var cborTypeAttr = (CborTypeAttribute)Attribute.GetCustomAttribute(targetType, typeof(CborTypeAttribute))!;
        if (cborTypeAttr != null)
        {
            return cborTypeAttr.CborRepresentation switch
            {
                CborRepresentation.Array => (T)DeserializeArray(reader, targetType),
                _ => throw new InvalidOperationException($"Unsupported CBOR representation: {cborTypeAttr.CborRepresentation}"),
            };
        }
        else
        {
            throw new InvalidOperationException("The target type does not have a CborType attribute.");
        }
    }

    private static object DeserializeArray(CborReader reader, Type objType)
    {
        PropertyInfo[] properties = objType.GetProperties();
        object obj = Activator.CreateInstance(objType)!;
        reader.ReadStartArray();
        int index = 0;
        while (reader.PeekState() != CborReaderState.EndArray)
        {
            foreach (var prop in properties)
            {
                var cborPropAttr = (CborPropertyAttribute)Attribute.GetCustomAttribute(prop, typeof(CborPropertyAttribute))!;
                if (cborPropAttr != null && cborPropAttr.IndexType == CborRepresentation.Int32 && (int)cborPropAttr.IndexValue == index)
                {
                    if (cborPropAttr.ValueType == CborRepresentation.Int32)
                    {
                        int value = reader.ReadInt32();
                        prop.SetValue(obj, value);
                    }
                    else if (cborPropAttr.ValueType == CborRepresentation.ByteString)
                    {
                        byte[] byteValue = reader.ReadByteString();
                        string stringValue = System.Text.Encoding.UTF8.GetString(byteValue);
                        prop.SetValue(obj, stringValue);
                    }
                    break;
                }
            }

            index++;
        }

        reader.ReadEndArray();

        return obj;
    }
}
