using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleSearch.Core.Attributes;
using SimpleSearch.Core.Helpers;
using SimpleSearch.Core.Schema;

namespace SimpleSearch.Specs.Helpers
{
    public class SortSchema : BaseSchema
    {
        [Sortable("Display Name")]
        public int DisplayName { get; set; }
    }

    [TestClass]
    public class SortBuilderSpecs
    {
        [TestMethod]
        public void ShouldGetAllFieldsWithSortableAttributesAttributes()
        {
            var builder = SortBuilder<SortSchema>.Instance;
            var fields = builder.GetSortableFields();
            
            fields.Count.Should().Be(2);
            fields.Skip(1).Take(1).First().Key.Should().Be("DisplayName");
            fields.Skip(1).Take(1).First().Value.Should().Be("Display Name");
        }
    }
}
