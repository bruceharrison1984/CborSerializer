using System;

namespace CborSerialization
{
    /// <summary>
    /// Associate an ICovertor with a class or struct
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class CborSerialize : Attribute
    {
        public Type ConvertorType { get; }

        public CborSerialize(Type convertorType)
        {
            ConvertorType = convertorType;
        }
    }
}
