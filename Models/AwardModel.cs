using System.Security.Cryptography.X509Certificates;

namespace TheGameAwards.Pages;

public class AwardModel
{
    public int ID {get; set;}
    public string Nominee {get; set;}
    public string Company {get; set;}

    public string Category {get; set;}

    public int Year {get; set;}

    public bool Winner {get; set;}

    public string Genres {get; set;}

    public double Metacritic {get; set;}

    public double Rating {get; set;}
    
    public AwardModel()
    {
        
    }
    
}