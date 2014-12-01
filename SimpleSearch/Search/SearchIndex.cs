using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SimpleSearch.Core.Attributes;
using SimpleSearch.Core.Excpetions;
using SimpleSearch.Core.Helpers;
using SimpleSearch.Core.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Directory = Lucene.Net.Store.Directory;

namespace SimpleSearch.Core.Search
{
    public class SearchIndex<TAnalyzer, TSchema> : IDisposable
        where TAnalyzer : Analyzer
        where TSchema : BaseSchema, new()
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static SearchIndex<TAnalyzer, TSchema> _instance;

        /// <summary>
        /// Thread lock for below
        /// </summary>
        // ReSharper disable once StaticFieldInGenericType
        private static readonly object ThreadLock = new object();

        /// <summary>
        /// Private ctor
        /// </summary>
        private SearchIndex()
        {
        }

        /// <summary>
        /// Singleton implementation
        /// </summary>
        internal static SearchIndex<TAnalyzer, TSchema> Instance
        {
            get
            {
                lock (ThreadLock)
                {
                    return _instance
                           ?? (_instance = new SearchIndex<TAnalyzer, TSchema>());
                }
            }
        }

        /// <summary>
        /// Path on disk to index file
        /// </summary>
        internal string IndexDirectory { get; set; }

        /// <summary>
        /// Unlocks the index if it's locked. 
        /// A locked index will thow an error if an attempt is made at altering the index
        /// </summary>
        private static void Unlock(Directory directory)
        {
            if (IndexWriter.IsLocked(directory))
            {
                IndexWriter.Unlock(directory);
            }
        }

        /// <summary>
        /// Deleted the .lock file if it exists
        /// </summary>
        private void DeleteLockFileIfExists()
        {
            var path = Path.Combine(IndexDirectory, IndexWriter.WRITE_LOCK_NAME);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Returns a new instance of a Lucene directory
        /// </summary>
        private FSDirectory NewDirectory
        {
            get { return FSDirectory.Open(new DirectoryInfo(IndexDirectory)); }
        }

        private FSDirectory _indexDirectory;

        /// <summary>
        /// Unlocks index and deletes .lock file before retuning a Lucene index.
        /// </summary>
        internal FSDirectory Directory
        {
            get
            {
                var dir = _indexDirectory
                          ?? (_indexDirectory = NewDirectory);

                Unlock(dir);

                DeleteLockFileIfExists();

                return dir;
            }
        }

        /// <summary>
        /// Sets the schema type
        /// </summary>
        /// <typeparam name="TSchema"></typeparam>
        internal void SetSchema()
        {
            _typeSchema = typeof(TSchema);
        }

        /// <summary>
        /// Stortes the type of schema at init
        /// </summary>
        private Type _typeSchema;

        /// <summary>
        /// Checks to see that schema types match.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal bool SchemasMatch<T>()
            where T : BaseSchema, new()
        {
            return typeof(T) == _typeSchema;
        }

        /// <summary>
        /// Add a T to index
        /// Element must provide a parameterless public constructor.
        /// </summary>
        /// <param name="document"></param>
        public void AddUpdateLuceneIndex(TSchema document)
        {
            AddUpdateLuceneIndex(new[] {document});
        }

        /// <summary>
        /// Thread lock for below
        /// </summary>
        private readonly Object _threadLock = new Object();

        /// <summary>
        /// Add a collection of T to index
        /// Element must provide a parameterless public constructor.
        /// </summary>
        /// <param name="documents"></param>
        public void AddUpdateLuceneIndex(IEnumerable<TSchema> documents)
        {
            lock (_threadLock)
            {
                if (!SchemasMatch<TSchema>())
                {
                    throw new SchemaException();
                }

                using (var analyzer = typeof (TAnalyzer).Instantiate<TAnalyzer>())
                {
                    using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                    {
                        foreach (var document in documents)
                        {
                            var properties = typeof (TSchema).Properties();
                            var idField =
                                properties.FirstOrDefault(f => f.GetCustomAttribute<IdentifierAttribute>() != null);
                            Validate.NotNull(idField,
                                string.Format("{0} does not contain an identifier field.",
                                    typeof (TAnalyzer).FullName));

                            // ReSharper disable once PossibleNullReferenceException
                            var idValue = idField.GetValue(document);
                            Validate.NotNull(idValue,
                                string.Format("{0} does not contain a valid identifier field. Value was null",
                                    typeof (TAnalyzer).FullName));

                            var searchQuery = new TermQuery(new Term(idField.Name, idValue.ToString()));
                            writer.DeleteDocuments(searchQuery);

                            writer.AddDocument(document.ToDocument(), analyzer);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the total number of documents in a Index.
        /// </summary>
        /// <returns></returns>
        public int Length()
        {
            using (var analyzer = typeof (TAnalyzer).Instantiate<TAnalyzer>())
            {
                using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    return writer.NumDocs();
                }
            }
        }

        /// <summary>
        /// Deletes all elements within an index.
        /// </summary>
        /// <returns>True if index is deleted, False if something has gone wrong</returns>
        public void DeleteAll()
        {
            using (var analyzer = typeof (TAnalyzer).Instantiate<TAnalyzer>())
            {
                using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.DeleteAll();
                }
            }
        }

        /// <summary>
        /// Delete an index item by identifier
        /// </summary>
        /// <param name="document"></param>
        public void DeleteByIdentifier(TSchema document)
        {
            if (!SchemasMatch<TSchema>())
            {
                throw new SchemaException();
            }

            using (var analyzer = typeof (TAnalyzer).Instantiate<TAnalyzer>())
            {
                var properties = typeof (TSchema).GetProperties();
                var idField = properties.FirstOrDefault(f => f.GetCustomAttribute<IdentifierAttribute>() != null);
                Validate.NotNull(idField,
                    string.Format("{0} does not contain an identifier field.",
                        typeof (TAnalyzer).FullName));

                // ReSharper disable once PossibleNullReferenceException
                var idValue = idField.GetValue(document);
                Validate.NotNull(idValue,
                    string.Format("{0} does not contain a valid identifier field. Value was null",
                        typeof (TAnalyzer).FullName));

                using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    var searchQuery = new TermQuery(new Term(idField.Name, idValue.ToString()));

                    writer.DeleteDocuments(searchQuery);
                }
            }
        }

        /// <summary>
        /// Optimize the search index. 
        /// This should be run regularly to defrag index.
        /// Increases performance for indexes that are often updated.
        /// </summary>
        public void Optimize()
        {
            using (var analyzer = typeof (TAnalyzer).Instantiate<TAnalyzer>())
            {
                using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.Optimize();
                }
            }
        }

        /// <summary>
        /// IDisposable implementation
        /// </summary>
        public void Dispose()
        {
            _instance = null;
        }
    }
}