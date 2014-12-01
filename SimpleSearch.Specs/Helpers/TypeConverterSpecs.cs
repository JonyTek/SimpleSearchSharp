using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleSearch.Core.Excpetions;
using SimpleSearch.Core.Helpers;
using SimpleSearch.Core.Models;
using System;

namespace SimpleSearch.Specs.Helpers
{    
    [TestClass]
    public class TypeConverterSpecs
    {
        private readonly Guid _guid = Guid.NewGuid();
        
        [TestMethod]
        public void ShouldTurnSchemaIemIntoDocument()
        {
            var schema = new MySchema
            {
                Id = 1,
                Text = "Some text",
                Date = new DateTime(2000, 1, 1),
                Long = 4294967296L,
                Guid = _guid,
                Enum = FieldIndex.Analyzed,
                NoStore = "YOOOOOOO"
            };
            
            var document = schema.ToDocument();

            document.GetField("Id").StringValue.Should().Be("1");
            document.GetField("Text").StringValue.Should().Be("Some text");
            var date = DateTime.ParseExact(document.GetField("Date").StringValue, Constants.QueryDateFormat, null);
            date.Should().Be(schema.Date);
        }

        [TestMethod]
        public void ShouldConvertADocumentBackToTypeT()
        {
            var schema = new MySchema
            {
                Id = 1,
                Text = "Some text",
                Date = new DateTime(2000, 1, 1),
                Long = 4294967296L,
                Guid = _guid,
                Enum = FieldIndex.Analyzed,
                NoStore = "YOOOOOOO"
            };
            
            var document = schema.ToDocument();

            var item = document.ToSchema<MySchema>();
            item.Id.Should().Be(1);
            item.Text.Should().Be("Some text");
            item.Date.Should().Be(new DateTime(2000, 1, 1));
            item.Long.Should().Be(4294967296L);
            item.Guid.Should().Be(_guid);
            item.Enum.Should().Be(FieldIndex.Analyzed);            
            item.NoStore.Should().BeNull();
        }

        [TestMethod]
        public void ShouldThrowAnException()
        {
            var schema = new MyExceptionSchema
            {
                Id = 1,
                Exception = new Exception()
            };

            var document = schema.ToDocument();
            Action action = () => document.ToSchema<MyExceptionSchema>();
            
            action.ShouldThrow<TypeException>();
        }
    }
}