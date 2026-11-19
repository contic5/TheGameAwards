
using System.Xml.Linq;

namespace ReadDataFromWebsite
{
    class Program
    {
        public static async Task Main(string [] args)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://en.wikipedia.org/wiki/The_Game_Awards_2014");

                if(response.IsSuccessStatusCode)
                {
                    var xml= await response.Content.ReadAsStringAsync();

                    var doc=XDocument.Parse(xml);

                    var titles= from el in doc.Descendants("title") select el.Value;

                    // And finally, we'll print out the titles
                    foreach (var title in titles)
                    {
                        Console.WriteLine(title);
                    }
                }

                
            }
        }
    }
}