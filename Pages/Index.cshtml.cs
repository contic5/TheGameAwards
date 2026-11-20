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

namespace TheGameAwards.Pages;

public class IndexModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int PageNumber {get; set;}=1;

    public IEnumerable<AwardModel> awards {get; set;}
    public void OnGet()
    {
        string filePath = "Data/Game_Awards_Data_Stable.xlsx";

        // This single line reads the sheet and binds it to your model list
        awards = MiniExcel.Query<AwardModel>(filePath);

        foreach (var award in awards)
        {
            Console.WriteLine($"{award.Nominee} ({award.Year}))");
        }
    }
}
