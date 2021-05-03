using System;
using System.Formats.Cbor;
using System.Linq;
using System.Reflection;

namespace CborSerialization
{
    public static class CborConvertor
    {
        /// <summary>
        /// Serialize the supplied object with the associated IConvertor<T>>
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="cborObject">Object to be serialized</param>
        /// <param name="cborConformanceMode">The CBOR spec that should be conformed to (Default is Lax)</param>
        /// <param name="allowMultipleRootLevelValues">Should multiple root level values be allowed (Default is false)</param>
        /// <returns>Byte array representation of the serialized object</returns>
        public static byte[] Serialize<T>(T cborObject, CborConformanceMode cborConformanceMode = CborConformanceMode.Lax, bool convertIndefiniteLengthEncodings = false, bool allowMultipleRootLevelValues = false)
        {
            var convertor = (ICborConvertor<T>)GetConvertor(typeof(T));
            var writer = new CborWriter(cborConformanceMode, convertIndefiniteLengthEncodings, allowMultipleRootLevelValues);
            convertor.Write(ref writer, cborObject);
            return writer.Encode();
        }

        /// <summary>
        /// Serialize the supplied object with the associated IConvertor<T>>
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="cborObject">Object to be serialized</param>
        /// <param name="cborObject">Buffer that the serialized object will be written in to</param>
        /// <param name="cborConformanceMode">The CBOR spec that should be conformed to (Default is Lax)</param>
        /// <param name="allowMultipleRootLevelValues">Should multiple root level values be allowed (Default is false)</param>
        /// <returns>Number of bytes written to the buffer</returns>
        public static int Serialize<T>(T cborObject, byte[] buffer, CborConformanceMode cborConformanceMode = CborConformanceMode.Lax, bool convertIndefiniteLengthEncodings = false, bool allowMultipleRootLevelValues = false)
        {
            var convertor = (ICborConvertor<T>)GetConvertor(typeof(T));
            var writer = new CborWriter(cborConformanceMode, convertIndefiniteLengthEncodings, allowMultipleRootLevelValues);
            convertor.Write(ref writer, cborObject);
            return writer.Encode(buffer);
        }

        /// <summary>
        /// Deserialize a byte array in to the specified type
        /// </summary>
        /// <typeparam name="T">Type to deserialize the byte array in to</typeparam>
        /// <param name="cborObject">Byte array representation of the serialized object</param>
        /// <param name="cborConformanceMode">The CBOR spec that should be conformed to (Default is Lax)</param>
        /// <param name="allowMultipleRootLevelValues">Should multiple root level values be allowed (Default is false)</param>
        /// <returns>The deserialized object</returns>
        public static T Deserialize<T>(byte[] cborObject, CborConformanceMode cborConformanceMode = CborConformanceMode.Lax, bool allowMultipleRootLevelValues = false)
        {
            var convertor = (ICborConvertor<T>)GetConvertor(typeof(T));
            var reader = new CborReader(cborObject, cborConformanceMode, allowMultipleRootLevelValues);
            return convertor.Read(ref reader);
        }

        private static object GetConvertor(Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            if(!type.GetCustomAttributes(typeof(CborSerialize)).Any()) throw new CborException($"Type {type.Name} does not have a CborSerialize attribute");

            var serilizerAttr = (CborSerialize)type.GetCustomAttributes(typeof(CborSerialize)).First();
            var convertor = Activator.CreateInstance(serilizerAttr.ConvertorType);

            return convertor;
        }
    }
}
