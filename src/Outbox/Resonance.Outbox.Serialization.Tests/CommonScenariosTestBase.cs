using System;
using AutoFixture;
using Xunit;

namespace Resonance.Outbox.Serialization.Tests
{
    public abstract class CommonScenariosTestBase<TSerializer> 
        where TSerializer : class, IMessageSerializer
    {
        protected readonly TSerializer Serializer;

        protected CommonScenariosTestBase(TSerializer serializer)
        {
            Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        [Fact]
        public void SerializesSimpleReferenceTypesProperly()
        {
            ArrangeSerializeDeserializeAndAssertEquality<CommonScenarioTypes.ReferenceType>();
        }

        [Fact]
        public void SerializesSimpleCollectionsProperly()
        {
            ArrangeSerializeDeserializeAndAssertEquality<CommonScenarioTypes.ReferenceTypeWithCollections>();
        }

        [Fact]
        public void SerializesComplexReferenceTypeProperly()
        {
            ArrangeSerializeDeserializeAndAssertEquality<CommonScenarioTypes.ComplexReferenceType>();
        }

        [Fact]
        public void SerializesComplexReferenceTypeWithComplexCollectionsProperly()
        {
            ArrangeSerializeDeserializeAndAssertEquality<CommonScenarioTypes.ComplexReferenceTypeWithComplexCollections>();
        }

        [Fact]
        public void SerializesMostConcreteTypeWhenGivenInterfaceProperly()
        {
            CommonScenarioTypes.ISimpleInterface message = new Fixture().Create<CommonScenarioTypes.ReferenceTypeImplementingInterface>();
            var serializedAndDeserializedMessage = Serializer.Deserialize(message.GetType(), Serializer.Serialize(message));

            Assert.Equal(message, serializedAndDeserializedMessage);
        }

        private void ArrangeSerializeDeserializeAndAssertEquality<TTestedType>()
        {
            var message = new Fixture().Create<TTestedType>();
            var serializedAndDeserializedMessage = Serializer.Deserialize(message.GetType(), Serializer.Serialize(message));

            Assert.Equal(message, serializedAndDeserializedMessage);
        }
    }
}
