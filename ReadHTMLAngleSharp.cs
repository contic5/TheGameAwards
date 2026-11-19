using System;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

class ReadHTMLAngleSharp
{
    public static async Task Main(string [] args)
    {
        var config=Configuration.Default.WithDefaultLoader();
        using var context=BrowsingContext.New(config);

        string url="https://en.wikipedia.org/wiki/The_Game_Awards_2014";
        using var document=await context.OpenAsync(url);

        Console.WriteLine($"Title: {document.Title}");

        var paragraphs=document.QuerySelectorAll("p");
        foreach(var p in paragraphs)
        {
            Console.WriteLine(p.TextContent.Trim());
        }

        var links = document.QuerySelectorAll<IHtmlAnchorElement>("a");
        foreach (var link in links)
        {
            Console.WriteLine($"{link.TextContent.Trim()} -> {link.Href}");
        }
    }
}