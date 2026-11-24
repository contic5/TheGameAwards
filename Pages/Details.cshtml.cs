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
using System.Collections;

namespace TheGameAwards.Pages
{
    public class DetailsModel : PageModel
    {

        [BindProperty(SupportsGet = true)]
        public int TargetID {get; set;}
        public void OnGet(int GameID)
        {
            Console.WriteLine("Game ID: "+GameID);
            TargetID=GameID;
            string filePath = "Data/Game_Awards_Data_and_RAWG_Stable.xlsx";

            // This single line reads the sheet and binds it to your model list
            IEnumerable<AwardModel> awards= MiniExcel.Query<AwardModel>(filePath,sheetName:"RAWG");

            AwardModel award=awards.Where(award=>award.ID==GameID).GetItemByIndex(0);
        }
    }
}
