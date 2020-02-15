using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Resonance.Outbox.Serialization.Tests
{
    public class CommonScenarioTypes
    {
        [DataContract]
        public class ReferenceType
        {
            [DataMember]
            public Guid Guid { get; set; }
            [DataMember]
            public Guid? NullableGuid { get; set; }
            [DataMember]
            public string String { get; set; }
            [DataMember]
            public bool Boolean { get; set; }
            [DataMember]
            public bool? BooleanFlag { get; set; }
            [DataMember]
            public byte Int8 { get; set; }
            [DataMember]
            public byte? NullableInt8 { get; set; }
            [DataMember]
            public short Int16 { get; set; }
            [DataMember]
            public short? NullableInt16 { get; set; }
            [DataMember]
            public int Int32 { get; set; }
            [DataMember]
            public int? NullableInt32 { get; set; }
            [DataMember]
            public long Int64 { get; set; }
            [DataMember]
            public long? NullableInt65 { get; set; }
            [DataMember]
            public decimal FloatingBinaryPoint32 { get; set; }
            [DataMember]
            public decimal? NullableFloatingBinaryPoint32 { get; set; }
            [DataMember]
            public decimal FloatingBinaryPoint64 { get; set; }
            [DataMember]
            public decimal? NullableFloatingBinaryPoint64 { get; set; }
            [DataMember]
            public decimal FloatingDecimalPoint128 { get; set; }
            [DataMember]
            public decimal? NullableFloatingDecimalPoint128 { get; set; }

            public override bool Equals(object obj)
            {
                return obj is ReferenceType type &&
                       type.Guid.Equals(Guid) &&
                       EqualityComparer<Guid?>.Default.Equals(NullableGuid, type.NullableGuid) &&
                       String == type.String &&
                       Boolean == type.Boolean &&
                       EqualityComparer<bool?>.Default.Equals(BooleanFlag, type.BooleanFlag) &&
                       Int8 == type.Int8 &&
                       EqualityComparer<byte?>.Default.Equals(NullableInt8, type.NullableInt8) &&
                       Int16 == type.Int16 &&
                       EqualityComparer<short?>.Default.Equals(NullableInt16, type.NullableInt16) &&
                       Int32 == type.Int32 &&
                       EqualityComparer<int?>.Default.Equals(NullableInt32, type.NullableInt32) &&
                       Int64 == type.Int64 &&
                       EqualityComparer<long?>.Default.Equals(NullableInt65, type.NullableInt65) &&
                       FloatingBinaryPoint32 == type.FloatingBinaryPoint32 &&
                       EqualityComparer<decimal?>.Default.Equals(NullableFloatingBinaryPoint32, type.NullableFloatingBinaryPoint32) &&
                       FloatingBinaryPoint64 == type.FloatingBinaryPoint64 &&
                       EqualityComparer<decimal?>.Default.Equals(NullableFloatingBinaryPoint64, type.NullableFloatingBinaryPoint64) &&
                       FloatingDecimalPoint128 == type.FloatingDecimalPoint128 &&
                       EqualityComparer<decimal?>.Default.Equals(NullableFloatingDecimalPoint128, type.NullableFloatingDecimalPoint128);
            }
        }

        [DataContract]
        public class ReferenceTypeWithCollections
        {
            [DataMember]
            public List<int?> NullableInt32List { get; set; }
            [DataMember]
            public List<string> StringList { get; set; }
            [DataMember]
            public int?[] NullableInt32Array { get; set; }
            [DataMember]
            public string[] StringArray { get; set; }
            [DataMember]
            public Dictionary<int, int?> Int32ToNullableInt32Dictionary { get; set; }
            [DataMember]
            public Dictionary<int, string> Int32ToStringDictionary { get; set; }
            [DataMember]
            public Dictionary<int, int?> GuidToNullableInt32Dictionary { get; set; }
            [DataMember]
            public Dictionary<int, string> GuidToStringDictionary { get; set; }

            public override bool Equals(object obj)
            {
                return obj is ReferenceTypeWithCollections collections &&
                       collections.NullableInt32List.SequenceEqual(NullableInt32List) &&
                       collections.StringList.SequenceEqual(StringList) &&
                       collections.NullableInt32Array.SequenceEqual(NullableInt32Array) &&
                       collections.StringArray.SequenceEqual(StringArray) &&
                       //This is naive but should be sufficient for simple test
                       collections.Int32ToNullableInt32Dictionary.OrderBy(kvp => kvp.Key)
                           .SequenceEqual(Int32ToNullableInt32Dictionary.OrderBy(kvp => kvp.Key)) &&
                       collections.Int32ToStringDictionary.OrderBy(kvp => kvp.Key)
                           .SequenceEqual(Int32ToStringDictionary.OrderBy(kvp => kvp.Key)) &&
                       collections.GuidToNullableInt32Dictionary.OrderBy(kvp => kvp.Key)
                           .SequenceEqual(GuidToNullableInt32Dictionary.OrderBy(kvp => kvp.Key)) &&
                       collections.GuidToStringDictionary.OrderBy(kvp => kvp.Key)
                           .SequenceEqual(GuidToStringDictionary.OrderBy(kvp => kvp.Key));
            }
        }

        [DataContract]
        public class ComplexReferenceType
        {
            [DataMember]
            public string String { get; set; }
            [DataMember]
            public bool? NullableBoolean { get; set; }
            [DataMember]
            public List<string> StringList { get; set; }
            [DataMember]
            public ReferenceType ReferenceType { get; set; }
            [DataMember]
            public ReferenceTypeWithCollections ReferenceTypeWithCollections { get; set; }

            public override bool Equals(object obj)
            {
                return obj is ComplexReferenceType type &&
                       String == type.String &&
                       EqualityComparer<bool?>.Default.Equals(NullableBoolean, type.NullableBoolean) &&
                       type.StringList.SequenceEqual(StringList) &&
                       EqualityComparer<ReferenceType>.Default.Equals(ReferenceType, type.ReferenceType) &&
                       EqualityComparer<ReferenceTypeWithCollections>.Default.Equals(ReferenceTypeWithCollections, type.ReferenceTypeWithCollections);
            }
        }

        [DataContract]
        public class ComplexReferenceTypeWithComplexCollections : ComplexReferenceType
        {
            [DataMember]
            public List<ReferenceType> ReferenceTypesList { get; set; }
            [DataMember]
            public Dictionary<int, ReferenceType> Int32ToReferenceTypeDictionary { get; set; }
            [DataMember]
            public Dictionary<int, ReferenceType> GuidToReferenceTypeDictionary { get; set; }

            public override bool Equals(object obj)
            {
                return obj is ComplexReferenceTypeWithComplexCollections collections &&
                       base.Equals(obj) &&
                       collections.ReferenceTypesList.SequenceEqual(ReferenceTypesList) &&
                       collections.Int32ToReferenceTypeDictionary.OrderBy(kvp => kvp.Key)
                           .SequenceEqual(Int32ToReferenceTypeDictionary.OrderBy(kvp => kvp.Key)) &&
                       collections.GuidToReferenceTypeDictionary.OrderBy(kvp => kvp.Key)
                           .SequenceEqual(GuidToReferenceTypeDictionary.OrderBy(kvp => kvp.Key));
            }
        }

        public interface ISimpleInterface
        {
            Guid Identifier { get; set; }
            string Text { get; set; }
            int Number { get; set; }
        }

        [DataContract]
        public class ReferenceTypeImplementingInterface : ReferenceType, ISimpleInterface
        {
            [DataMember]
            public Guid Identifier { get; set; }
            [DataMember]
            public string Text { get; set; }
            [DataMember]
            public int Number { get; set; }

            public override bool Equals(object obj)
            {
                return obj is ReferenceTypeImplementingInterface @interface &&
                       base.Equals(obj) &&
                       Identifier.Equals(@interface.Identifier) &&
                       Text == @interface.Text &&
                       Number == @interface.Number;
            }
        }
    }
}
