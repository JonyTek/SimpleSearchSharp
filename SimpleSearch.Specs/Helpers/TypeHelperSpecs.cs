using System;
using FluentAssertions;
using Lucene.Net.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleSearch.Core.Helpers;

namespace SimpleSearch.Specs.Helpers
{
    [TestClass]
    public class TypeHelperSpecs
    {
        [TestMethod]
        public void ShouldReturnTrueForValidTypes()
        {
            TypeHelper.IsValidType(typeof(string)).Should().BeTrue();
            TypeHelper.IsValidType(typeof(long)).Should().BeTrue();
            TypeHelper.IsValidType(typeof(DateTime)).Should().BeTrue();
            TypeHelper.IsValidType(typeof(bool)).Should().BeTrue();
            TypeHelper.IsValidType(typeof(uint)).Should().BeTrue();
            TypeHelper.IsValidType(typeof(Field.Store)).Should().BeTrue();
        }

        [TestMethod]
        public void ShouldReturnFalseForInvalidTypes()
        {
            TypeHelper.IsValidType(typeof(Field)).Should().BeFalse();
            TypeHelper.IsValidType(typeof(TypeHelperSpecs)).Should().BeFalse();
        }
    }
}