using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Ubik.Infra.Contracts;

namespace Ubik.Infra.Ext
{
    public static class SerializationExtensions
    {
        /// <summary>
        /// Function to serialize object to xml string using the XmlSerializer
        /// </summary>
        /// <param name="objectInstance">ISerializable: the object to serialize</param>
        /// <returns>string: the xml string with the objects public properties</returns>
        public static string GetXmlString(this object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

        public static string GetXmlString(this object objectInstance, Type[] types)
        {
            var serializer = new XmlSerializer(objectInstance.GetType(), types);
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Deserialize an object from xml string
        /// </summary>
        /// <typeparam name="T">Type: the target of invocation</typeparam>
        /// <param name="objectData">string: Xml string</param>
        /// <returns>Object of Type T</returns>
        public static T XmlDeserializeFromString<T>(this string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        /// <summary>
        /// Deserialize an object from xml string
        /// </summary>
        /// <param name="objectData">string: Xml string</param>
        /// <param name="type">Type: the target of invocation</param>
        /// <returns>Object of Type type</returns>
        public static object XmlDeserializeFromString(this string objectData, Type type)
        {
            if (String.IsNullOrWhiteSpace(objectData)) return null;
            var serializer = new XmlSerializer(type);

            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        /// <summary>
        /// Return a string representation of the buffer data
        /// using  <see cref="System.Text.Encoding.UTF8"/>
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public static string ToUTF8(this byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return string.Empty;
            return System.Text.Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// Converts a string to a binary value
        /// using UTF8 Encoding
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>buffer</returns>
        public static byte[] ConvertUTF8ToBinary(this string str)
        {
            var encoding = new UTF8Encoding();
            return encoding.GetBytes(str);
        }


        public static string ToJsonString(this ICanBeJsonString serializable)
        {
            return JsonConvert.SerializeObject(serializable);
        }


        public static object FromJsonString(this string data)
        {
          return  JsonConvert.DeserializeObject(data);
        }

        public static TClass FromJsonString<TClass>(this string data) where TClass :class
        {
            var obj = JsonConvert.DeserializeObject<TClass>(data);
            return obj as TClass;
        }

    }
}