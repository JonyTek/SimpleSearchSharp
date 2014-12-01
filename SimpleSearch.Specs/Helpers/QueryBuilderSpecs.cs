using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleSearch.Core.Helpers;

namespace SimpleSearch.Specs.Helpers
{
    [TestClass]
    public class QueryBuilderSpecs
    {
        [TestCleanup]
        public void CleanUp()
        {
            QueryBuilder<BasicSchema2>.Instance.Dispose();
        }

        [TestMethod]
        public void ShouldCreateAPhraseQuery()
        {
            const string expected = "(Text:\"abc\") ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .ContainsPhrase(schema => schema.Text, "ABC")
                .ToString();

            result.Should().Be(expected);
        }

        [TestMethod]
        public void ShouldCreateAnAndPhraseQuery()
        {
            const string expected = "((Text:\"jump\") AND (Text1:\"car\")) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .ContainsPhrase(schema => schema.Text, "jump")
                .AndContainsPhrase(schema => schema.Text1, "car")
                .ToString();

            result.Should().Be(expected);
        }

        [TestMethod]
        public void ShouldCreateAnOrPhraseQuery()
        {
            const string expected = "((Text:\"jump\") OR (Text1:\"car\")) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .ContainsPhrase(schema => schema.Text, "jump")
                .OrContainsPhrase(schema => schema.Text1, "car")
                .ToString();

            result.Should().Be(expected);
        }

        [TestMethod]
        public void ShouldCreateAKeywordQuery()
        {
            const string expected = "(Text:abc) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .ContainsKeyword(schema => schema.Text, "ABC")
                .ToString();

            result.Should().Be(expected);
        }

        [TestMethod]
        public void ShouldCreateAnAndKeywordQuery()
        {
            const string expected = "((Text:jump) AND (Text1:car)) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .ContainsKeyword(schema => schema.Text, "jump")
                .AndContainsKeyword(schema => schema.Text1, "car")
                .ToString();

            result.Should().Be(expected);
        }

        [TestMethod]
        public void ShouldCreateAnOrKeywordQuery()
        {
            const string expected = "((Text:jump) OR (Text1:car)) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .ContainsKeyword(schema => schema.Text, "jump")
                .OrContainsKeyword(schema => schema.Text1, "car")
                .ToString();

            result.Should().Be(expected);
        }

        [TestMethod]
        public void ShouldCreateAnMixedQuery()
        {
            const string expected = "(((Text:jump) OR (Text1:car)) AND (Text:\"some text\")) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .ContainsKeyword(schema => schema.Text, "jump")
                .OrContainsKeyword(schema => schema.Text1, "car")
                .AndContainsPhrase(schema => schema.Text, "some text")
                .ToString();

            result.Should().Be(expected);
        }

        [TestMethod]
        public void ShouldCreateAnNotContainsQuery()
        {
            const string expected = "(((Text:jump) OR (Text1:car)) -(Text:\"some text\")) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .ContainsKeyword(schema => schema.Text, "jump")
                .OrContainsKeyword(schema => schema.Text1, "car")
                .AndDoesntContain(schema => schema.Text, "some text")
                .ToString();

            result.Should().Be(expected);
        }
        
        [TestMethod]
        public void ShouldCreateAnWildcardQuery()
        {
            const string expected = "(Text:foo*) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .WildCardLike(schema => schema.Text, "foo")
                .ToString();

            result.Should().Be(expected);
        }

        [TestMethod]
        public void ShouldCreateAnWildcardQueryWithMultipleTerms()
        {
            const string expected = "(Text:foo*) OR (Text:bar*) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .WildCardLike(schema => schema.Text, "foo bar")
                .ToString();

            result.Should().Be(expected);
        }
        //NEED TO CREATE SEARCH SPECS
        [TestMethod]
        public void ShouldCreateAnOrWildcardQuery()
        {
            var expected = "((Text:foo*) OR (Text1:the*) OR (Text1:sky*)) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .WildCardLike(schema => schema.Text, "foo")
                .OrWildcardLike(schema => schema.Text1, "the sky")
                .ToString();

            result.Should().Be(expected);

            expected = "(((Text:foo*) OR (Text1:the*) OR (Text1:sky*)) OR (Text1:ttt*)) ";
            result = QueryBuilder<BasicSchema2>
                .Instance
                .WildCardLike(schema => schema.Text, "foo")
                .OrWildcardLike(schema => schema.Text1, "the sky")
                .OrWildcardLike(schema => schema.Text1, "ttt")
                .ToString();

            result.Should().Be(expected);
        }
        //NEED TO CREATE SEARCH SPECS
        [TestMethod]
        public void ShouldCreateAnAndNotWildcardQuery()
        {
            const string expected = "((Text:foo*) AND -(Text1:the*) AND -(Text1:sky*)) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .WildCardLike(schema => schema.Text, "foo")
                .AndNotWildcardLike(schema => schema.Text1, "the sky")
                .ToString();

            result.Should().Be(expected);
        }

