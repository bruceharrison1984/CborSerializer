![Build Status](https://github.com/bruceharrison1984/CborSerializer/actions/workflows/dotnet.yml/badge.svg)

[Nuget Package Download](https://www.nuget.org/packages/CborSerializer/)

# CborSerializer
Open-ended CBOR serialization for .net5. This is just a thin wrapper around the System.Formats.Cbor namespace to allow for easier serialization of objects. The only dependency is `System.Formats.Cbor`.

## Usage
Usage is straight-forward.

Declare a class that will be serialized/deserialized:
```csharp
[CborSerialize(typeof(TestObjectConvertor))]
public class TestObject
{
    public int SomeInt { get; }
    public string SomeString { get; }
    public Tuple<int, bool> SomeTuple { get; }

    public TestObject(int someInt, string someString, Tuple<int,bool> someTuple)
    {
        SomeInt = someInt;
        SomeString = someString;
        SomeTuple = someTuple;
    }
}
```

Declare a CborConvertor for it:
```csharp
public class TestObjectConvertor : ICborConvertor<TestObject>
  {
      public TestObject Read(ref CborReader reader)
      {
          reader.ReadStartArray();
          var someInt = reader.ReadInt32();
          var someString = reader.ReadTextString();

          // in this example we convert the Tuple to an array, but your implementation
          // can be whatever you want
          reader.ReadStartArray();
          var tupleInt = reader.ReadInt32();
          var tupleBool = reader.ReadBoolean();

          return new TestObject(someInt, someString, new Tuple<int, bool>(tupleInt, tupleBool));
      }

      public void Write(ref CborWriter writer, TestObject value)
      {
            writer.WriteStartArray(2);
            writer.WriteInt32(value.SomeInt);
            writer.WriteTextString(value.SomeString);

            // in this example we convert the Tuple to an array, but your implementation
            // can be whatever you want
            writer.WriteStartArray(2);
            writer.WriteInt32(value.SomeTuple.Item1);
            writer.WriteInt32(value.SomeTuple.Item2);
            writer.WriteEndArray();

            writer.WriteEndArray();
      }
  }
```

Converting to/from serialized formats using the static methods:
```csharp
byte[] serializedPayload = CborConvertor.Serialize(new TestObject());
TestObject deserializedObject = CborConvertor.Deserialize<TestObject>(byteArray);
```

## Automatic Serialization
This library does not implement any type of automatic property mapping/serialization. All serialization must be manually implemented. My use case was a unique wire format and auto-mapping was not a good fit. This allows you to declare any wire format.
