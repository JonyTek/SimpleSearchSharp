SimpleSearchSharp
===========

A fully configurable little framework that makes use of the lightning speed of lucene.net.

Contact: jonathontek@gmail.com

PM> Install-Package Bamboo.Sharp

```csharp
//Define you schema class
 public class Schema : BaseSchema
{
	//Specify your Id field
	[Identifier]
	//Specify wheather you would like to retrieve fields for later use
	[Store(FieldStore.Yes)]
	//Specify wheather you would like a field to be included in yopur index
	[Analyze(FieldIndex.Analyzed)]
	public int Id { get; set; }

	[Store(FieldStore.Yes)]
	[Analyze(FieldIndex.Analyzed)]
	public string Heading { get; set; }
}

//Configure
public static class Searcher
{
	private static readonly SimpleSearch<StandardAnalyzer, Schema> _simpleSearch;

	private const string PathToIndex = @"C:\Working\Sandbox\SimpleSearch\Index";

	static Searcher()
	{
		_simpleSearch = SimpleSearch<StandardAnalyzer, Schema>.Init(PathToIndex);
	}

	public static SearchResultSet<Schema> BasicSearch(string term)
	{
		var query = _simpleSearch.QueryBuilder
			.LikePhrase(schema => schema.Body, "jump")
			.ToString();

		var sort = _simpleSearch.SortBuilder
			.Build(schema => schema.Body);

		return _simpleSearch.Searcher.Search(query, sort);
	}
}

//Create custom queries at runtime
var query = _simpleSearch.QueryBuilder
                .ContainsPhrase(schema => schema.Text, "jump")
                .AndContainsPhrase(schema => schema.Text1, "car")
                .AndDateAfter(schema => schema.Date, new DateTime(2000, 1, 1))
                .WithInRange(schema => schema.CategoryId, 34, 36)
                .AndDoesntContain(schema => schema.Text, "c#")
                .ToString();
```