        [TestMethod]
        public void ShouldCreateAnProximityQuery()
        {
            const string expected = "(Text1:\"quick over\"~2) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .Proximity(schema => schema.Text1, "quick over", 2)
                .ToString();

            result.Should().Be(expected);
        }
        //NEED TO CREATE SEARCH SPECS
        [TestMethod]
        public void ShouldCreateAnSimilariryLikeQuery()
        {
            var expected = "(Text:foo~0.9) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .SimilarityLike(schema => schema.Text, "foo", 0.9)
                .ToString();

            result.Should().Be(expected);

            expected = "(Text:foo~0.9) OR (Text:bar~0.9) ";
            result = QueryBuilder<BasicSchema2>
                .Instance
                .SimilarityLike(schema => schema.Text, "foo bar", 0.9)
                .ToString();

            result.Should().Be(expected);
        }

        //NEED TO CREATE SEARCH SPECS
        [TestMethod]
        public void ShouldCreateAnAndSimilariryLikeQuery()
        {
            var expected = "((Text:foo~0.9) AND (Text:bar~0.1)) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .SimilarityLike(schema => schema.Text, "foo", 0.9)
                .AndSimilarityLike(schema => schema.Text, "bar", 0.1)
                .ToString();

            result.Should().Be(expected);

            expected = "((Text:foo~0.9) AND (Text:bar~0.1) AND (Text:cat~0.1)) ";
            result = QueryBuilder<BasicSchema2>
                .Instance
                .SimilarityLike(schema => schema.Text, "foo", 0.9)
                .AndSimilarityLike(schema => schema.Text, "bar cat", 0.1)
                .ToString();

            result.Should().Be(expected);
        }
        //NEED TO CREATE SEARCH SPECS
        [TestMethod]
        public void ShouldCreateAnAndNotSimilariryLikeQuery()
        {
            var expected = "((Text:foo~0.9) AND -(Text:bar~0.1)) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .SimilarityLike(schema => schema.Text, "foo", 0.9)
                .AndNotSimilarityLike(schema => schema.Text, "bar", 0.1)
                .ToString();

            result.Should().Be(expected);

            expected = "((Text:foo~0.9) AND -(Text:bar~0.1) AND -(Text:cat~0.1)) ";
            result = QueryBuilder<BasicSchema2>
                .Instance
                .SimilarityLike(schema => schema.Text, "foo", 0.9)
                .AndNotSimilarityLike(schema => schema.Text, "bar cat", 0.1)
                .ToString();

            result.Should().Be(expected);
        }

        [TestMethod]
        public void ShouldCreateADateAfterQuery()
        {
            const string expected = "(Date:[20000101 TO 99999999]) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .DateAfter(schema => schema.Date, new DateTime(2000, 1, 1))
                .ToString();

            result.Should().Be(expected);
        }

        [TestMethod]
        public void ShouldCreateADateAndAfterQuery()
        {
            const string expected = "((Text1:\"quick over\"~2) AND (Date:[20000101 TO 99999999])) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .Proximity(schema => schema.Text1, "quick over", 2)
                .AndDateAfter(schema => schema.Date, new DateTime(2000, 1, 1))
                .ToString();

            result.Should().Be(expected);
        }

        [TestMethod]
        public void ShouldCreateADateBeforeQuery()
        {
            const string expected = "(Date:[11111111 TO 20000101]) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .DateBefore(schema => schema.Date, new DateTime(2000, 1, 1))
                .ToString();

            result.Should().Be(expected);
        }

        [TestMethod]
        public void ShouldCreateADateAndBeforeQuery()
        {
            const string expected = "((Text1:\"quick over\"~2) AND (Date:[11111111 TO 20000101])) ";
            var result = QueryBuilder<BasicSchema2>
                .Instance
                .Proximity(schema => schema.Text1, "quick over", 2)
                .AndDateBefore(schema => schema.Date, new DateTime(2000, 1, 1))
                .ToString();

            result.Should().Be(expected);
        }
       
    }
}