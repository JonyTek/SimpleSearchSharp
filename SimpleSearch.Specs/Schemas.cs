using System;
using SimpleSearch.Core.Attributes;
using SimpleSearch.Core.Models;
using SimpleSearch.Core.Schema;

namespace SimpleSearch.Specs
{
    public class MySchema : BaseSchema
    {
        [Identifier]
        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public int Id { get; set; }

        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public string Text { get; set; }

        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public DateTime Date { get; set; }

        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public long Long { get; set; }

        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public Guid Guid { get; set; }

        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public FieldIndex Enum { get; set; }

        //Elements that are specfied as no store do not get returned.
        [Store(FieldStore.No)]
        [Analyze(FieldIndex.Analyzed)]
        public string NoStore { get; set; }
    }

    public class MyExceptionSchema : BaseSchema
    {
        [Identifier]
        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public int Id { get; set; }

        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public Exception Exception { get; set; }
    }

    public class BasicSchema : BaseSchema
    {
        [Identifier]
        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public int Id { get; set; }

        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public string Text { get; set; }
    }

    public class BasicSchema1 : BaseSchema
    {
        [Identifier]
        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public int Id { get; set; }

        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public string Text { get; set; }
    }

    public class BasicSchema2 : BaseSchema
    {
        [Identifier]
        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public int Id { get; set; }

        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public string Text { get; set; }

        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public string Text1 { get; set; }

        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public DateTime Date { get; set; }

        [Store(FieldStore.Yes)]
        [Analyze(FieldIndex.Analyzed)]
        public int CategoryId { get; set; }
    }
}