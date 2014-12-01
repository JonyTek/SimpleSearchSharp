using System;
using SimpleSearch.Core.Attributes;
using SimpleSearch.Core.Models;
using SimpleSearch.Core.Schema;

namespace SimpleSearch.Angular.Search
{
    public class Schema : BaseSchema
    {
        [Identifier]
        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public int Id { get; set; }

        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public string Heading { get; set; }

        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public string Body { get; set; }

        [Sortable("Date Created")]
        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public DateTime CreatedAt { get; set; }

        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public int CategoryId { get; set; }
        
        [Store(FieldStore.No)]
        [Analyze(FieldIndex.Analyzed)]
        public string AllText
        {
            get { return string.Format("{0} {1}", Heading, Body); }
        }
    }
}