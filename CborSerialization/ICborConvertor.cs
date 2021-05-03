using System.Formats.Cbor;

namespace CborSerialization
{
    /// <summary>
    /// Interface for creating CborConvertor
    /// </summary>
    /// <typeparam name="T">Object type the covertor is for</typeparam>
    public interface ICborConvertor<T>
    {
        public T Read(ref CborReader reader);
        public void Write(ref CborWriter writer, T value);
    }
}
