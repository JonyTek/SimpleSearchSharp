SimpleSearchSharp
===========

A fully configurable little framework that makes use of the lightning speed of lucene.net.

Contact: jonathontek@gmail.com

PM> COMING SOON

```csharp
//Define you schema class
//Schema class must contain only public properties
public class Schema : BaseSchema
{
	//Specify your Id field
	//This attribute is mandatory
	[Identifier]
	//Specify wheather you would like to retrieve fields for later use
	[Store(FieldStore.Yes)]
	//Specify wheather you would like a field to be included in yopur index
	[Analyze(FieldIndex.Analyzed)]
	//Field will be returned in result set so that fields can be sorted by
	[Sortable("Identity")]
	public int Id { get; set; }

	//Store attributes are optional. If not specified they default to FieldStore.Yes
	//You may choose to not store a field with Store.No which will not be returned in a result set
	//Analyze attributes are optional. If not specified they default to FieldIndex.Analyzed
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
                .AndWithInRange(schema => schema.CategoryId, 34, 36)
                .AndDoesntContain(schema => schema.Text, "c#")
                .ToString();
```

Contained within the source code is a MVC/ angular project to get you started. 

Before beginning you will need to seed an Index. Within SimpleSearch.App.Specs.Seeder modify the path for the index and run the test, this will seed your data. Secondly just update the path in SimpleSearch.Angular.Search.Searcher to point to the same location....

Happy searching.