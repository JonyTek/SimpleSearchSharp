using System.Collections.Generic;
using SimpleSearch.Core.Models;
using SimpleSearch.Core.Schema;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SimpleSearch.Core.Helpers
{
    public class QueryBuilder<TSchema> : IDisposable
        where TSchema : BaseSchema, new()
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static QueryBuilder<TSchema> _instance;

        /// <summary>
        /// Thread lock for below
        /// </summary>
        //ReSharper disable once StaticFieldInGenericType
        private static readonly object ThreadLock = new object();

        /// <summary>
        /// Private ctor
        /// </summary>
        private QueryBuilder()
        {
            _queryBuilder = new StringBuilder();
        }

        /// <summary>
        /// Singleton implementation
        /// </summary>
        internal static QueryBuilder<TSchema> Instance
        {
            get
            {
                lock (ThreadLock)
                {
                    return _instance
                           ?? (_instance = new QueryBuilder<TSchema>());
                }
            }
        }

        private StringBuilder _queryBuilder;

        /// <summary>
        /// The specified field must contain the phrase specified
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> ContainsPhrase<TProperty>(Expression<Func<TSchema, TProperty>> expression, string phrase)
        {
            Validate.NotNull(expression, "No expression provided");
            Validate.StringNotEmpty(phrase, "No phrase provided");

            _queryBuilder.AppendFormat("({0}:\"{1}\") ",
                expression.FieldName(),
                phrase.ToLower());

            return this;
        }

        /// <summary>
        /// Creates a AND phrase query
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> AndContainsPhrase<TProperty>(Expression<Func<TSchema, TProperty>> expression, string phrase)
        {
            Validate.NotNull(expression, "No expression provided");
            Validate.StringNotEmpty(phrase, "No phrase provided");

            _queryBuilder =
                new StringBuilder(string.Format("({0}AND ({1}:\"{2}\")) ",
                    _queryBuilder, 
                    expression.FieldName(), 
                    phrase.ToLower()));

            return this;
        }

        /// <summary>
        /// Create a OR phrase query
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> OrContainsPhrase<TProperty>(Expression<Func<TSchema, TProperty>> expression, string phrase)
        {
            Validate.NotNull(expression, "No expression provided");
            Validate.StringNotEmpty(phrase, "No phrase provided");

            _queryBuilder.Insert(0, "(")
                .AppendFormat("OR ({0}:\"{1}\")) ",
                    expression.FieldName(),
                    phrase.ToLower());

            return this;
        }

        /// <summary>
        /// The specified field must contain the keyword specified
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> ContainsKeyword<TProperty>(Expression<Func<TSchema, TProperty>> expression, string keyword)
        {
            Validate.NotNull(expression, "No expression provided");
            Validate.StringNotEmpty(keyword, "No keyword provided");

            _queryBuilder.AppendFormat("({0}:{1}) ", 
                expression.FieldName(), 
                keyword.ToLower());
            
            return this;
        }

        /// <summary>
        /// Creates a AND keyword query
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> AndContainsKeyword<TProperty>(Expression<Func<TSchema, TProperty>> expression, string keyword)
        {
            Validate.NotNull(expression, "No expression provided");
            Validate.StringNotEmpty(keyword, "No keyword provided");

            _queryBuilder.Insert(0, "(")
                .AppendFormat("AND ({0}:{1})) ",
                    expression.FieldName(),
                    keyword.ToLower());

            return this;
        }

        /// <summary>
        /// Create a OR keyword query
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> OrContainsKeyword<TProperty>(Expression<Func<TSchema, TProperty>> expression, string keyword)
        {
            Validate.NotNull(expression, "No expression provided");
            Validate.StringNotEmpty(keyword, "No keyword provided");

            _queryBuilder.Insert(0, "(")
                .AppendFormat("OR ({0}:{1})) ",
                    expression.FieldName(),
                    keyword.ToLower());

            return this;
        }

        /// <summary>
        /// Create a NOT contains query
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> AndDoesntContain<TProperty>(Expression<Func<TSchema, TProperty>> expression, string phrase)
        {
            Validate.NotNull(expression, "No expression provided");
            Validate.StringNotEmpty(phrase, "No phrase provided");

            _queryBuilder.Insert(0, "(")
                .AppendFormat("-({0}:\"{1}\")) ",
                    expression.FieldName(),
                    phrase.ToLower());

            return this;
        }

        /// <summary>
        /// Creates a query that will return matches based on each word.
        /// Words can contain wild cards e.g. st*ng will create a match on string,
        /// Otherwise each word will be queryed with a suffix of *
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> WildCardLike<TProperty>(Expression<Func<TSchema, TProperty>> expression, string phrase)
        {
            Validate.NotNull(expression, "No expression provided");
            Validate.StringNotEmpty(phrase, "No phrase provided");

            _queryBuilder.Append(ToWildcardLikeQuery(expression.FieldName(), phrase, false, true));

            return this;
        }

        /// <summary>
        /// Creates a query that will return matches based on each word.
        /// Words can contain wild cards e.g. st*ng will create a match on string,
        /// Otherwise each word will be queryed with a suffix of *
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> OrWildcardLike<TProperty>(Expression<Func<TSchema, TProperty>> expression, string phrase)
        {
            Validate.NotNull(expression, "No expression provided");
            Validate.StringNotEmpty(phrase, "No phrase provided");

            _queryBuilder.Insert(0, "(")
                .Append("OR ")
                .Append(ToWildcardLikeQuery(expression.FieldName(), phrase, false, true).TrimEnd(' '))
                .Append(") ");

            return this;
        }

        /// <summary>
        /// Creates a query that will return matches based on NOT each word.
        /// Words can contain wild cards e.g. st*ng will create a match on string,
        /// Otherwise each word will be queryed with a suffix of *
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> AndNotWildcardLike<TProperty>(Expression<Func<TSchema, TProperty>> expression, string phrase)
        {
            Validate.NotNull(expression, "No expression provided");
            Validate.StringNotEmpty(phrase, "No phrase provided");

            _queryBuilder.Insert(0, "(")
                .Append("AND ")
                .Append(ToWildcardLikeQuery(expression.FieldName(), phrase, true, false).TrimEnd(' '))
                .Append(") ");

            return this;
        }

        /// <summary>
        /// Shared like implementation
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="phrase"></param>
        /// <param name="isNot"></param>
        /// <param name="isOr"></param>
        /// <returns></returns>
        private string ToWildcardLikeQuery(string fieldName, string phrase, bool isNot, bool isOr)
        {
            var queryBuilder = new StringBuilder();

            var terms =
                phrase.Trim()
                    .ToLower()
                    .Replace("-", " ")
                    .Split(' ')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => x.Trim() + "*").ToList();

            var count = 0;
            var seperator = isOr ? "OR" : "AND";
            var format = isNot ? "-({0}:{1}) " : "({0}:{1}) ";

            foreach (var term in terms)
            {
                count++;
                queryBuilder.AppendFormat(format, fieldName, term);

                if (count != terms.Count())
                    queryBuilder.AppendFormat("{0} ", seperator);
            }

            return queryBuilder.ToString();
        }

        /// <summary>
        /// Create a query that will look for specified words within a certain distance from each other,
        /// Distance is the number of words between matches
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="phrase"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> Proximity<TProperty>(Expression<Func<TSchema, TProperty>> expression, string phrase, int distance)
        {
            Validate.NotNull(expression, "No expression provided");
            Validate.StringNotEmpty(phrase, "No phrase provided");
            Validate.Range(0, 10, distance, "Distance must be between 0 and 10");

            _queryBuilder.AppendFormat("({0}:\"{1}\"~{2}) ",
                expression.FieldName(),
                phrase.ToLower(),
                distance);

            return this;
        }

        /// <summary>
        /// Creates a query that will perform a LIKE on each word specified.
        /// Similarity specifies how similar the specified phrase must be - 0.1 is the loosest where 0.9 
        /// is the strictest
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="phrase"></param>
        /// <param name="similarity"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> SimilarityLike<TProperty>(Expression<Func<TSchema, TProperty>> expression, string phrase,
            double similarity = 0.5)
        {
            Validate.NotNull(expression, "No expression provided");
            Validate.StringNotEmpty(phrase, "No phrase provided");
            Validate.Range(0.0, 0.9, similarity, "Similarity must be between 0.0 and 0.9");

            _queryBuilder.Append(ToSimilarityLike(expression.FieldName(), phrase, similarity, true, false))
                .Append(" ");

            return this;
        }

        /// <summary>
        /// Creates a AND query that will perform a LIKE on each word specified.
        /// Similarity specifies how similar the specified phrase must be - 0.1 is the loosest where 0.9 
        /// is the strictest
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="phrase"></param>
        /// <param name="similarity"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> AndSimilarityLike<TProperty>(Expression<Func<TSchema, TProperty>> expression, string phrase,
            double similarity = 0.5)
        {
            Validate.NotNull(expression, "No expression provided");
            Validate.StringNotEmpty(phrase, "No phrase provided");
            Validate.Range(0.0, 0.9, similarity, "Similarity must be between 0.0 and 0.9");
            
            _queryBuilder.Insert(0, "(")
                .Append("AND ")
                .Append(ToSimilarityLike(expression.FieldName(), phrase, similarity, false, false));

            _queryBuilder.Append(") ");

            return this;
        }

        /// <summary>
        /// Creates a AND NOT query that will perform a LIKE on each word specified.
        /// Similarity specifies how similar the specified phrase must be - 0.1 is the loosest where 0.9 
        /// is the strictest
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="phrase"></param>
        /// <param name="similarity"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> AndNotSimilarityLike<TProperty>(Expression<Func<TSchema, TProperty>> expression, string phrase,
            double similarity = 0.5)
        {
            Validate.NotNull(expression, "No expression provided");
            Validate.StringNotEmpty(phrase, "No phrase provided");
            Validate.Range(0.0, 0.9, similarity, "Similarity must be between 0.0 and 0.9");

            _queryBuilder.Insert(0, "(")
                 .Append("AND ")
                 .Append(ToSimilarityLike(expression.FieldName(), phrase, similarity, false, true));

            _queryBuilder.Append(") ");

            return this;
        }

        private string ToSimilarityLike(string fieldName, string phrase, double similarity, bool isOr, bool isNot)
        {
            var queryBuilder = new StringBuilder();

            var terms =
                phrase.Trim()
                    .ToLower()
                    .Replace("-", " ")
                    .Split(' ')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

            var count = 0;
            var seperator = isOr ? " OR" : " AND";
            var format = isNot ? "-({0}:{1}~{2})" : "({0}:{1}~{2})";

            foreach (var term in terms)
            {
                count++;
                queryBuilder.AppendFormat(
                    format,
                    fieldName,
                    term.ToLower(),
                    similarity);

                if (count != terms.Count())
                    queryBuilder.AppendFormat("{0} ", seperator);
            }

            return queryBuilder.ToString();
        }

        /// <summary>
        /// Creates a query that will search based on documents with a date field greater than
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> DateAfter<TProperty>(Expression<Func<TSchema, TProperty>> expression, DateTime date)
        {
            Validate.NotNull(expression, "No expression provided");

            WithInRange(expression, date.ToString(Constants.QueryDateFormat), "99999999");

            return this;
        }

        /// <summary>
        /// Creates a AND query that will search based on documents with a date field greater than
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> AndDateAfter<TProperty>(Expression<Func<TSchema, TProperty>> expression, DateTime date)
        {
            Validate.NotNull(expression, "No expression provided");

            _queryBuilder =
                new StringBuilder(string.Format("({0}AND {1}) ",
                    _queryBuilder,
                    Range(expression, date.ToString(Constants.QueryDateFormat), "99999999")
                        .TrimEnd(' ')));

            return this;
        }

        /// <summary>
        /// Creates a query that will search based on documents with a date field less than
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> DateBefore<TProperty>(Expression<Func<TSchema, TProperty>> expression, DateTime date)
        {
            Validate.NotNull(expression, "No expression provided");

            WithInRange(expression, "11111111", date.ToString(Constants.QueryDateFormat));

            return this;
        }

        /// <summary>
        /// Creates a AND query that will search based on documents with a date field less than
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> AndDateBefore<TProperty>(Expression<Func<TSchema, TProperty>> expression, DateTime date)
        {
            Validate.NotNull(expression, "No expression provided");
            
            _queryBuilder =
                new StringBuilder(string.Format("({0}AND {1}) ",
                    _queryBuilder,
                    Range(expression, "11111111", date.ToString(Constants.QueryDateFormat))
                        .TrimEnd(' ')));

            return this;
        }

        /// <summary>
        /// Query based on a field that is within a range incluseive
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> WithInRange<TProperty>(Expression<Func<TSchema, TProperty>> expression, object lower, object upper)
        {
            Validate.NotNull(expression, "No expression provided");
            Validate.NotNull(lower, "No lower param provided");
            Validate.NotNull(lower, "No upper param provided");

            _queryBuilder.Append(Range(expression, lower, upper));

            return this;
        }

        /// <summary>
        /// AND Query based on a field that is within a range inclusive
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public QueryBuilder<TSchema> AndWithInRange<TProperty>(Expression<Func<TSchema, TProperty>> expression, object lower, object upper)
        {
            Validate.NotNull(expression, "No expression provided");
            Validate.NotNull(lower, "No lower param provided");
            Validate.NotNull(lower, "No upper param provided");

            _queryBuilder =
                new StringBuilder(string.Format("({0}AND {1}) ",
                    _queryBuilder,
                    Range(expression, lower, upper)
                        .TrimEnd(' ')));

            return this;
        }

        private static string Range<TProperty>(Expression<Func<TSchema, TProperty>> expression, object lower, object upper)
        {
            return string.Format("({0}:[{1} TO {2}]) ",
                expression.FieldName(),
                lower,
                upper);
        }

        /// <summary>
        /// Return the lucene query
        /// Note: Dispose called on ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            Dispose();

            return _queryBuilder.ToString();
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