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
    public int TargetYear {get; set;}

    [BindProperty(SupportsGet = true)]
    public string TargetCompany {get; set;}="";

    [BindProperty(SupportsGet = true)]
    public string TargetCategory {get; set;}="";

    [BindProperty(SupportsGet = true)]
    public string TargetGenre {get; set;}="";

    public IEnumerable<AwardModel> Awards {get; set;}


    public List<SelectListItem> SortByItems {get; set;}=[];
    
    [BindProperty(SupportsGet = true)]
    public string SortBy {get; set;}="";

    [BindProperty(SupportsGet = true)]
    public string Direction {get; set;}="-"; 

    public int TotalPages {get; set;}=0; 

    public IEnumerable<string> AllGenres {get; set;}=[];

    public IEnumerable<string> AllCategories {get; set;}=[];
    public IEnumerable<string> AllCompanies {get; set;}=[];

    public IEnumerable<string> GetAllGenres(IEnumerable<AwardModel> Awards)
    {
        HashSet<string> uniqueGenres=new HashSet<string>();
        foreach(var award in Awards)
        {
            if(award.Genres!=null&&award.Genres!="")
            {
                string [] genres=award.Genres.Split(",");
                foreach(string genre in genres)
                {
                    uniqueGenres.Add(genre);
                }
            }
        }
        List<string> res=uniqueGenres.ToList();
        res.Sort();
        return res;
    }
    public void OnGet()
    {
        string filePath = "Data/Game_Awards_Data_and_RAWG_Stable.xlsx";

        // This single line reads the sheet and binds it to your model list
        Awards = MiniExcel.Query<AwardModel>(filePath,sheetName:"Game_Awards");

        TotalPages= (int)Math.Ceiling(1.0*Awards.Count()/50);

        AllCategories=SharedFunctions.GetUniqueValues(Awards,"Category").Select(category=>category.ToString());

        AllCompanies=SharedFunctions.GetUniqueValues(Awards,"Company").Select(category=>category.ToString());


        AllGenres=this.GetAllGenres(Awards);
        Console.WriteLine(string.Join(',',AllGenres));


        if(SortBy!="")
        {
            Console.WriteLine($"Sort By: {SortBy} Sort Direction {Direction}");

            //Sort values
            Awards=SharedFunctions.SortValues(Awards,SortBy,Direction=="-").Select(item => (AwardModel)item).ToList();
        }

        if(TargetYear!=0)
        {
            Console.WriteLine(TargetYear);
            Awards=Awards.Where(award=>award.Year==TargetYear);
        }

        if(TargetGenre!="")
        {
            Console.WriteLine(TargetGenre);
            Awards=Awards.Where(award=>award.Genres.Contains(TargetGenre));
        }

        if(TargetCompany!="")
        {
            Console.WriteLine(TargetCompany);   
            Awards=Awards.Where(award=>award.Company==TargetCompany);
        }

        if(TargetCategory!="")
        {
            Console.WriteLine(TargetCategory); 
            Awards=Awards.Where(award=>award.Category==TargetCategory);
        }

        //Filter Awards to current page only
        Awards= SharedFunctions.Slice(Awards,PageNumber).Select(item => (AwardModel)item).ToList();

        //Make sort by select items
        SortByItems=SharedFunctions.MakeSelectListItemsFromColumnNames(Awards.GetItemByIndex(0));
    }
}
