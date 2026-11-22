using MiniExcelLibs;
using System.Collections.Generic;
using System.IO;

using CsvHelper;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AngleSharp.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TheGameAwards.Pages;

public class IndexModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int PageNumber {get; set;}=1;

    [BindProperty(SupportsGet = true)]
    public int Year {get; set;}

    [BindProperty(SupportsGet = true)]
    public string Company {get; set;}="";

    [BindProperty(SupportsGet = true)]
    public string Category {get; set;}="";

    public IEnumerable<AwardModel> awards {get; set;}


    public List<SelectListItem> SortByItems {get; set;}=[];
    
    [BindProperty(SupportsGet = true)]
    public string SortBy {get; set;}="";

    [BindProperty(SupportsGet = true)]
    public string Direction {get; set;}="-"; 

    public int TotalPages {get; set;}=0;
    
    public void OnGet()
    {
        string filePath = "Data/Game_Awards_Data_Stable.xlsx";

        // This single line reads the sheet and binds it to your model list
        awards = MiniExcel.Query<AwardModel>(filePath);

        TotalPages= (int)Math.Ceiling(1.0*awards.Count()/50);

        Console.WriteLine($"Sort By: {SortBy} Sort Direction {Direction}");

        if(SortBy!="")
        {
            awards=SharedFunctions.SortValues(awards,SortBy,Direction=="-").Select(item => (AwardModel)item).ToList();
        }

        awards= SharedFunctions.Slice(awards,PageNumber).Select(item => (AwardModel)item).ToList();
        SortByItems=SharedFunctions.MakeSelectListItemsFromColumnNames(awards.GetItemByIndex(0));
        foreach (var award in awards)
        {
            Console.WriteLine($"{award.Nominee} ({award.Year}))");
        }
    }
}
