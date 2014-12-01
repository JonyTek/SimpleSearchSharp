using System;
using Lucene.Net.Analysis.Standard;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleSearch.Angular.Search;
using SimpleSearch.Core;

namespace SimpleSearch.App.Specs
{
    [TestClass]
    public class Seeder
    {
        private const string PathToIndex = @"C:\Working\Sandbox\SimpleSearch\Index";


        [TestMethod]
        public void Seed()
        {
            var simpleSearch = SimpleSearch<StandardAnalyzer, Schema>.Init(PathToIndex);
            var documents = new[]
            {
                new Schema
                {
                    Id = 1,
                    Heading = "The Letter",
                    Body = "It was November. Although it was not yet late, the sky was dark when I turned into Laundress Passage. Father had finished for the day, switched off the shop lights and closed the shutters; but so I would not come home to darkness he had left on the light over the stairs to the flat. Through the glass in the door it cast a foolscap rectangle of paleness onto the wet pavement, and it was while I was standing in that rectangle, about to turn my key in the door, that I first saw the letter. Another white rectangle, it was on the fifth step from the bottom, where I couldn't miss it.",
                    CreatedAt = new DateTime(2014, 11, 2),
                    CategoryId = 100
                },
                new Schema
                {
                    Id = 2,
                    Heading = "The Door",
                    Body = "I closed the door and put the shop key in its usual place behind Bailey's Advanced Principles of Geometry. Poor Bailey. No one has wanted his fat gray book for thirty years. Sometimes I wonder what he makes of his role as guardian of the bookshop keys. I don't suppose it's the destiny he had in mind for the masterwork that he spent two decades writing.",
                    CreatedAt = new DateTime(2014, 12, 20),
                    CategoryId = 100
                },
                new Schema
                {
                    Id = 3,
                    Heading = "The Feeling",
                    Body = "It gave me a queer feeling. Yesterday or the day before, while I had been going about my business, quietly and in private, some unknown person -- some stranger -- had gone to the trouble of marking my name onto this envelope. Who was it who had had his mind's eye on me while I hadn't suspected a thing?",
                    CreatedAt = new DateTime(2013, 1, 2),
                    CategoryId = 100
                },
                new Schema
                {
                    Id = 4,
                    Heading = "The Text",
                    Body = "Still in my coat and hat, I sank onto the stair to read the letter. (I never read without making sure I am in a secure position. I have been like this ever since the age of seven when, sitting on a high wall and reading The Water Babies, I was so seduced by the descriptions of underwater life that I unconsciously relaxed my muscles. Instead of being held buoyant by the water that so vividly surrounded me in my mind, I plummeted to the ground and knocked myself out. I can still feel the scar under my fringe now. Reading can be dangerous.)",
                    CreatedAt = new DateTime(2014, 11, 2),
                    CategoryId = 101
                },
                new Schema
                {
                    Id = 5,
                    Heading = "The Opening",
                    Body = "I opened the letter and pulled out a sheaf of half a dozen pages, all written in the same laborious script. Thanks to my work, I am experienced in the reading of difficult manuscripts. There is no great secret to it. Patience and practice are all that is required. That and the willingness to cultivate an inner eye. When you read a manuscript that has been damaged by water, fire, light or just the passing of the years, your eye needs to study not just the shape of the letters but other marks of production. The speed of the pen. The pressure of the hand on the page. Breaks and releases in the flow. You must relax. Think of nothing. Until you wake into a dream where you are at once a pen flying over vellum and the vellum itself with the touch of ink tickling your surface. Then you can read it. The intention of the writer, his thoughts, his hesitations, his longings and his meaning. You can read as clearly as if you were the very candlelight illuminating the page as the pen speeds over it.",
                    CreatedAt = new DateTime(2014, 4, 12),
                    CategoryId = 101
                },
            };


            simpleSearch.Index.AddUpdateLuceneIndex(documents);
            var count = simpleSearch.Index.Length();
        }
    }
}
